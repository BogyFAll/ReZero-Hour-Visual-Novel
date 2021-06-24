using System;
using System.Linq;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Video;

namespace VisualNovel.Service
{
	public class VisualNovelGameService : IVisualNovelGameService
	{
		#region Fields

		private readonly MonoBehaviour _monoBehaviour;
		private readonly AudioSource _audioSource;
		private readonly VideoPlayer _videoPlayer;
		private readonly GameContext _context;

		private int _index;
		private float _speed;
		private Coroutine _coroutine;
		private Coroutine _startCoroutine;
		private GameContextItem _currentGameContextItem;

		#endregion

		#region .ctr

		public VisualNovelGameService(MonoBehaviour monoBehaviour,
									  GameContext context,
									  AudioSource audioSource,
									  VideoPlayer videoPlayer)
		{
			_monoBehaviour = monoBehaviour;
			_context = context;
			_audioSource = audioSource;
			_videoPlayer = videoPlayer;
		}

		#endregion

		#region Properties

		public Action<string, string> ActionUI { get; set; }
		public Action<Sprite, Sprite> ActionNewFrame { get; set; }
		public Action<AudioClip> ActionBackground { get; set; }
		public Action<VideoClip> ActionStartVideo { get; set; }
		public Action ActionStart { get; set; }
		public Action<string> ActionStartPreview { get; set; }

		#endregion

		public void SetSpeed(float speed)
		{
			_speed = speed;
		}

		public void SetIndex(int index)
		{
			if(_coroutine != null)
			{
				_monoBehaviour.StopCoroutine(_coroutine);
				_coroutine = null;
				ActionUI?.Invoke(_currentGameContextItem.Text, _currentGameContextItem.PersonName);
			} else
			{
				_index = index;

				if (_index >= _context.GameContextItems.Count || _index < 0)
					_index = index >= _context.GameContextItems.Count || index < 0 ? 0 : index;

				_currentGameContextItem = _context.GameContextItems[_index];
				_coroutine = _monoBehaviour.StartCoroutine(StartScene());
			}
		}

		public void NextIndex()
		{
			SetIndex(_index + 1);
		}

		public void LastIndex()
		{
			SetIndex(_index - 1);
		}

		public void Start()
		{
			_startCoroutine = _monoBehaviour.StartCoroutine(StartVideo());
		}

		private IEnumerator StartVideo()
		{
			yield return new WaitForSeconds(2f);

			ActionStartVideo?.Invoke(_context.GameContextStartItem.VideoClip);

			yield return new WaitForSeconds((float)_context.GameContextStartItem.VideoClip.length);

			ActionStartPreview?.Invoke(_context.GameContextStartItem.Header);

			yield return new WaitForSeconds(3f);

			ActionStartPreview?.Invoke(_context.GameContextStartItem.Name);

			yield return new WaitForSeconds(3f);

			ActionStart?.Invoke();

			SetIndex(0);

			_startCoroutine = null;
			yield break;
		}

		private IEnumerator StartScene()
		{
			var delay = new WaitForSeconds(_speed);

			_audioSource.clip = _currentGameContextItem.Audio;
			_audioSource.Play();

			AudioClip backgroundClip = _currentGameContextItem.BackgroundAudio != null ? _currentGameContextItem.BackgroundAudio
									   : _context.GameContextItems.Select((value, i) => (value, i))
																  .Where(e => e.i <= _index && e.value.BackgroundAudio != null)
																  .LastOrDefault().value.BackgroundAudio;

			ActionBackground?.Invoke(backgroundClip);

			string currentText = string.Empty;

			Sprite background = _currentGameContextItem.Background != null
								? _currentGameContextItem.Background
								: _context.GameContextItems.Select((value, i) => (value, i))
														   .Where(v => v.i <= _index && v.value.Background != null)
														   .LastOrDefault().value.Background;

			ActionNewFrame?.Invoke(_currentGameContextItem.Person, background);

			if (_index == 0)
				yield return new WaitForSeconds(2f);

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
	}
}
