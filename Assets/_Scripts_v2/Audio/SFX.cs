using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SFX : MonoBehaviour {

	// Each object will have one Audio Source
	[SerializeField] protected AudioSource audioSource;

	public AudioSource GetAudioSource() {
		return this.audioSource;
	}
}
