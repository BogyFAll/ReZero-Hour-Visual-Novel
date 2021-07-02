
using System.Collections;
using System.Linq;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;
using UnityEngine.SceneManagement;
using VisualNovel.Service;
using TMPro;
using UnityEngine.Video;
using VisualNovel.Scene;
using VisualNovel.MainScene;

namespace VisualNovel.GameScene
{
	public class GameView : MonoBehaviour, IView, IPointerClickHandler
	{
		#region Fields

		public GameContext Context;

		[Space]
		[Header("Panels")]
		[SerializeField] private GameObject _optionView;
		[SerializeField] private GameObject _gamePanel;
		[SerializeField] private GameObject _textPanel;
		[SerializeField] private GameObject _videoPanel;
		[SerializeField] private GameObject _previewPanel;
		[SerializeField] private HistoryListPanel _listHistoryPanel;

		[Space]
		[Header("Prefabs")]
		[SerializeField] private GameObject _personSpritePref;
		[SerializeField] private GameObject _historyElementPref;

		[Space]
		[Header("UI")]
		[SerializeField] private TextMeshProUGUI _mainText;
		[SerializeField] private TextMeshProUGUI _nameText;
		[SerializeField] private TextMeshProUGUI _headerText;
		[SerializeField] private TextMeshProUGUI _authorText;
		[SerializeField] private Image _backgroundImage;
		[SerializeField] private Transform _personPanelTransform;
		[SerializeField] private AudioSource _backgroundAudioSource;
		[SerializeField] private VideoPlayer _videoPlayer;
		[Header( "Buttons Game Panel" )]
		[SerializeField] private GameObject _lastButton;
		[SerializeField] private GameObject _nextButton;

		[Space]
		[SerializeField] private Image _visualEffect;

		private bool _isNextFrame = false;
		private IVisualNovelGameService _visualNovelGameService;

		#endregion

		#region Default Methods

		private void Awake()
		{
			Context = SceneParameters.LoadGameContext ?? Context;
			SceneParameters.LoadGameContext = null;

			_visualNovelGameService = new BaseVisualNovelGameService(this, Context, GetComponent<AudioSource>(), _videoPlayer);
			_visualNovelGameService.SetOption( Context.VisualNovelOption );
			_visualNovelGameService.SetSpeed(PlayerPrefs.GetFloat("SpeedText", 0.1f));
			_visualNovelGameService.ActionNewFrame = SetFrame;
			_visualNovelGameService.ActionUI = SetText;
			_visualNovelGameService.ActionBackground = SetBackgroundAudio;
			_visualNovelGameService.ActionStartVideo = StartVideo;
			_visualNovelGameService.ActionStart = StartGame;
			_visualNovelGameService.ActionStartPreview = StartHeader;
			_visualNovelGameService.ActionExit = Close;
		}

		private void Start()
		{
			_optionView.GetComponent<OptionView>().LoadSettings();
			_optionView.GetComponent<OptionView>().SetGameOption();
			_visualNovelGameService.Start();
		}

		private void OnEnable()
		{
			if(_isNextFrame)
			{
				_visualNovelGameService.SetSpeed(PlayerPrefs.GetFloat("SpeedText", 0.1f));
				_visualNovelGameService.SetIndex(_visualNovelGameService.Index);
			}
		}

		public void OnPointerClick(PointerEventData eventData)
		{
			if (_isNextFrame)
				_visualNovelGameService.NextIndex();
		}

		#endregion

		#region Actions

		private void SetText(string text, string name)
		{
			_gamePanel.SetActive(true);
			_mainText.text = text;
			_nameText.text = name;
		}

		private void SetFrame(GameObject person, Sprite background)
		{
			_isNextFrame = true;

			for (int i = 0; i < _personPanelTransform.childCount; i++)
				Destroy(_personPanelTransform.GetChild(i).gameObject);

			_backgroundImage.sprite = background ? background : _backgroundImage.sprite;

			if (person != null)
				Instantiate(person, _personPanelTransform);

			_lastButton.SetActive( _visualNovelGameService.Index > 0 );
			_nextButton.SetActive( _visualNovelGameService.Index < _visualNovelGameService.GetMaxIndex );
		}

		private void SetBackgroundAudio(AudioClip audio)
		{
			if (audio && audio != _backgroundAudioSource.clip)
			{
				_backgroundAudioSource.clip = audio;
				_backgroundAudioSource.Play();
			}
		}

		private void StartVideo(VideoClip clip)
		{
			_gamePanel.SetActive(false);
			_previewPanel.SetActive(false);
			_videoPanel.SetActive(true);

			_videoPlayer.clip = clip;
		}

		[ContextMenu("Game Controller/Start")]
		private void StartGame()
		{
			StartCoroutine(PushVisualEffect());

			_gamePanel.SetActive(true);
			_previewPanel.SetActive(false);
			_videoPanel.SetActive(false);
		}

		private void StartHeader(string text, string author)
		{
			StartCoroutine(PushVisualEffect());

			_gamePanel.SetActive(false);
			_previewPanel.SetActive(true);
			_videoPanel.SetActive(false);

			_headerText.text = text;
			_authorText.text = author;
		}

		private void Close()
		{
			_isNextFrame = false;
			StartCoroutine(ExitVisualEffect());
		}

		#endregion

		#region Visual Effect

		private IEnumerator ExitVisualEffect()
		{
			_visualEffect.gameObject.SetActive(true);
			_visualEffect.color = new Color(0, 0, 0, 1);

			Color color = _visualEffect.color;
			color.a = 0f;

			float delta = 1f / 30f;
			var delay = new WaitForSeconds(delta);

			while (color.a <= 1)
			{
				color.a += delta;
				_visualEffect.color = color;

				yield return delay;
			}

			PlayerPrefs.SetString("Level1", "true");

			_gamePanel.SetActive( false );
			_visualNovelGameService = null;

			SceneManager.LoadScene(0);

			yield break;
		}

		private IEnumerator PushVisualEffect()
		{
			_visualEffect.gameObject.SetActive(true);
			_visualEffect.color = new Color(0, 0, 0, 1);

			Color color = _visualEffect.color;

			float delta = 1f / 25f;
			var delay = new WaitForSeconds(delta);

			while (color.a > 0)
			{
				color.a -= delta;
				_visualEffect.color = color;

				yield return delay;
			}

			_visualEffect.gameObject.SetActive(false);

			yield break;
		}

		#endregion

		#region Commands

		public void CommandHandler(string commandName)
		{
			switch (commandName)
			{
				case "mainMenu":
					SceneManager.LoadScene(0);
					break;
				case "visibleList":
					VisibleList();
					break;
				case "unvisibleList":
					UnvisibleList();
					break;
				case "nextPage":
					_visualNovelGameService.NextIndex();
					break;
				case "lastPage":
					_visualNovelGameService.LastIndex();
					break;
				case "activeOptionView":
						ActiveOptionViewCommandHandler();
					break;
			}
		}

		private void VisibleList()
		{
			_isNextFrame = false;

			_listHistoryPanel.gameObject.SetActive(true);
			_textPanel.SetActive(false);

			_listHistoryPanel.VisibleResult(_visualNovelGameService.GetListFromIndex());
		}

		private void UnvisibleList()
		{
			_isNextFrame = true;

			_listHistoryPanel.gameObject.SetActive(false);
			_textPanel.SetActive(true);

			_listHistoryPanel.DeleteContent();
		}

		private void ActiveOptionViewCommandHandler()
		{
			gameObject.SetActive(false);
			_optionView.SetActive(true);
		}

		[ContextMenu("Go Final Frame")]
		private void GoFinalFram()
		{
			_visualNovelGameService.MaxIndex();
		}

		#endregion
	}
}