using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class VisibleBonus : MonoBehaviour
{
	[SerializeField] private GameObject _visibleButton;

	private void Start()
	{
		PlayerPrefs.SetString( "Level1", "true" );

		if ( PlayerPrefs.GetString( "Level1" ) == "true" )
			_visibleButton.SetActive( true );
		else
			_visibleButton.SetActive( false );

		PlayerPrefs.DeleteAll();
	}
}
