using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DummyPedagogicalComponent : AbstractPedagogicalComponent {

	[SerializeField] private bool updateDenominators = false;

	// Use this for initialization
	void Start () {
		Random.InitState (123156);
		UpdateValidDenominators ();
		updateDenominators = false;
	}

	void Update() {
		if (updateDenominators)
			UpdateValidDenominators ();
	}

	public override void UpdateValidDenominators() {
		updateDenominators = false;
		validDenominators = new List<int> ();

//		for (int i = 0; i < Random.Range (3, 30); i++) {
//			validDenominators.Add (Random.Range (3, 10));
//		}
	}

	public void GenerateUnitFractions() {
		validNumerators = new List<int> ();
		validNumerators.Add (1);
	}

	public void GetUnitFractions() {

	}

	public void GetProperFractions() {

	}

	public void GetImproperFractions() {

	}
}
