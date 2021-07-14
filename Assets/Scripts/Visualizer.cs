using System.Collections;
using System.Linq;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace VisualNovel.UI
{
	public class Visualizer : MonoBehaviour
	{
		[SerializeField] private AudioSource _audioSource;
		[SerializeField] private bool _isVisualizer;
		[SerializeField] private FFTWindow _type;
		[SerializeField] private float[] _samples = new float[256];

		[SerializeField] private List<Image> _images = new List<Image>();

		private void Update()
		{
			if(_isVisualizer)
			{
				_audioSource.GetSpectrumData( _samples, 0, _type );

				for ( int i = 0; i < _samples.Length; i++ )
					_images[i].fillAmount = _samples[i];
			}
		}
	}
}
