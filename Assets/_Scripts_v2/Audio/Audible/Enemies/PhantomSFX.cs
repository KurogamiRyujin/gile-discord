using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PhantomSFX : SFX {

	[SerializeField] private PhantomAudible[] audibles;
	private Dictionary<AudibleNames.Phantom, AudioClip> audioPairs;

	void Start () {
		this.audioSource = gameObject.AddComponent<AudioSource> ();
		this.audibles = GetComponents<PhantomAudible>();
		audioSource.playOnAwake = false;
		GenerateAudioPairs ();
	}

	public AudioSource GetAudioSource(AudibleNames.Phantom name) {
		this.audioSource.clip = this.audioPairs [name];
		return this.audioSource;
	}

	void GenerateAudioPairs() {
		this.audioPairs = new Dictionary<AudibleNames.Phantom, AudioClip>();
		foreach (PhantomAudible audible in audibles) {
			audioPairs.Add (audible.GetID(), audible.GetAudioClip());
		}
	}


}
