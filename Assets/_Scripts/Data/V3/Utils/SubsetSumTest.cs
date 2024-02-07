using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using SubsetSumUtils;

public class SubsetSumTest : MonoBehaviour {

	public List<FractionData> fractions;

	// Use this for initialization
	void Start () {
		FractionSet set = new FractionSet ();
		set.fractions.AddRange (fractions);
		int index = 0;

		FractionData target = new FractionData ();
		target.numerator = 1;
		target.denominator = 2;

		foreach (FractionSet s in SubsetSumFractions.SubsetSum(set, target)) {
			string log = "Set " + index + ": ";
			foreach (FractionData fraction in s.fractions) {
				log += fraction.numerator.ToString () + "/" + fraction.denominator.ToString () + ", ";
			}

			Debug.Log (log);
			index++;
		}
	}
}
