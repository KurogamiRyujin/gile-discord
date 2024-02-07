using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AudioManager : MonoBehaviour {

	public AudioSource BGM;

	// Use this for initialization
	void Start () {
		DontDestroyOnLoad (gameObject);

		if (FindObjectsOfType<AudioManager> ().Length > 1)
			Destroy (gameObject);
	}
	
	// Update is called once per frame
	void Update () {
		
	}

	public void ChangeBGM(AudioClip music) {
		if (BGM.clip.name == music.name)
			return;
		BGM.Stop ();
		BGM.clip = music;
		BGM.loop = true;
		BGM.Play ();
	}

	public void MuteBGM() {
		BGM.mute = true;
	}
}
