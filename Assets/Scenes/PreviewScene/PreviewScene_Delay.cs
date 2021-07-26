using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace PreviewScene
{
	public class PreviewScene_Delay	: MonoBehaviour
	{
		[Range(1f, 10f)] public float DelayInSecond;

		private void Start()
		{
			StartCoroutine(StartDelay());
		}

		private IEnumerator StartDelay()
		{
			yield return new WaitForSecondsRealtime(DelayInSecond);

			SceneManager.LoadScene(1);

			yield return null;
		}
	}
}

