using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StabilityController : MonoBehaviour {

	[SerializeField] private StabilityNumberLine numberLine;
	[SerializeField] private bool numberLineIsArbitrary = false;

	public FractionData UpdateStabilityNumberLine(List<HollowBlock> blocks) {
		FractionData frac = new FractionData ();
		if (this.GetNumberLine () != null) {
			frac.numerator = numberLine.GetTargetNumerator ();
			frac.denominator = numberLine.GetTargetDenominator ();

			if (!this.numberLineIsArbitrary) {
				blocks.Shuffle<HollowBlock> ();

				int inclusionCount = Random.Range (1, blocks.Count+1);
				float sum = 0f;

				if (blocks.Count == 1) {
					if (!blocks [0].IsSolved ()) {
						sum += (blocks [0].GetNumerator () / blocks [0].GetDenominator ());
					}
				} else if (blocks.Count > 1) {
					for (int i = 0; i < inclusionCount; i++)
						sum += (blocks [i].GetNumerator () / blocks [i].GetDenominator ());
				} else {
					Debug.LogError ("No Blocks given.");
				}

				float[] fraction = General.SimplifyFraction (sum * 100f, 100f);
				frac.numerator = (int)fraction [0];
				frac.denominator = (int)fraction [1];

				numberLine.ChangeValue (frac.denominator, frac.numerator, frac.denominator);
			}
		} else {
			frac.numerator = 0;
			frac.denominator = 1;
		}

		return frac;
	}

	public StabilityNumberLine GetNumberLine() {
		if (numberLine == null) {
			this.numberLine = FindObjectOfType<StabilityNumberLine> ();
		}
		return this.numberLine;
	}
}
