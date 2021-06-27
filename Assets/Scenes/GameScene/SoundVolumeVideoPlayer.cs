using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Video;

public class SoundVolumeVideoPlayer	: MonoBehaviour
{
	[SerializeField] private AudioSource _audioSource;
	[SerializeField] private VideoPlayer _videoPlayer;

	private void Start()
	{
		_videoPlayer.EnableAudioTrack(0, true);
		_videoPlayer.SetTargetAudioSource(0, _audioSource);

		_videoPlayer.Play();
		_audioSource.Play();
	}
}
