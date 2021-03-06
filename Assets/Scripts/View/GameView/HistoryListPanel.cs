using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using VisualNovel.GameScene;
using VisualNovel.Service;

namespace VisualNovel.UI
{
	public class HistoryListPanel : MonoBehaviour
	{
		[SerializeField] private Transform _content;
		[SerializeField] private ScrollRect _scrollRect;
		[SerializeField] private GameObject _historyElementPref;

		public void VisibleResult(IEnumerable<IHistory> histories)
		{
			DeleteContent();

			foreach (var item in histories)
			{
				if (string.IsNullOrEmpty(item.Name) && string.IsNullOrEmpty(item.Text))
					continue;

				var element = Instantiate(_historyElementPref, _content).GetComponent<HistoryElement>();
				element.SetText(item.Name, item.Text);
			}

			_scrollRect.normalizedPosition = new Vector2(0, 0);
		}

		public void DeleteContent()
		{
			for (int i = 0; i < _content.childCount; i++)
				Destroy(_content.GetChild(i).gameObject);
		}
	}
}
