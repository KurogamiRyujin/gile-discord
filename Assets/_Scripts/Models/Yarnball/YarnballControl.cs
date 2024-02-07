using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// Manages the charm pedestals in the room. Charm values are distributed by this behaviour.
/// </summary>
public class YarnballControl : MonoBehaviour {

    /// <summary>
    /// Modes which the charm controller gives values.
    /// 
    /// Dynamic bases its values from the pedagogical component.
    /// Arbitrary bases its values from a list of values given by the developers.
    /// </summary>
	public enum GivenDenominators
	{
		DYNAMIC,
		ARBITRARY
	}
    /// <summary>
    /// Mode for how the controller sets the charm values.
    /// </summary>
	[SerializeField] private GivenDenominators givenDenominators = GivenDenominators.ARBITRARY;
    /// <summary>
    /// List of all charm pedestals in the room.
    /// </summary>
	[SerializeField] private List<YarnballPedestal> yarnballPedestals = new List<YarnballPedestal> ();

    /// <summary>
    /// List of arbitrary values used if the mode is in Arbitrary.
    /// </summary>
	[SerializeField] private List<int> arbitraryValues;

	void OnEnable() {
		if (this.givenDenominators == GivenDenominators.ARBITRARY)
			InitiateYarnballPedestal ();
	}

	void OnDisable() {
		DeactivateYarnballPedestals ();
	}

    /// <summary>
    /// Initializes the charm pedestals to spawn charms. Charm values depend on mode.
    /// </summary>
	public void InitiateYarnballPedestal() {
		DeactivateYarnballPedestals ();
		List<int> denominators = new List<int> ();

		switch (this.givenDenominators) {
		case GivenDenominators.DYNAMIC:
			foreach (YarnballPedestal pedestal in this.yarnballPedestals) {
				int denominator = PedagogicalComponent_v2.Instance.RequestDenominator ();

				if (denominator != 0)
					pedestal.Open (denominator);
			}
			break;
		case GivenDenominators.ARBITRARY:
			int j = 0;
			foreach (YarnballPedestal yarnballPedestal in yarnballPedestals) {
				yarnballPedestal.Open (this.arbitraryValues[j]);
				j++;
			}
			break;
		}

	}

    /// <summary>
    /// Close all charm pedestals, destroying the charms they hold.
    /// </summary>
	public void DeactivateYarnballPedestals() {
		foreach (YarnballPedestal yarnballPedestal in yarnballPedestals) {
			yarnballPedestal.Close ();
		}
	}

    /// <summary>
    /// Returns the mode the controller is set in.
    /// </summary>
    /// <returns>Controller Mode</returns>
	public GivenDenominators GivenDenoms() {
		return this.givenDenominators;
	}
}
