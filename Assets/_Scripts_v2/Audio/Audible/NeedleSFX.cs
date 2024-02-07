using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NeedleSFX : SFX {

	[SerializeField] private NeedleAudible[] audibles;
	private Dictionary<AudibleNames.Needle, AudioClip> audioPairs;

	void Start () {
		this.audioSource = gameObject.AddComponent<AudioSource> ();
		this.audibles = GetComponents<NeedleAudible>();
		audioSource.playOnAwake = false;
		GenerateAudioPairs ();
	}

	public AudioSource GetAudioSource(AudibleNames.Needle name) {
		this.audioSource.clip = this.audioPairs [name];
		return this.audioSource;
	}

	void GenerateAudioPairs() {
		this.audioPairs = new Dictionary<AudibleNames.Needle, AudioClip>();
		foreach (NeedleAudible audible in audibles) {
			audioPairs.Add (audible.GetID(), audible.GetAudioClip());
		}
	}
}
