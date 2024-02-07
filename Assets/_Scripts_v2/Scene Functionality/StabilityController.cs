using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// Handles the stability number line's target value based on the instantiated ghost blocks in the room.
/// </summary>
public class StabilityController : MonoBehaviour {

    /// <summary>
    /// Reference to the stability number line of the room.
    /// </summary>
	[SerializeField] private StabilityNumberLine numberLine;
    /// <summary>
    /// Flag if the stability number line is arbitrary. False by default.
    /// 
    /// If it is true, whatever target value the stability number line had will be maintained.
    /// Otherwise, a new target value will be created based on the instantiated ghost blocks in the room.
    /// </summary>
	[SerializeField] private bool numberLineIsArbitrary = false;

    /// <summary>
    /// Updates the stability number line's target value given the instantiated ghost blocks in the room.
    /// 
    /// Target value is kept if the stability number line is set to arbitrary.
    /// </summary>
    /// <param name="blocks">All instantiated ghost blocks in the room.</param>
    /// <returns>Fraction Target Value</returns>
	public FractionData UpdateStabilityNumberLine(List<HollowBlock> blocks) {
		FractionData frac = new FractionData ();
		if (this.GetNumberLine () != null) {
			frac.numerator = numberLine.GetTargetNumerator ();
			frac.denominator = numberLine.GetTargetDenominator ();

			if (!this.numberLineIsArbitrary) {
				blocks.Shuffle<HollowBlock> ();
				int inclusionCount = Random.Range (1, blocks.Count+1);
				FractionData fractionSum = new FractionData ();
				fractionSum.numerator = 0;
				fractionSum.denominator = 1;
				float sum = 0f;

				if (blocks.Count == 1) {
					if (!blocks [0].IsSolved ()) {
						int denom = General.LCD(fractionSum.denominator, (int) blocks [0].GetDenominator ());
						FractionData temp = new FractionData ();
						temp.denominator = denom;
						temp.numerator = (denom / fractionSum.denominator) * fractionSum.numerator;

						Debug.Log ("Temp Frac 1: " + temp.numerator + "/" + temp.denominator);

						FractionData temp2 = new FractionData ();
						temp2.denominator = denom;
						temp2.numerator = (denom / (int) blocks [0].GetDenominator ()) * (int) blocks [0].GetNumerator ();

						Debug.Log ("Temp Frac 2: " + temp2.numerator + "/" + temp2.denominator);

						fractionSum.numerator = temp.numerator + temp2.numerator;
						fractionSum.denominator = denom;

						Debug.Log ("Fraction Sum: " + fractionSum.numerator + "/" + fractionSum.denominator);

//						sum += (blocks [0].GetNumerator () / blocks [0].GetDenominator ());
					}
				} else if (blocks.Count == 2) {
					int count = Random.Range (1, 3);

					if (count == 1 || blocks[0].IsSolved()) {
						int index = Random.Range (0, 2);

						if (PedagogicalComponent_v2.Instance.CurrentDenominator () != (int)blocks [index].GetDenominator () && !blocks [index].IsSolved ()) {
							index = Mathf.Abs (index - 1);
						} else if (blocks [index].IsSolved () && PedagogicalComponent_v2.Instance.CurrentDenominator () == (int)blocks [index].GetDenominator ())
							index = Mathf.Abs (index - 1);

						int denom = General.LCD (fractionSum.denominator, (int)blocks [index].GetDenominator ());
						FractionData temp = new FractionData ();
						temp.denominator = denom;
						temp.numerator = (denom / fractionSum.denominator) * fractionSum.numerator;

						Debug.Log ("Temp Frac 1: " + temp.numerator + "/" + temp.denominator);

						FractionData temp2 = new FractionData ();
						temp2.denominator = denom;
						temp2.numerator = (denom / (int)blocks [index].GetDenominator ()) * (int)blocks [index].GetNumerator ();

						Debug.Log ("Temp Frac 2: " + temp2.numerator + "/" + temp.denominator);

						fractionSum.numerator = temp.numerator + temp2.numerator;
						fractionSum.denominator = denom;

						Debug.Log ("Fraction Sum: " + fractionSum.numerator + "/" + fractionSum.denominator);
					} else {
						for (int i = 0; i < count; i++) {
							int denom = General.LCD(fractionSum.denominator, (int) blocks [i].GetDenominator ());
							FractionData temp = new FractionData ();
							temp.denominator = denom;
							temp.numerator = (denom / fractionSum.denominator) * fractionSum.numerator;

							Debug.Log ("Temp Frac 1: " + temp.numerator + "/" + temp.denominator);

							FractionData temp2 = new FractionData ();
							temp2.denominator = denom;
							temp2.numerator = (denom / (int) blocks [i].GetDenominator ()) * (int) blocks [i].GetNumerator ();

							Debug.Log ("Temp Frac 2: " + temp2.numerator + "/" + temp.denominator);

							fractionSum.numerator = temp.numerator + temp2.numerator;
							fractionSum.denominator = denom;

							Debug.Log ("Fraction Sum: " + fractionSum.numerator + "/" + fractionSum.denominator);
						}
					}

//					sum += blocks [index].GetNumerator () / blocks [index].GetDenominator ();
				} else if (blocks.Count > 2) {
					if (inclusionCount == blocks.Count)
						inclusionCount = blocks.Count - 1;

					int index1 = blocks.FindIndex (x => x.GetDenominator () == PedagogicalComponent_v2.Instance.CurrentDenominator ());
					HollowBlock blockTemp = blocks [0];
					blocks [0] = blocks [index1];
					blocks [index1] = blockTemp;

					for (int i = 0; i < inclusionCount; i++) {

						int denom = General.LCD(fractionSum.denominator, (int) blocks [i].GetDenominator ());
						FractionData temp = new FractionData ();
						temp.denominator = denom;
						temp.numerator = (denom / fractionSum.denominator) * fractionSum.numerator;

						Debug.Log ("Temp Frac 1: " + temp.numerator + "/" + temp.denominator);

						FractionData temp2 = new FractionData ();
						temp2.denominator = denom;
						temp2.numerator = (denom / (int) blocks [i].GetDenominator ()) * (int) blocks [i].GetNumerator ();

						Debug.Log ("Temp Frac 2: " + temp2.numerator + "/" + temp.denominator);

						fractionSum.numerator = temp.numerator + temp2.numerator;
						fractionSum.denominator = denom;

						Debug.Log ("Fraction Sum: " + fractionSum.numerator + "/" + fractionSum.denominator);

//						sum += (blocks [i].GetNumerator () / blocks [i].GetDenominator ());
					}
				} else {
					Debug.LogError ("No Blocks given.");
				}

				frac.numerator = fractionSum.numerator;
				frac.denominator = fractionSum.denominator;

				float[] simpFrac = new float[2];
				simpFrac = General.SimplifyFraction (frac.numerator, frac.denominator);

				frac.numerator = (int)simpFrac [0];
				frac.denominator = (int)simpFrac [1];

//				float[] fraction = General.SimplifyFraction (sum * 100f, 100f);
//				frac.numerator = (int)fraction [0];
//				frac.denominator = (int)fraction [1];

				float maxPoint = 0;
				foreach (HollowBlock block in blocks) {
                    Debug.LogError("STAB MAX CALC "+ block.GetNumerator() +" / "+ block.GetDenominator());
					maxPoint += (block.GetNumerator () / block.GetDenominator ());
				}

                //Debug.LogError("RAW MAX IS " + maxPoint);
                // TODO change last parameter (maxPoint) to the closest greater integer to the summation of all hollowblock values
                //numberLine.ChangeValue(frac.denominator, frac.numerator, frac.denominator, Mathf.CeilToInt(maxPoint));
                //numberLine.ChangeValueNoPrompt(frac.denominator, frac.numerator, frac.denominator, Mathf.CeilToInt(maxPoint));
                //Debug.LogError("STAB CONTROLLER GAVE Max pt of "+(Mathf.CeilToInt(maxPoint)));
                numberLine.ChangeValueNoPrompt(frac.denominator, frac.numerator, frac.denominator);
                //numberLine.ChangeValueNoPrompt(frac.denominator, frac.numerator, frac.denominator, Mathf.CeilToInt(maxPoint));
            }
        } else {
			frac.numerator = 0;
			frac.denominator = 1;
		}

		return frac;
	}

    /// <summary>
    /// Returns the stability number line.
    /// </summary>
    /// <returns>Stability Number Line</returns>
	public StabilityNumberLine GetNumberLine() {
		if (numberLine == null) {
			this.numberLine = FindObjectOfType<StabilityNumberLine> ();
		}
		return this.numberLine;
	}
}
