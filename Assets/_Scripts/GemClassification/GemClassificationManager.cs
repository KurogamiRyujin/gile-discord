using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GemClassificationManager : MonoBehaviour {

	enum GemType {
		GEM_PROPER, GEM_IMPROPER, GEM_WHOLE
	}

	[SerializeField] private bool isCharged;
	[SerializeField] private GemType type;
	[SerializeField] private HammerObject hammer;
	[SerializeField] private ParticleSystem particleSystem;

	void Start () {
		CheckNull ();

		this.isCharged = false;
		this.particleSystem.Stop ();
	}

	void CheckNull() {
		if(this.particleSystem == null) {
			this.particleSystem = gameObject.GetComponentInChildren<ParticleSystem> ();
		}
	}


	void Update () {
		
	}


	void Charge(int numerator, int denominator) {
		switch (type) {
			case GemType.GEM_PROPER:
				if (numerator != 0 && numerator < denominator)
					this.isCharged = true;
				break;
			case GemType.GEM_IMPROPER:
				if (numerator >= denominator)
					this.isCharged = true;
				break;
			case GemType.GEM_WHOLE:
				if (numerator == denominator)
					this.isCharged = true;
				break;
		}

		if (this.isCharged) {
			Debug.Log ("SUCCESSFULLY CHARGED GEM "+type.ToString());
			this.particleSystem.Play ();
		}
	}

	void OnTriggerEnter2D(Collider2D other) {

		if (!this.isCharged) {
			if (other.CompareTag ("Hammer Child")) {
				Debug.Log ("Hammer Enter");

				HammerChildCollisionDetection hammerChild = other.GetComponent<HammerChildCollisionDetection> ();
				this.hammer = hammerChild.GetParentHammerObject ();
				Charge (hammer.GetNumerator (), hammer.GetDenominator ());
			}
		}
	}
}
