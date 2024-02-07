using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AttemptedAnswerv2 {
	private List<FractionData> fractions;
	private List<int> numerators, denominators;
	private SceneTopic topic;

	public AttemptedAnswerv2(SceneTopic topic) {
		this.fractions = new List<FractionData> ();
		this.numerators = new List<int> ();
		this.denominators = new List<int> ();
		this.topic = topic;
	}

	public void AddFraction(FractionData fraction) {
		this.fractions.Add (fraction);
		this.numerators.Add (fraction.numerator);
		this.denominators.Add (fraction.denominator);
	}

	public FractionData[] GetFractions() {
		return this.fractions.ToArray ();
	}

	public int[] GetUsedNumerators() {
		return this.numerators.ToArray ();
	}

	public int[] GetUsedDenominators() {
		return this.denominators.ToArray ();
	}

	public SceneTopic GetTopic() {
		return topic;
	}
}
