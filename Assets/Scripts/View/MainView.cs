using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using VisualNovel.Scene;
using VisualNovel.Service;
using VisualNovel.UI;

namespace VisualNovel.MainScene
{
	public class MainView : MonoBehaviour, IView
	{
		[Space]
		[SerializeField] private RawImage _visualEffect;
		[SerializeField] private GameObject _buttonsPanel;
		[SerializeField] private GameObject _optionView;
		[SerializeField] private HeaderPanel _headerPanel;

		[Space]
		[Header( "Audio" )]
		[SerializeField] private AudioSource _backgroundAudio;

		private void Start()
		{
			_optionView.GetComponent<OptionView>().LoadSettings();
		}

		public void CommandHandler(string commandName)
		{
			switch (commandName)
			{
				case "startGame":
					StartGameCommandHandler();
					break;
				case "optionGame":
					OptionGameCommandHandler();
					break;
				case "quitGame":
					QuitGameCommandHandler();
					break;
			}
		}

		private void StartGameCommandHandler()
		{
			StartCoroutine( LoadGame( 2, _headerPanel.SelectedGameContext ) );
		}

		private void OptionGameCommandHandler()
		{
			_optionView.SetActive(true);
			gameObject.SetActive(false);
		}

		private void QuitGameCommandHandler()
		{
#if UNITY_EDITOR
			EditorApplication.isPlaying = false;
#endif

			Application.Quit();
		}

		private IEnumerator LoadGame(int sceneIndex, GameContext gameContext)
		{
			SceneParameters.LoadGameContext = gameContext;

			_buttonsPanel.SetActive( false );

			_visualEffect.color = new Color( 1, 1, 1, 1 );

			Color color = _visualEffect.color;

			float delta = 1f / 20f;
			var delay = new WaitForSeconds( delta );

			float volumeSound = 0.05f; //Скорость приглушения звука

			while ( color.a > 0 )
			{
				color.a -= delta;
				_visualEffect.color = color;

				_backgroundAudio.volume -= volumeSound;

				yield return delay;
			}

			_visualEffect.gameObject.SetActive( false );

			SceneManager.LoadScene( sceneIndex );

			yield break;
		}

		[ContextMenu("Reset Prefs")]
		private void ResetPrefs()
		{
			PlayerPrefs.DeleteAll();
			SceneManager.LoadScene(1);
		}

		[ContextMenu("Add Prefs")]
		private void AddPrefs()
		{
			PlayerPrefs.SetString("Level1", "true");
			SceneManager.LoadScene(1);
		}

		[ContextMenu("Export")]
		private void ExportContext()
		{
			ExportToFile.Export( _headerPanel.SelectedGameContext );
		}
	}
}
