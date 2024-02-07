using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ButtonSFX : SFX {

	[SerializeField] private ButtonAudible[] audibles;
	private Dictionary<AudibleNames.Button, AudioClip> audioPairs;

	void Start () {
		this.audioSource = gameObject.AddComponent<AudioSource> ();
		this.audibles = GetComponents<ButtonAudible>();
		audioSource.playOnAwake = false;
		GenerateAudioPairs ();
	}

	public AudioSource GetAudioSource(AudibleNames.Button name) {
		this.audioSource.clip = this.audioPairs [name];
		return this.audioSource;
	}

	void GenerateAudioPairs() {
		this.audioPairs = new Dictionary<AudibleNames.Button, AudioClip>();
		foreach (ButtonAudible audible in audibles) {
			audioPairs.Add (audible.GetID(), audible.GetAudioClip());
		}
	}


}
