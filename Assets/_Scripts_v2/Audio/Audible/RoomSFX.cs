using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RoomSFX : SFX {

	[SerializeField] private RoomAudible[] audibles;
	private Dictionary<AudibleNames.Room, AudioClip> audioPairs;

	void Start () {
		this.audioSource = gameObject.AddComponent<AudioSource> ();
		this.audibles = GetComponents<RoomAudible>();
		audioSource.playOnAwake = false;
		GenerateAudioPairs ();
	}

	public AudioSource GetAudioSource(AudibleNames.Room name) {
		this.audioSource.clip = this.audioPairs [name];
		return this.audioSource;
	}

	void GenerateAudioPairs() {
		this.audioPairs = new Dictionary<AudibleNames.Room, AudioClip>();
		foreach (RoomAudible audible in audibles) {
			audioPairs.Add (audible.GetID(), audible.GetAudioClip());
		}
	}


}
