using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DoorSFX : SFX {

	[SerializeField] private DoorAudible[] audibles;
	private Dictionary<AudibleNames.Door, AudioClip> audioPairs;

	void Start () {
		this.audioSource = gameObject.AddComponent<AudioSource> ();
		this.audibles = GetComponents<DoorAudible>();
		audioSource.playOnAwake = false;
		GenerateAudioPairs ();
	}

	public AudioSource GetAudioSource() {
		if (this.audioSource == null) {
			this.audioSource = gameObject.AddComponent<AudioSource> ();
		}
		return this.audioSource;
	}

	public AudioClip GetAudioPairs(AudibleNames.Door name) {
		if (this.audioPairs == null) {
			this.GenerateAudioPairs ();
		}
		if (this.audioPairs.ContainsKey (name)) {
			return this.audioPairs [name];
		}
		else {
			return null;
		}
	}

	public AudioSource GetAudioSource(AudibleNames.Door name) {
		this.GetAudioSource().clip = this.GetAudioPairs (name);
		return this.audioSource;
	}

	void GenerateAudioPairs() {
		this.audioPairs = new Dictionary<AudibleNames.Door, AudioClip>();
		foreach (DoorAudible audible in audibles) {
			audioPairs.Add (audible.GetID(), audible.GetAudioClip());
		}
	}


}
