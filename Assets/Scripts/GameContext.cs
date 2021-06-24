using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Video;

namespace VisualNovel.Service
{
	[CreateAssetMenu(fileName = "new Game Context", menuName = "Game Context", order = 0)]
	public class GameContext : ScriptableObject
	{
		public GameContextStartItem GameContextStartItem;
		public List<GameContextItem> GameContextItems;
	}

	[System.Serializable]
	public class GameContextItem
	{
		public string PersonName;
		[TextArea(0, 30)]
		public string Text;

		public Sprite Person;
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
		public VideoClip VideoClip;
	}
}
