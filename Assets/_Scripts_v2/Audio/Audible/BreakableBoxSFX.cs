using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BreakableBoxSFX : SFX {

	[SerializeField] private BreakableBoxAudible[] audibles;
	private Dictionary<AudibleNames.BreakableBox, AudioClip> audioPairs;

	void Start () {
		this.audioSource = gameObject.AddComponent<AudioSource> ();
		this.audibles = GetComponents<BreakableBoxAudible>();
		audioSource.playOnAwake = false;
		GenerateAudioPairs ();
	}

	public AudioSource GetAudioSource(AudibleNames.BreakableBox name) {
		this.audioSource.clip = this.audioPairs [name];
		return this.audioSource;
	}

	void GenerateAudioPairs() {
		this.audioPairs = new Dictionary<AudibleNames.BreakableBox, AudioClip>();
		foreach (BreakableBoxAudible audible in audibles) {
			audioPairs.Add (audible.GetID(), audible.GetAudioClip());
		}
	}


}
