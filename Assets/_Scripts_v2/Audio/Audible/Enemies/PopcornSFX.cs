using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PopcornSFX : SFX {

	[SerializeField] private PopcornAudible[] audibles;
	private Dictionary<AudibleNames.Popcorn, AudioClip> audioPairs;

	void Start () {
		this.audioSource = gameObject.AddComponent<AudioSource> ();
		this.audibles = GetComponents<PopcornAudible>();
		audioSource.playOnAwake = false;
		GenerateAudioPairs ();

	}

	public AudioSource GetAudioSource(AudibleNames.Popcorn name) {
		this.audioSource.clip = this.audioPairs [name];
		return this.audioSource;
	}

	void GenerateAudioPairs() {
		this.audioPairs = new Dictionary<AudibleNames.Popcorn, AudioClip>();
		foreach (PopcornAudible audible in audibles) {
			audioPairs.Add (audible.GetID(), audible.GetAudioClip());
		}
	}


}
