using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace VisualNovel.Service
{
	[CreateAssetMenu( fileName = "new Header Info", menuName = "Visual Novel/Header Info", order = 0 )]
	public class HeaderInfo : ScriptableObject
	{
		public GameContext Context;
		public string Name;
		public string Path;
		public Sprite Background;
	}
}
