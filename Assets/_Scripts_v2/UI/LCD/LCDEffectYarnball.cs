using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LCDEffectYarnball : MonoBehaviour {
	[SerializeField] private ParticleEffect particleEffectDecrease;
	void Start () {
		this.particleEffectDecrease = GetComponent<ParticleEffect> ();
	}

	public ParticleEffect GetParticleEffectDecrease() {
		return this.particleEffectDecrease;
	}
}
