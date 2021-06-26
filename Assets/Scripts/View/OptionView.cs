using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.UI;

namespace VisualNovel.MainScene
{
	public class OptionView : MonoBehaviour, IView
	{
		#region Fields

		[SerializeField] private GameObject _mainView;

		[Space]
		[Header("Speed Text")]
		[SerializeField] private Slider _speedTextSlider;
		[SerializeField] private TextMeshProUGUI _speedText;

		#endregion

		#region Default Methods

		private void Start()
		{
			float speedText = PlayerPrefs.GetFloat("SpeedText", _speedTextSlider.minValue);
			_speedTextSlider.value = speedText;
		}

		#endregion

		#region Options

		public void OnTextSpeed(System.Single value)
		{
			float speedText = ((int)((1 / value) * 100f) / 100f);
			_speedText.text = speedText.ToString() + " сим/сек";

			PlayerPrefs.SetFloat("SpeedText", value);
		}

		#endregion

		#region Commands

		public void CommandHandler(string commandName)
		{
			switch(commandName)
			{
				case "activeMainMenu":
					ActiveMainView();
					break;
			}
		}

		private void ActiveMainView()
		{
			gameObject.SetActive(false);
			_mainView.SetActive(true);
		}

		#endregion
	}
}
