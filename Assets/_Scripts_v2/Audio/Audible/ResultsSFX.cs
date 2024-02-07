using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ResultsSFX : SFX {

	[SerializeField] private ResultsAudible[] audibles;
	private Dictionary<AudibleNames.Results, AudioClip> audioPairs;

	void Start () {
		this.audioSource = gameObject.AddComponent<AudioSource> ();
		this.audibles = GetComponents<ResultsAudible>();
		audioSource.playOnAwake = false;
		GenerateAudioPairs ();
	}

	public AudioSource GetAudioSource(AudibleNames.Results name) {
		this.audioSource.clip = this.audioPairs [name];
		return this.audioSource;
	}

	void GenerateAudioPairs() {
		this.audioPairs = new Dictionary<AudibleNames.Results, AudioClip>();
		foreach (ResultsAudible audible in audibles) {
			audioPairs.Add (audible.GetID(), audible.GetAudioClip());
		}
	}


}
