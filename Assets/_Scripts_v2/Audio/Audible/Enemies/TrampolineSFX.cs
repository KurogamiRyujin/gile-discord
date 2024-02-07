using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TrampolineSFX : SFX {

	[SerializeField] private TrampolineAudible[] audibles;
	private Dictionary<AudibleNames.Trampoline, AudioClip> audioPairs;

	void Start () {
		this.audioSource = gameObject.AddComponent<AudioSource> ();
		this.audibles = GetComponents<TrampolineAudible>();
		audioSource.playOnAwake = false;
		GenerateAudioPairs ();

	}

	public AudioSource GetAudioSource(AudibleNames.Trampoline name) {
		this.audioSource.clip = this.audioPairs [name];
		return this.audioSource;
	}

	void GenerateAudioPairs() {
		this.audioPairs = new Dictionary<AudibleNames.Trampoline, AudioClip>();
		foreach (TrampolineAudible audible in audibles) {
			audioPairs.Add (audible.GetID(), audible.GetAudioClip());
		}
	}


}
