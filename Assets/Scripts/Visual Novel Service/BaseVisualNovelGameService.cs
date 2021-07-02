using System;
using System.Linq;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Video;

namespace VisualNovel.Service
{
	/// <summary>
	/// Базовая реализация интерфейса IVisualNovelGameServise
	/// </summary>
	public class BaseVisualNovelGameService : IVisualNovelGameService
	{
		#region Fields

		private readonly MonoBehaviour _monoBehaviour;
		private readonly AudioSource _audioSource;
		private readonly VideoPlayer _videoPlayer;
		private readonly GameContext _context;

		private float _speed;
		private Coroutine _coroutine;
		private Coroutine _startCoroutine;
		private GameContextItem _currentGameContextItem;

		#endregion

		#region .ctr

		public BaseVisualNovelGameService( MonoBehaviour monoBehaviour,
									  GameContext context,
									  AudioSource audioSource,
									  VideoPlayer videoPlayer )
		{
			_monoBehaviour = monoBehaviour;
			_context = context;
			_audioSource = audioSource;
			_videoPlayer = videoPlayer;
		}

		#endregion

		#region Properties

		public VisualNovelOption VisualNovelOption { get; private set; }
		public int Index { get; private set; }
		public int GetMaxIndex { get => _context.GameContextItems.Count - 1; }
		public Action<string, string> ActionUI { get; set; }
		public Action<GameObject, Sprite> ActionNewFrame { get; set; }
		public Action<AudioClip> ActionBackground { get; set; }
		public Action<VideoClip> ActionStartVideo { get; set; }
		public Action ActionStart { get; set; }
		public Action<string, string> ActionStartPreview { get; set; }
		public Action ActionExit { get; set; }
		public Action ActionExitEvents { get; set; }

		#endregion

		public void SetOption(VisualNovelOption visualNovelOption)
		{
			VisualNovelOption = visualNovelOption;
		}

		public void SetSpeed(float speed)
		{
			_speed = speed;
		}

		public void SetIndex(int index)
		{
			if(index >= _context.GameContextItems.Count)
			{
				ActionExitEvents?.Invoke();
				ActionExit?.Invoke();

				return;
			}

			if(index <= 0)
				index = 0;

			if (_coroutine != null)
			{
				_monoBehaviour.StopCoroutine(_coroutine);
				_coroutine = null;
				ActionUI?.Invoke(_currentGameContextItem.Text, _currentGameContextItem.PersonName);
			} else
			{
				Index = index;

				_currentGameContextItem = _context.GameContextItems[Index];
				_coroutine = _monoBehaviour.StartCoroutine(StartScene());
			}
		}

		public void NextIndex()
		{
			SetIndex(Index + 1);
		}

		public void LastIndex()
		{
			if (_coroutine != null)
			{
				_monoBehaviour.StopCoroutine(_coroutine);
				_coroutine = null;
			}

			SetIndex(Index - 1);
		}

		public void Start()
		{
			_startCoroutine = _monoBehaviour.StartCoroutine(StartVideo());
		}

		public void MaxIndex()
		{
			SetIndex(_context.GameContextItems.Count - 1);
		}

		public IEnumerable<IHistory> GetListFromIndex()
		{
			return _context.GameContextItems.Take(Index + 1)
											.Select(e => new BaseHistory
											{
												Name = e.PersonName,
												Text = e.Text
											});
		}

		#region Methond

		private IEnumerator StartVideo()
		{
			yield return new WaitForSeconds( VisualNovelOption.DelayStartVideo );

			if(_context.GameContextStartItem.VideoClip)
			{
				ActionStartVideo?.Invoke( _context.GameContextStartItem.VideoClip );

				yield return new WaitForSeconds( (float)_context.GameContextStartItem.VideoClip.length );
			}

			ActionStartPreview?.Invoke( _context.GameContextStartItem.Header ?? string.Empty, string.Empty );

			yield return new WaitForSeconds(VisualNovelOption.DelayHeaderText);

			ActionStartPreview?.Invoke( _context.GameContextStartItem.Name ?? string.Empty, _context.GameContextStartItem.Author );

			yield return new WaitForSeconds(VisualNovelOption.DelayAuthorText);

			ActionStart?.Invoke();

			SetIndex(0);

			_startCoroutine = null;
			yield break;
		}

		private IEnumerator StartScene()
		{
			var delay = new WaitForSeconds(_speed);

			AudioClip backgroundClip = _currentGameContextItem.BackgroundAudio != null ? _currentGameContextItem.BackgroundAudio
									   : _context.GameContextItems.Select((value, i) => (value, i))
																  .Where(e => e.i <= Index && e.value.BackgroundAudio != null)
																  .LastOrDefault().value?.BackgroundAudio;
			ActionBackground?.Invoke(backgroundClip);

			_audioSource.clip = _currentGameContextItem.Audio;
			_audioSource.Play();

			Sprite background = _currentGameContextItem.Background != null ? _currentGameContextItem.Background
								: _context.GameContextItems.Select((value, i) => (value, i))
																  .Where(e => e.i <= Index && e.value.Background != null)
																  .LastOrDefault().value?.Background;
			ActionNewFrame?.Invoke(_currentGameContextItem.Person, background);

			string currentText = string.Empty;

			foreach (var item in _currentGameContextItem.Text.Select((value, i) => (value, i)))
			{
				currentText = _currentGameContextItem.Text.Substring(0, item.i);
				currentText += "<color=#00000000>" + _currentGameContextItem.Text.Substring(item.i) + "</color>";

				ActionUI?.Invoke(currentText, _currentGameContextItem.PersonName);

				yield return delay;
			}

			currentText = _currentGameContextItem.Text;
			ActionUI?.Invoke(currentText, _currentGameContextItem.PersonName);

			_coroutine = null;
			yield break;
		}

		#endregion
	}
}
