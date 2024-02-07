using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LCDEffectHammer : MonoBehaviour {

	[SerializeField] private ParticleEffect particleEffectIncrease;
	void Start () {
		this.particleEffectIncrease = GetComponent<ParticleEffect> ();
	}

	public ParticleEffect GetParticleEffectIncrease() {
		return this.particleEffectIncrease;
	}
}
