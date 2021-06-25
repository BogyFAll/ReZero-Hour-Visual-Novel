
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

public class GameView : MonoBehaviour, IView, IPointerClickHandler
{
	public GameContext Context;

	[Space]
	[Header("Panels")]
	[SerializeField] private GameObject _gamePanel;
	[SerializeField] private GameObject _videoPanel;
	[SerializeField] private GameObject _previewPanel;

	[Space]
	[Header("UI")]
	[SerializeField] private TextMeshProUGUI _mainText;
	[SerializeField] private TextMeshProUGUI _nameText;
	[SerializeField] private TextMeshProUGUI _headerText;
	[SerializeField] private Image _backgroundImage;
	[SerializeField] private Transform _personPanelTransform;
	[SerializeField] private GameObject _personSpritePref;
	[SerializeField] private AudioSource _backgroundAudioSource;
	[SerializeField] private VideoPlayer _videoPlayer;

	[Space]
	[SerializeField] private Image _visualEffect;

	#region Properties

	private IVisualNovelGameService _visualNovelGameService;

	#endregion

	#region Default Methods

	private void Awake()
	{
		_visualNovelGameService = new VisualNovelGameService(this, Context, GetComponent<AudioSource>(), _videoPlayer);
		_visualNovelGameService.SetSpeed(0.02f);
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
		_visualNovelGameService.Start();
	}

	public void OnPointerClick( PointerEventData eventData )
	{
		_visualNovelGameService.NextIndex();
	}

	#endregion

	#region UI

	private void SetText(string text, string name)
	{
		_gamePanel.SetActive(true);
		_mainText.text = text;
		_nameText.text = name;
	}

	private void SetFrame(Sprite person, Sprite background)
	{
		for (int i = 0; i < _personPanelTransform.childCount; i++)
			Destroy(_personPanelTransform.GetChild(i).gameObject);

		_backgroundImage.sprite = background ? background : _backgroundImage.sprite;
		
		if(person)
			Instantiate(_personSpritePref, _personPanelTransform);
	}

	private void SetBackgroundAudio(AudioClip audio)
	{
		if(audio && audio != _backgroundAudioSource.clip)
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
		_videoPlayer.Play();
	}

	[ContextMenu("Game Controller/Start")]
	private void StartGame()
	{
		StartCoroutine( PushVisualEffect() );

		_gamePanel.SetActive(true);
		_previewPanel.SetActive(false);
		_videoPanel.SetActive(false);
	}

	private void StartHeader(string text)
	{
		StartCoroutine( PushVisualEffect() );

		_gamePanel.SetActive(false);
		_previewPanel.SetActive(true);
		_videoPanel.SetActive(false);

		_headerText.text = text;
	}

	private void Close()
	{
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

		float delta = 1f / 1200f;
		var delay = new WaitForSeconds(delta);

		while (color.a <= 1)
		{
			color.a += delta;
			_visualEffect.color = color;

			yield return delta;
		}

		PlayerPrefs.SetString("Level1", "true");

		SceneManager.LoadSceneAsync(0);

		yield break;
	}

	private IEnumerator PushVisualEffect()
	{
		_visualEffect.gameObject.SetActive( true );
		_visualEffect.color = new Color(0, 0, 0, 1);

		Color color = _visualEffect.color;

		float delta = 1f / 400f;
		var delay = new WaitForSeconds( delta );

		while(color.a > 0)
		{
			color.a -= delta;
			_visualEffect.color = color;

			yield return delta;
		}

		_visualEffect.gameObject.SetActive( false );

		yield break;
	}

	#endregion

	#region Commands

	public void CommandHandler( string commandName )
	{
		throw new System.NotImplementedException();
	}

	#endregion
}