using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LCDEffectEnemy : MonoBehaviour {

	[SerializeField] private ParticleEffect particleEffectDecrease;
	void Start () {
		this.particleEffectDecrease = GetComponent<ParticleEffect> ();
	}

	public ParticleEffect GetParticleEffectDecrease() {
		return this.particleEffectDecrease;
	}
}
