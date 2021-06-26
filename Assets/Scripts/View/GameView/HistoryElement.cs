using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

namespace VisualNovel.GameScene
{
	public class HistoryElement	: MonoBehaviour
	{
		[SerializeField] private TextMeshProUGUI _textName;
		[SerializeField] private TextMeshProUGUI _text;

		public void SetText(string name, string text)
		{
			_textName.text = name;
			_text.text = text;
		}
	}
}
