using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class AbstractPedagogicalComponent : MonoBehaviour {

	[SerializeField] protected List<int> validDenominators;
	protected List<int> validNumerators;

	protected abstract void UpdateValidDenominators ();

	public List<int> GetValidDenominators() {
		return this.validDenominators;
	}

	public List<int> GetValidNumerators() {
		return this.validNumerators;
	}
}
