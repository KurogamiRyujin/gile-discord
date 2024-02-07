using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SwitchMusicOnLoad : MonoBehaviour {

	public AudioClip newTrack;
	private AudioManager audioMngr;

	// Use this for initialization
	void Start () {
		audioMngr = FindObjectOfType<AudioManager> ();
		if (newTrack != null) {
			audioMngr.ChangeBGM (newTrack);
		}
	}
	
	// Update is called once per frame
	void Update () {
		
	}
}
