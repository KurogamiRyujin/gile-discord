using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CheckAudioManager : MonoBehaviour {

	public GameObject audioMngr;

	// Use this for initialization
	void Start () {
		if (FindObjectOfType<AudioManager> ())
			return;
		else
			Instantiate (audioMngr, transform.position, transform.rotation);
	}
	
	// Update is called once per frame
	void Update () {
		
	}
}
