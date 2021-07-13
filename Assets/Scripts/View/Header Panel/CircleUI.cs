using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace VisualNovel.UI
{
	public class CircleUI : MonoBehaviour
	{
		[SerializeField] private float MinSize;
		[SerializeField] private float MaxSize;
		[SerializeField] private Color _bonusColor;

		private RawImage _image;

		private void Awake()
		{
			_image = GetComponent<RawImage>();
		}

		public void SetMin()
		{
			SetSize( Vector2.one * MinSize );
		}

		public void SetMax()
		{
			SetSize( new Vector2( MaxSize, MinSize ) );
		}

		public void SetColor( bool isBonus )
		{
			_image.color = isBonus ? _bonusColor : Color.white;
		}

		private void SetSize( Vector2 value )
		{
			GetComponent<RectTransform>().sizeDelta = Vector2.one * value;
		}
	}
}