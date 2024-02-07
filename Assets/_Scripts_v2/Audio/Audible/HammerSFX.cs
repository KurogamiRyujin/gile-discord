using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HammerSFX : SFX {

	[SerializeField] private HammerAudible[] audibles;
	private Dictionary<AudibleNames.Hammer, AudioClip> audioPairs;

	void Start () {
		this.audioSource = gameObject.AddComponent<AudioSource> ();
		this.audibles = GetComponents<HammerAudible>();
		audioSource.playOnAwake = false;
		GenerateAudioPairs ();
	}

	public AudioSource GetAudioSource(AudibleNames.Hammer name) {
		this.audioSource.clip = this.audioPairs [name];
		Debug.Log (this.audioPairs [name].name);
		return this.audioSource;
	}

	void GenerateAudioPairs() {
		this.audioPairs = new Dictionary<AudibleNames.Hammer, AudioClip>();
		foreach (HammerAudible audible in audibles) {
			audioPairs.Add (audible.GetID(), audible.GetAudioClip());
			Debug.Log ("<color=green>added "+audible.GetID()+"</color>");
		}
	}


}
