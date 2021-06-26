using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;
using UnityEngine.SceneManagement;
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
		[SerializeField] private GameObject _optionView;

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
			SceneParameters.LoadGameContext = _bonusContext;
			SceneManager.LoadScene(1);
		}

		private void CountiuneGameCommandHandler()
		{
			SceneManager.LoadScene(1);
		}

		private void NewGameCommandHandler()
		{
			SceneParameters.LoadGameContext = _header1Context;
			SceneManager.LoadScene(1);
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
