using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using VisualNovel.Scene;
using VisualNovel.Service;

namespace VisualNovel.MainScene
{
	public class MainView : MonoBehaviour, IView
	{
		[Space]
		[Header("Contexts")]
		[SerializeField] private GameContext _bonusContext;
		[SerializeField] private GameContext _header1Context;

		[Space]
		[SerializeField] private Image _visualEffect;
		[SerializeField] private GameObject _buttonsPanel;
		[SerializeField] private GameObject _optionView;

		private void Start()
		{
			_optionView.GetComponent<OptionView>().LoadSettings();
		}

		public void CommandHandler(string commandName)
		{
			switch (commandName)
			{
				case "bonusGame":
					BonusGameCommandHandler();
					break;
				case "countiuneGame":
					CountiuneGameCommandHandler();
					break;
				case "newGame":
					NewGameCommandHandler();
					break;
				case "optionGame":
					OptionGameCommandHandler();
					break;
				case "quitGame":
					QuitGameCommandHandler();
					break;
			}
		}

		private void BonusGameCommandHandler()
		{
			StartCoroutine( LoadGame( 1, _bonusContext ) );
		}

		private void CountiuneGameCommandHandler()
		{
			SceneManager.LoadScene(1);
		}

		private void NewGameCommandHandler()
		{
			StartCoroutine( LoadGame( 1, _header1Context ) );
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

			_visualEffect.GetComponent<Animator>().enabled = false;
			_visualEffect.color = new Color( 1, 1, 1, 1 );

			Color color = _visualEffect.color;

			float delta = 1f / 500f;
			var delay = new WaitForSeconds( delta );

			while ( color.a > 0 )
			{
				color.a -= delta;
				_visualEffect.color = color;

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
			SceneManager.LoadScene(0);
		}

		[ContextMenu("Add Prefs")]
		private void AddPrefs()
		{
			PlayerPrefs.SetString("Level1", "true");
			SceneManager.LoadScene(0);
		}
	}
}
