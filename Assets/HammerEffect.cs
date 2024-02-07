using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HammerEffect : MonoBehaviour {

	[SerializeField] private ParticleSystem effectHammerSmash;

	// Use this for initialization
	void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
		
	}

	public void PlayHammerEffect(float value) {
		this.effectHammerSmash.Play ();
	}

	public void SetHammerEffect(ParticleSystem effect) {
		this.effectHammerSmash = effect;
	}
}
