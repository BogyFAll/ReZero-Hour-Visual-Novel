using System.Collections;
using System.Linq;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;
using VisualNovel.Service;
using TMPro;
using UnityEngine.Video;

public class GameView : MonoBehaviour, IPointerClickHandler
{
	public GameContext Context;

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

	#region Properties

	private IVisualNovelGameService _visualNovelGameService;

	#endregion

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
	}

	private void Start()
	{
		_visualNovelGameService.Start();
	}

	private void SetText(string text, string name)
	{
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

	private void StartGame()
	{
		_gamePanel.SetActive(true);
		_previewPanel.SetActive(false);
		_videoPanel.SetActive(false);
	}

	private void StartHeader(string text)
	{
		_gamePanel.SetActive(false);
		_previewPanel.SetActive(true);
		_videoPanel.SetActive(false);

		_headerText.text = text;
	}

	public void OnPointerClick(PointerEventData eventData)
	{
		_visualNovelGameService.NextIndex();
	}
}