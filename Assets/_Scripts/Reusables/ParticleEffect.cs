using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ParticleEffect : MonoBehaviour {
	public enum EffectType {
		ACTIVATE,
		LAMP_LIGHTS,
		YUNI_DAMAGE
	}

	[SerializeField] private EffectType NAME;
	[SerializeField] private ParticleSystem[] particleEffects;

	void Start () {
		NullCheck ();
	}

	void NullCheck() {
		if (particleEffects == null || particleEffects.Length == 0) {
			this.particleEffects = gameObject.GetComponentsInChildren<ParticleSystem> ();
		}
	}


	public void Play() {
//		Debug.Log ("ParticleEffect.cs PLAY " + NAME.ToString ());
		foreach (ParticleSystem particleSystem in this.particleEffects) {
//			Debug.Log ("ps play");
			particleSystem.Play ();
		}
	}

	public void ChangeParticleTransform(Vector3 newPosition) {
		foreach (ParticleSystem effect in particleEffects) {
			effect.transform.position = new Vector3 (newPosition.x, newPosition.y, newPosition.z);
		}
	}

	public void ChangeParticleRotation(Quaternion newRotation) {
		foreach (ParticleSystem effect in particleEffects) {
			effect.transform.rotation = Quaternion.Euler (newRotation.x, newRotation.y, newRotation.z);
		}
	}

	public void Stop() {
		foreach (ParticleSystem particleSystem in this.particleEffects) {
			particleSystem.Stop ();
		}
	}

	public EffectType GetEffectType() {
		return this.NAME;
	
	}
}
