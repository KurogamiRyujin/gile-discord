using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using BNUtils;
using System;
using System.IO;
using ProbCSharp;
using static ProbCSharp.ProbBase;
using UnityEngine.SceneManagement;

/// <summary>
/// Component handling the generation and updating of the learner model.
/// </summary>
public class LearnerModelingComponent {
    /// <summary>
    /// Hollow Block string for consistent usage of the string.
    /// </summary>
	private const string HOLLOW_BLOCK = "Hollow Block";

    /// <summary>
    /// Bayesian Network; Learner Model
    /// </summary>
    private BayesianNetwork bayesNet;
    /// <summary>
    /// Holds the percentage (in float) of each denominator's mastery rate; initiated to 0.5
    /// </summary>
    private List<DenominatorMastery> denominatorMastery;
    /// <summary>
    /// Username of the learner model's owner
    /// </summary>
    private string owner;
    /// <summary>
    /// Folder path of the owner's data.
    /// </summary>
	private string folderDataPath;
    /// <summary>
    /// Filepath of the learner model json file.
    /// </summary>
	private string learnerModelDataPath;

    /// <summary>
    /// Denominators that can be unlocked.
    /// </summary>
    private List<int> unlockableDenominators;//Denominators that can be unlocked;

    /// <summary>
    /// Constructor
    /// </summary>
	public LearnerModelingComponent() {
		this.bayesNet = new BayesianNetwork ();
		this.denominatorMastery = new List<DenominatorMastery> ();
		this.unlockableDenominators = new List<int> ();

		if (PlayerPrefs.HasKey ("Username"))
			this.owner = PlayerPrefs.GetString ("Username");
		else
			this.owner = "default";

		folderDataPath = Application.persistentDataPath + "/Game Data/user_" + this.owner;
		learnerModelDataPath = folderDataPath + "/" + this.owner + "_LearnerModel.json";

		//Unlockable denominators
		//NOTE: SINCE WE'LL BE THE ONES SUPPLYING THE DENOMINATORS, I DID NOT MAKE A HANDLE FOR DUPLICATES
		this.unlockableDenominators.Add (4);
		this.unlockableDenominators.Add (2);
		this.unlockableDenominators.Add (3);
		this.unlockableDenominators.Add (6);
		this.unlockableDenominators.Add (8);
		this.unlockableDenominators.Add (9);

		LoadLearnerModel ();
	}

    /// <summary>
    /// Attempts to load an existing learner model if available.
    /// </summary>
	public void LoadLearnerModel() {
		if (File.Exists (this.learnerModelDataPath)) {
			LearnerModelData data = JsonUtility.FromJson<LearnerModelData> (File.ReadAllText (this.learnerModelDataPath));

			this.bayesNet.fbSimilarAddition = data.additionSimilar;
			this.bayesNet.fbSimilarSubtraction = data.subtractionSimilar;
			this.bayesNet.UpdateSimilarProbability ();

			this.bayesNet.fbEquivalentAddition = data.additionEquivalent;
			this.bayesNet.fbEquivalentSubtraction = data.subtractionEquivalent;
			this.bayesNet.UpdateEquivalentProbability ();

			this.bayesNet.fbDissimilarAddition = data.additionDissimilar;
			this.bayesNet.fbDissimilarSubtraction = data.subtractionDissimilar;
			this.bayesNet.UpdateDissimilarProbability ();

			this.bayesNet.UpdateLearnerModel ();

			for (int i = 0; i < this.unlockableDenominators.Count; i++) {
				DenominatorMastery mastery = new DenominatorMastery (unlockableDenominators [i]);
				List<DenominatorMasteryData> masteries = data.denominatorMasteries.FindAll (x => x.denominator == this.unlockableDenominators [i]);
				foreach (DenominatorMasteryData masteryData in masteries) {
					mastery.AddMastery (masteryData.topic, masteryData.mastery);
				}

				denominatorMastery.Add (mastery);
			}
		} else {
			for (int i = 0; i < this.unlockableDenominators.Count; i++) {
				DenominatorMastery mastery = new DenominatorMastery (unlockableDenominators [i]);
				mastery.AddMastery (SceneTopic.SIMILAR_ADD);
				mastery.AddMastery (SceneTopic.SIMILAR_SUB);
				mastery.AddMastery (SceneTopic.EQUIVALENT_ADD);
				mastery.AddMastery (SceneTopic.EQUIVALENT_SUB);
				mastery.AddMastery (SceneTopic.DISSIMILAR_ADD);
				mastery.AddMastery (SceneTopic.DISSIMILAR_SUB);

				denominatorMastery.Add (mastery);
			}
		}
	}

    /// <summary>
    /// Saves the learner model as a json file to the filepath for he file.
    /// </summary>
	public void SaveLearnerModel() {
		LearnerModelData lm = new LearnerModelData ();

		lm.additionSimilar = this.GetRateofObservedNode (LearnerModel.Node.Similar, LearnerModel.Operation.ADDITION);
		lm.subtractionSimilar = this.GetRateofObservedNode (LearnerModel.Node.Similar, LearnerModel.Operation.SUBTRACTION);
		lm.similar = this.Infer (LearnerModel.Node.Similar);

		lm.additionEquivalent = this.GetRateofObservedNode (LearnerModel.Node.Equivalent, LearnerModel.Operation.ADDITION);
		lm.subtractionEquivalent = this.GetRateofObservedNode (LearnerModel.Node.Equivalent, LearnerModel.Operation.SUBTRACTION);
		lm.equivalent = this.Infer (LearnerModel.Node.Equivalent);

		lm.additionDissimilar = this.GetRateofObservedNode (LearnerModel.Node.Dissimilar, LearnerModel.Operation.ADDITION);
		lm.subtractionEquivalent = this.GetRateofObservedNode (LearnerModel.Node.Dissimilar, LearnerModel.Operation.SUBTRACTION);
		lm.dissimilar = this.Infer (LearnerModel.Node.Dissimilar);

		foreach (DenominatorMastery mastery in this.denominatorMastery) {
			DenominatorMasteryData data = new DenominatorMasteryData ();
			data.denominator = mastery.GetDenominator ();
			data.topic = SceneTopic.SIMILAR_ADD;
			data.mastery = mastery.Mastery (data.topic);
			lm.denominatorMasteries.Add (data);

			data = new DenominatorMasteryData ();
			data.denominator = mastery.GetDenominator ();
			data.topic = SceneTopic.SIMILAR_SUB;
			data.mastery = mastery.Mastery (data.topic);
			lm.denominatorMasteries.Add (data);

			data = new DenominatorMasteryData ();
			data.denominator = mastery.GetDenominator ();
			data.topic = SceneTopic.EQUIVALENT_ADD;
			data.mastery = mastery.Mastery (data.topic);
			lm.denominatorMasteries.Add (data);

			data = new DenominatorMasteryData ();
			data.denominator = mastery.GetDenominator ();
			data.topic = SceneTopic.EQUIVALENT_SUB;
			data.mastery = mastery.Mastery (data.topic);
			lm.denominatorMasteries.Add (data);

			data = new DenominatorMasteryData ();
			data.denominator = mastery.GetDenominator ();
			data.topic = SceneTopic.DISSIMILAR_ADD;
			data.mastery = mastery.Mastery (data.topic);
			lm.denominatorMasteries.Add (data);

			data = new DenominatorMasteryData ();
			data.denominator = mastery.GetDenominator ();
			data.topic = SceneTopic.DISSIMILAR_SUB;
			data.mastery = mastery.Mastery (data.topic);
			lm.denominatorMasteries.Add (data);
		}

		string newData = JsonUtility.ToJson (lm);
		File.WriteAllText (learnerModelDataPath, newData);
	}

    /// <summary>
    /// Update an observed node with the given score.
    /// </summary>
    /// <param name="node">Main Topic</param>
    /// <param name="operation">Operation</param>
    /// <param name="score">Score</param>
	public void UpdateNode(LearnerModel.Node node, LearnerModel.Operation operation, double score) {
		switch (node) {
		case LearnerModel.Node.Similar:
			if (operation == LearnerModel.Operation.ADDITION)
				this.bayesNet.fbSimilarAddition = (score + this.bayesNet.fbSimilarAddition) / 2;
			else if (operation == LearnerModel.Operation.SUBTRACTION)
				this.bayesNet.fbSimilarSubtraction = (score + this.bayesNet.fbSimilarSubtraction) / 2;

			this.bayesNet.UpdateSimilarProbability ();
			break;
		case LearnerModel.Node.Equivalent:
			if (operation == LearnerModel.Operation.ADDITION)
				this.bayesNet.fbEquivalentAddition = (score + this.bayesNet.fbEquivalentAddition) / 2;
			else if (operation == LearnerModel.Operation.SUBTRACTION)
				this.bayesNet.fbEquivalentSubtraction = (score + this.bayesNet.fbEquivalentSubtraction) / 2;

			this.bayesNet.UpdateEquivalentProbability ();
			break;
		case LearnerModel.Node.Dissimilar:
			if (operation == LearnerModel.Operation.ADDITION)
				this.bayesNet.fbDissimilarAddition = (score + this.bayesNet.fbDissimilarAddition) / 2;
			else if (operation == LearnerModel.Operation.SUBTRACTION)
				this.bayesNet.fbDissimilarSubtraction = (score + this.bayesNet.fbDissimilarSubtraction) / 2;

			this.bayesNet.UpdateDissimilarProbability ();
			break;
		}
	}

    /// <summary>
    /// Update mastery for the specified denominator for the specified sub-topic.
    /// </summary>
    /// <param name="denom">Denominator</param>
    /// <param name="score">Score</param>
    /// <param name="topic">Sub-Topic</param>
	public void UpdateDenominatorMastery(int denom, float score, SceneTopic topic) {
		this.denominatorMastery.Find (x => x.GetDenominator () == denom).UpdateMastery (topic, score);
		Debug.Log (GetMasteryOfDenominator(denom, topic)+" is Mastery of "+denom+" "+topic);
	}

    /// <summary>
    /// Retrieve mastery of the specified denominator for that sub-topic.
    /// </summary>
    /// <param name="denom">Denominator</param>
    /// <param name="topic">Sub-Topic</param>
    /// <returns></returns>
	public float GetMasteryOfDenominator(int denom, SceneTopic topic) {
		return this.denominatorMastery.Find (x => x.GetDenominator () == denom).Mastery (topic);
	}
    
    /// <summary>
    /// Function call to update the bayesian network (learner model).
    /// </summary>
	public void UpdateLearnerModel() {
		this.bayesNet.UpdateLearnerModel ();
	}

    /// <summary>
    /// Infer the probability that the specified main topic is mastered.
    /// </summary>
    /// <param name="node">Main Topic</param>
    /// <returns>Probability</returns>
	public double Infer(LearnerModel.Node node) {
		return this.bayesNet.Infer (node);
	}

    /// <summary>
    /// Infer the probability of an observed node.
    /// </summary>
    /// <param name="node">Main Topic</param>
    /// <param name="operation">Operation</param>
    /// <returns></returns>
	public double GetRateofObservedNode(LearnerModel.Node node, LearnerModel.Operation operation) {
		double rate = 0d;

		switch (node) {
		case LearnerModel.Node.Similar:
			if (operation == LearnerModel.Operation.ADDITION)
				rate = this.bayesNet.fbSimilarAddition;
			else if (operation == LearnerModel.Operation.SUBTRACTION)
				rate = this.bayesNet.fbSimilarSubtraction;
			break;
		case LearnerModel.Node.Equivalent:
			if (operation == LearnerModel.Operation.ADDITION)
				rate = this.bayesNet.fbEquivalentAddition;
			else if (operation == LearnerModel.Operation.SUBTRACTION)
				rate = this.bayesNet.fbEquivalentSubtraction;
			break;
		case LearnerModel.Node.Dissimilar:
			if (operation == LearnerModel.Operation.ADDITION)
				rate = this.bayesNet.fbDissimilarAddition;
			else if (operation == LearnerModel.Operation.SUBTRACTION)
				rate = this.bayesNet.fbDissimilarSubtraction;
			break;
		}

		return rate;
	}

    /// <summary>
    /// Function call to request a denominator to be used for the specified sub-topic.
    /// </summary>
    /// <param name="topic">Sub-Topic</param>
    /// <returns>Denominator</returns>
	public int RequestDenominator(SceneTopic topic) {
		//assign first unlockable as default
		int denom = this.unlockableDenominators [0];

		if (topic == SceneTopic.NONE) {
//			Debug.LogError ("Topic: none");
			denom = this.unlockableDenominators [UnityEngine.Random.Range (0, this.unlockableDenominators.Count)];
		}
		else {
//			Debug.LogError (this.unlockableDenominators.Count+" count");
			for (int i = 1; i < this.unlockableDenominators.Count; i++) {
				//if the previous unlockable denominator has reached treshold, add the current unlockable denominator
//				Debug.LogError("Denominator Mastery of " + this.unlockableDenominators[i-1] + " is " + this.denominatorMastery.Find (x => x.GetDenominator () == this.unlockableDenominators [i - 1]).Mastery (topic).ToString());
				if (this.denominatorMastery.Find (x => x.GetDenominator () == this.unlockableDenominators [i - 1]).Mastery (topic) > 0.7f) {
					denom = this.unlockableDenominators [i];
//					Debug.LogError ("New Denominator: " + denom);
				}
				else
					break;
			}
		}

		return denom;
	}
}
