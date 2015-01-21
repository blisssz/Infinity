using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class PlayList : MonoBehaviour {

	private AudioSource[] allAudioSources;

	// Use this for initialization
	void Start () {
		AudioSource[] allAudioSources =
			UnityEngine.Object.FindObjectsOfType<AudioSource>();
	}
	
	// Update is called once per frame
	void Update () {
		AudioSource[] allAudioSources =
			UnityEngine.Object.FindObjectsOfType<AudioSource>();
	}
}
