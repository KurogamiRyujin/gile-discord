using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using BNUtils;
using System;
using ProbCSharp;
using static ProbCSharp.ProbBase;

public class BayesTesting : MonoBehaviour {

	public double fbSimilarAddition = 0.5;
	public double fbSimilarSubtraction = 0.5;

	public double fbEquivalentAddition = 0.5;
	public double fbEquivalentSubtraction = 0.5;

	public double fbDissimilarAddition = 0.5;
	public double fbDissimilarSubtraction = 0.5;

	private BayesianNetwork bn;

	// Use this for initialization
	void Start () {
		bn = new BayesianNetwork ();
	}
	
	// Update is called once per frame
	void Update () {
		if (Input.GetKeyDown (KeyCode.Space))
			UpdateProbs ();

		if (Input.GetKeyDown (KeyCode.S))
			this.bn.Infer (LearnerModel.Node.Similar);

		if (Input.GetKeyDown (KeyCode.E))
			this.bn.Infer (LearnerModel.Node.Equivalent);

		if (Input.GetKeyDown (KeyCode.D))
			this.bn.Infer (LearnerModel.Node.Dissimilar);
	}

	private void UpdateProbs() {
		this.bn.fbSimilarAddition = this.fbSimilarAddition;
		this.bn.fbSimilarSubtraction = this.fbSimilarSubtraction;

		this.bn.UpdateSimilarProbability ();

		this.bn.fbEquivalentAddition = this.fbEquivalentSubtraction;
		this.bn.fbEquivalentSubtraction = this.fbEquivalentSubtraction;

		this.bn.UpdateEquivalentProbability ();

		this.bn.fbDissimilarAddition = this.fbDissimilarAddition;
		this.bn.fbDissimilarSubtraction = this.fbDissimilarSubtraction;

		this.bn.UpdateDissimilarProbability ();

		this.bn.UpdateLearnerModel ();
	}
}
