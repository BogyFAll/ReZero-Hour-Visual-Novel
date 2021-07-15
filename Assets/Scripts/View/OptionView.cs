using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.UI;
using UnityEngine.Audio;
using UnityEngine.SceneManagement;

namespace VisualNovel.MainScene
{
	public class OptionView : MonoBehaviour, IView
	{
		#region Fields

		[SerializeField] private GameObject _mainView;
		[SerializeField] private AudioMixer _audioMixer;

		[Space]
		[Header("Speed Text")]
		[SerializeField] private Slider _speedTextSlider;
		[SerializeField] private TextMeshProUGUI _speedText;

		[Space]
		[Header("Background Sounds")]
		[SerializeField] private Slider _backgroundSoundsSlider;
		[SerializeField] private TextMeshProUGUI _backgroundSoundsText;

		[Space]
		[Header("Sounds")]
		[SerializeField] private Slider _soundsSlider;
		[SerializeField] private TextMeshProUGUI _soundsText;

		[Space]
		[SerializeField] private GameObject _mainMenuButton;

		#endregion

		#region Default Methods

		private void Start()
		{
			LoadSettings();
		}

		public void SetGameOption()
		{
			_mainMenuButton.SetActive( true );

		}

		public void LoadSettings()
		{
			float speedText = PlayerPrefs.GetFloat("SpeedText", 0.01f);
			_speedTextSlider.value = speedText;

			float backgroundSounds = PlayerPrefs.GetInt("BackgroundSounds", -40);
			_backgroundSoundsSlider.value = backgroundSounds;
			_audioMixer.SetFloat( "Background Group", backgroundSounds );

			float sounds = PlayerPrefs.GetInt("Sounds", -40);
			_soundsSlider.value = sounds;
			_audioMixer.SetFloat( "Sounds Group", sounds );
		}

		#endregion

		#region Options

		public void OnTextSpeed(System.Single value)
		{
			float speedText = ((int)((1 / value) * 100f) / 100f);
			_speedText.text = speedText.ToString() + " сим/сек";

			PlayerPrefs.SetFloat("SpeedText", value);
		}

		public void OnBackgroundSounds(System.Single value)
		{
			_backgroundSoundsText.text = ((int)(GetSound(value) * 100)).ToString();

			_audioMixer.SetFloat("Background Group", value);

			PlayerPrefs.SetInt("BackgroundSounds", int.Parse(value.ToString()));
		}

		public void OnSouds(System.Single value)
		{
			_soundsText.text = ( (int)( GetSound( value ) * 100 ) ).ToString();

			_audioMixer.SetFloat("Sounds Group", value);

			PlayerPrefs.SetInt("Sounds", int.Parse(value.ToString()));
		}

		private float GetSound(float value)
		{
			return ( -( ( value - ( -80 ) ) / ( -80 - 0 ) ) );
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
				case "goMainMenu":
					SceneManager.LoadScene( 0 );
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
