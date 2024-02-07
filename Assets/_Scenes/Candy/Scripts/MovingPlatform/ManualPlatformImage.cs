using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ManualPlatformImage : MonoBehaviour {
	[SerializeField] private ManualLeverAnimatable manualLeverAnimatable;

	void Awake() {
		this.manualLeverAnimatable = GetComponent<ManualLeverAnimatable> ();
	}

	public ManualLeverAnimatable GetAnimatable() {
		if (this.manualLeverAnimatable == null) {
			this.manualLeverAnimatable = GetComponent<ManualLeverAnimatable> ();
		}
		return this.manualLeverAnimatable;
	}

	public void ToDefault() {
		this.GetAnimatable ().Open ();
	}

	public void ToTarget() {
		this.GetAnimatable ().Close ();
	}
}
