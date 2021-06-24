using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;
using UnityEngine.SceneManagement;

public class MainView : MonoBehaviour, IView
{
	[SerializeField] private GameObject _optionView;

	public void CommandHandler( string commandName )
	{
		switch ( commandName )
		{
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

	private void CountiuneGameCommandHandler()
	{
		SceneManager.LoadSceneAsync( 1 );
	}

	private void NewGameCommandHandler()
	{
		SceneManager.LoadSceneAsync( 1 );
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
}
