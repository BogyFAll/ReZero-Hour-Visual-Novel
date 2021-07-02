using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.Video;

namespace VisualNovel.Service
{
	[CreateAssetMenu(fileName = "new Game Context", menuName = "Game Context", order = 0)]
	public class GameContext : ScriptableObject
	{
		public VisualNovelOption VisualNovelOption;
		public GameContextStartItem GameContextStartItem;
		public List<GameContextItem> GameContextItems;
		public GameContextExitItem GameContextExitItem;
	}

	[System.Serializable]
	public class GameContextItem
	{
		public string PersonName;
		[TextArea(0, 30)]
		public string Text;

		public GameObject Person;
		public Sprite Background;

		[Space]
		[Header("Audio")]
		public AudioClip Audio;
		public AudioClip BackgroundAudio;
	}

	[System.Serializable]
	public class GameContextStartItem
	{
		public string Header;
		public string Name;
		public string Author;
		public VideoClip VideoClip;
	}

	[System.Serializable]
	public class GameContextExitItem
	{
		public string Header;
		public UnityEvent Events;
	}
}
