using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class YuniSFX : SFX {

	[SerializeField] private YuniAudible[] audibles;
	private Dictionary<AudibleNames.Yuni, AudioClip> audioPairs;

	void Start () {
		this.audioSource = gameObject.AddComponent<AudioSource> ();
		this.audibles = GetComponents<YuniAudible>();
		audioSource.playOnAwake = false;
		GenerateAudioPairs ();
	}

	public AudioSource GetAudioSource(AudibleNames.Yuni name) {
		this.audioSource.clip = this.audioPairs [name];
		return this.audioSource;
	}

	void GenerateAudioPairs() {
		this.audioPairs = new Dictionary<AudibleNames.Yuni, AudioClip>();
		foreach (YuniAudible audible in audibles) {
			audioPairs.Add (audible.GetID(), audible.GetAudioClip());
		}
	}


}
