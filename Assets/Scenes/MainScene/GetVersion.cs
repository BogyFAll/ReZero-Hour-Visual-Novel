using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class GetVersion : MonoBehaviour
{
	private void Start()
	{
		GetComponent<TextMeshProUGUI>().text = string.Format( "v.{0}", Application.version );
	}
}
