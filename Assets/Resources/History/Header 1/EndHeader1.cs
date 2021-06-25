using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace VisualNovel.Bonus1
{
	public class EndHeader1 : MonoBehaviour
	{
		public void Events()
		{
			PlayerPrefs.SetString("Level1", "true");
		}
	}
}
