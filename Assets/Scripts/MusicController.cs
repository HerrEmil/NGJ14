using UnityEngine;
using System.Collections;

public class MusicController : MonoBehaviour {

	public AudioClip winAudio;
	public AudioClip gameAudio;

	public void PlayGameAudio()
	{
		GetComponent<AudioSource>().clip = gameAudio;
	}

	public void PlayWinAudio()
	{
		GetComponent<AudioSource>().clip = winAudio;
		GetComponent<AudioSource>().Play();
	}
}
