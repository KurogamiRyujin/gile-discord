using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LCDInterfaceSFX : SFX {

	[SerializeField] private LCDInterfaceAudible[] audibles;
	private Dictionary<AudibleNames.LCDInterface, AudioClip> audioPairs;

	void Start () {
		this.audioSource = gameObject.AddComponent<AudioSource> ();
		this.audibles = GetComponents<LCDInterfaceAudible>();
		audioSource.playOnAwake = false;
		GenerateAudioPairs ();
	}

	public AudioSource GetAudioSource(AudibleNames.LCDInterface name) {
		this.audioSource.clip = this.audioPairs [name];
		return this.audioSource;
	}

	void GenerateAudioPairs() {
		this.audioPairs = new Dictionary<AudibleNames.LCDInterface, AudioClip>();
		foreach (LCDInterfaceAudible audible in audibles) {
			audioPairs.Add (audible.GetID(), audible.GetAudioClip());
		}
	}
}
