using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;

namespace SubsetSumUtils {
	public class FractionSet {
		public List<FractionData> fractions;

		public FractionSet() {
			this.fractions = new List<FractionData> ();
		}

		public float Sum() {
			float total = 0f;
			FractionData fractionSum = new FractionData ();
			fractionSum.numerator = 0;
			fractionSum.denominator = 1;

			foreach (FractionData fraction in fractions) {
				int denom = General.LCD(fractionSum.denominator, (int) fraction.denominator);
				FractionData temp = new FractionData ();
				temp.denominator = denom;
				temp.numerator = (denom / fractionSum.denominator) * fractionSum.numerator;

				Debug.Log ("Temp Frac 1: " + temp.numerator + "/" + temp.denominator);

				FractionData temp2 = new FractionData ();
				temp2.denominator = denom;
				temp2.numerator = (denom / fraction.denominator) * fraction.numerator;

				Debug.Log ("Temp Frac 2: " + temp2.numerator + "/" + temp2.denominator);

				fractionSum.numerator = temp.numerator + temp2.numerator;
				fractionSum.denominator = denom;

//				total += (fraction.numerator / (float)fraction.denominator);
			}

			total = fractionSum.numerator / (float)fractionSum.denominator;

			Debug.Log ("Fraction Sum: " + fractionSum.numerator + "/" + fractionSum.denominator);
			Debug.Log ("Total: " + total);

			return total;
		}

		public FractionData FractionSum() {
			FractionData fractionSum = new FractionData ();
			fractionSum.numerator = 0;
			fractionSum.denominator = 1;

			foreach (FractionData fraction in fractions) {
				int denom = General.LCD(fractionSum.denominator, (int) fraction.denominator);
				FractionData temp = new FractionData ();
				temp.denominator = denom;
				temp.numerator = (denom / fractionSum.denominator) * fractionSum.numerator;

				FractionData temp2 = new FractionData ();
				temp2.denominator = denom;
				temp2.numerator = (denom / fraction.denominator) * fraction.numerator;

				fractionSum.numerator = temp.numerator + temp2.numerator;
				fractionSum.denominator = denom;
			}

			return fractionSum;
		}

		//<summary>
		//Algorithm by SergeyS from stackoverflow
		//</summary>
		public IEnumerable<FractionSet> EnumeratePowerSet() {
			int n = fractions.Count;
			int powerSetCount = 1 << n;
			List<FractionSet> sets = new List<FractionSet> ();

			for (int setMask = 0; setMask < powerSetCount; setMask++) {
				FractionSet set = new FractionSet ();
				for (int i = 0; i < n; i++) {
					if ((setMask & (1 << i)) > 0) {
						set.fractions.Add (fractions [i]);
					}
				}
				sets.Add (set);
			}

			return sets;
		}
	}

	public class SubsetSumFractions {

		//<summary>
		//Code snippet by Eric Lippert from stackexchange
		//</summary>
		public static IEnumerable<FractionSet> SubsetSum(FractionSet fractions, FractionData target) {
			return from subset in fractions.EnumeratePowerSet ()
			       where General.SimplifyFraction (subset.FractionSum ().numerator, subset.FractionSum ().denominator) [0] == General.SimplifyFraction (target.numerator, target.denominator) [0]
			           && General.SimplifyFraction (subset.FractionSum ().numerator, subset.FractionSum ().denominator) [1] == General.SimplifyFraction (target.numerator, target.denominator) [1]
			       select subset;
//			return from subset in fractions.EnumeratePowerSet ()
//			       where subset.FractionSum ().numerator == target.numerator && subset.FractionSum ().denominator == target.denominator
//			       select subset;

//			return from subset in fractions.EnumeratePowerSet ()
//			       where subset.Sum () == (target.numerator / (float)target.denominator)
//			       select subset;
		}
	}
}
