using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using ProbCSharp;
using static ProbCSharp.ProbBase;

/// <summary>
/// Namespace for the bayesian network and the learner model.
/// </summary>
namespace BNUtils {

    /// <summary>
    /// Holds the nodes and types of operations for the learner model implemented as a monad.
    /// </summary>
	public class LearnerModel {
        /// <summary>
        /// Enum for what topic an unobserved node represents.
        /// </summary>
		public enum Node {
			Similar,
			Equivalent,
			Dissimilar
		}

        /// <summary>
        /// Enum for what operation an observed node for an unobserved node represents.
        /// </summary>
		public enum Operation {
			ADDITION,
			SUBTRACTION
		}

        /// <summary>
        /// Monad Constructor
        /// </summary>
        /// <param name="similar">Similar Node</param>
        /// <param name="equivalent">Equivalent Node</param>
        /// <param name="dissimilar">Dissimilar Node</param>
		public LearnerModel(bool similar, bool equivalent, bool dissimilar) {
			Similar = similar;
			Equivalent = equivalent;
			Dissimilar = dissimilar;
		}

		public bool Similar { get; }
		public bool Equivalent { get; }
		public bool Dissimilar { get; }
	}

    /// <summary>
    /// Hard coded bayesian network implemented through a probabilistic monads.
    /// </summary>
	public class BayesianNetwork {
        /// <summary>
        /// Rate of observed node for addition of similar fractions.
        /// </summary>
		public double fbSimilarAddition = 0.5;
        /// <summary>
        /// Rate of observed node for subtraction of similar fractions.
        /// </summary>
		public double fbSimilarSubtraction = 0.5;

        /// <summary>
        /// Observed node for addition of similar fractions as a finite distribution.
        /// </summary>
		private FiniteDist<bool> similarAdditionDist;
        /// <summary>
        /// Observed node for subtraction of similar fractions as a finite distribution.
        /// </summary>
		private FiniteDist<bool> similarSubtractionDist;

        /// <summary>
        /// Unobserved node for operations on similar fractions as a monad.
        /// </summary>
		private Func<bool, bool, Prob> similarProb;

        /// <summary>
        /// Rate of observed node for addition of equivalent fractions.
        /// </summary>
		public double fbEquivalentAddition = 0.5;
        /// <summary>
        /// Rate of observed node for addition of equivalent fractions.
        /// </summary>
		public double fbEquivalentSubtraction = 0.5;

        /// <summary>
        /// Observed node for addition of equivalent fractions as a finite distribution.
        /// </summary>
		private FiniteDist<bool> equivalentAdditionDist;
        /// <summary>
        /// Observed node for addition of equivalent fractions as a finite distribution.
        /// </summary>
		private FiniteDist<bool> equivalentSubtractionDist;

        /// <summary>
        /// Unobserved node for operations on equivalent fractions as a monad.
        /// </summary>
		private Func<bool, bool, bool, Prob> equivalentProb;

        /// <summary>
        /// Rate of observed node for addition of dissimilar fractions.
        /// </summary>
		public double fbDissimilarAddition = 0.5;
        /// <summary>
        /// Rate of observed node for addition of dissimilar fractions.
        /// </summary>
		public double fbDissimilarSubtraction = 0.5;

        /// <summary>
        /// Observed node for addition of dissimilar fractions as a finite distribution.
        /// </summary>
		private FiniteDist<bool> dissimilarAdditionDist;
        /// <summary>
        /// Observed node for addition of dissimilar fractions as a finite distribution.
        /// </summary>
		private FiniteDist<bool> dissimilarSubtractionDist;

        /// <summary>
        /// Unobserved node for operations on dissimilar fractions as a monad.
        /// </summary>
		private Func<bool, bool, bool, Prob> dissimilarProb;

        /// <summary>
        /// Learner model as a finite distribution of probabilities derived from the unobserved nodes.
        /// </summary>
		public FiniteDist<LearnerModel> learnerModel;

        /// <summary>
        /// Constructor
        /// </summary>
		public BayesianNetwork() {
			UpdateNodeProbabilities ();
			InitCPTs ();
			UpdateLearnerModel ();
		}

        /// <summary>
        /// Function call to initialize the conditional probability tables.
        /// </summary>
		private void InitCPTs() {
			similarProb = (addition, subtraction) => {
				if (addition && subtraction)
					return ProbBase.Prob (0.99);
				if (!addition && subtraction)
					return ProbBase.Prob (0.5);
				if (addition && !subtraction)
					return ProbBase.Prob (0.5);
				return ProbBase.Prob (0.01);
			};

			equivalentProb = (addition, subtraction, similar) => {
				if (addition && subtraction && similar)
					return ProbBase.Prob (0.99);
				if (!addition && subtraction && similar)
					return ProbBase.Prob (0.75);
				if (addition && !subtraction && similar)
					return ProbBase.Prob (0.75);
				if (addition && subtraction && !similar)
					return ProbBase.Prob (0.5);
				if (!addition && !subtraction && similar)
					return ProbBase.Prob (0.5);
				if (!addition && subtraction && !similar)
					return ProbBase.Prob (0.25);
				if (addition && !subtraction && !similar)
					return ProbBase.Prob (0.25);
				return ProbBase.Prob (0.01);
			};

			dissimilarProb = (addition, subtraction, equivalent) => {
				if (addition && subtraction && equivalent)
					return ProbBase.Prob (0.99);
				if (!addition && subtraction && equivalent)
					return ProbBase.Prob (0.75);
				if (addition && !subtraction && equivalent)
					return ProbBase.Prob (0.75);
				if (addition && subtraction && !equivalent)
					return ProbBase.Prob (0.5);
				if (!addition && !subtraction && equivalent)
					return ProbBase.Prob (0.5);
				if (!addition && subtraction && !equivalent)
					return ProbBase.Prob (0.25);
				if (addition && !subtraction && !equivalent)
					return ProbBase.Prob (0.25);
				return ProbBase.Prob (0.01);
			};
		}

        /// <summary>
        /// Function call to update the observed nodes for operations on similar fractions with its respective rates.
        /// </summary>
		public void UpdateSimilarProbability() {
			similarAdditionDist = ProbBase.BernoulliF (ProbBase.Prob (fbSimilarAddition));
			similarSubtractionDist = ProbBase.BernoulliF (ProbBase.Prob (fbSimilarSubtraction));
		}

        /// <summary>
        /// Function call to update the observed nodes for operations on equivalent fractions with its respective rates.
        /// </summary>
		public void UpdateEquivalentProbability() {
			equivalentAdditionDist = ProbBase.BernoulliF(ProbBase.Prob(fbEquivalentAddition));
			equivalentSubtractionDist = ProbBase.BernoulliF(ProbBase.Prob(fbEquivalentSubtraction));
		}

        /// <summary>
        /// Function call to update the observed nodes for operations on dissimilar fractions with its respective rates.
        /// </summary>
		public void UpdateDissimilarProbability() {
			dissimilarAdditionDist = ProbBase.BernoulliF (ProbBase.Prob (fbDissimilarAddition));
			dissimilarSubtractionDist = ProbBase.BernoulliF (ProbBase.Prob (fbDissimilarSubtraction));
		}
        
        /// <summary>
        /// Function call to update all observed nodes.
        /// </summary>
		public void UpdateNodeProbabilities() {
			UpdateSimilarProbability ();
			UpdateEquivalentProbability ();
			UpdateDissimilarProbability ();
		}

        /// <summary>
        /// Function call to update the learner model based on the current probabilities of the observed nodes.
        /// </summary>
		public void UpdateLearnerModel() {
			learnerModel =
				from similarAddition in similarAdditionDist
				from similarSubtraction in similarSubtractionDist
				from similar in ProbBase.BernoulliF (similarProb (similarAddition, similarSubtraction))
				from equivalentAddition in equivalentAdditionDist
				from equivalentSubtraction in equivalentSubtractionDist
				from equivalent in ProbBase.BernoulliF (equivalentProb (equivalentAddition, equivalentSubtraction, similar))
				from dissimilarAddition in dissimilarAdditionDist
				from dissimilarSubtraction in dissimilarSubtractionDist
				from dissimilar in ProbBase.BernoulliF (dissimilarProb (dissimilarAddition, dissimilarSubtraction, equivalent))
				select new LearnerModel (similar, equivalent, dissimilar);

			Infer (LearnerModel.Node.Similar);
			Infer (LearnerModel.Node.Equivalent);
			Infer (LearnerModel.Node.Dissimilar);
		}

        /// <summary>
        /// Infer the probability of an unobserved node.
        /// </summary>
        /// <param name="node">Main Topic</param>
        /// <returns>Node Probability</returns>
		public double Infer(LearnerModel.Node node) {
			double probability = 0;

			switch (node) {
			case LearnerModel.Node.Similar:
				probability = learnerModel.ProbOf (l => l.Similar).Value;
				Debug.Log ("Similar: " + probability);
				break;
			case LearnerModel.Node.Equivalent:
				probability = learnerModel.ProbOf (l => l.Equivalent).Value;
				Debug.Log ("Equivalent: " + probability);
				break;
			case LearnerModel.Node.Dissimilar:
				probability = learnerModel.ProbOf (l => l.Dissimilar).Value;
				Debug.Log ("Dissimilar: " + probability);
				break;
			}

			return probability;
		}
	}
}