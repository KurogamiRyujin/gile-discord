using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Audible : MonoBehaviour {
	[SerializeField] protected AudioClip audioClip;

	public AudioClip GetAudioClip() {
		return this.audioClip;
	}
}
