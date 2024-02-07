using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using BNUtils;
using SessionTags;

//<summary>
//This component handles the values used by the hollow blocks and yarnballs.
//It can turn the learner modeling component off which basically means it will not refer to it when giving values.
//
//It is a singleton for the purposes that it should not have more than one instance.
//This and only this can make use of the learner modeling component.
//</summary>
public class PedagogicalComponent_v2 {
	private static PedagogicalComponent_v2 sharedInstance = null;

	public static PedagogicalComponent_v2 Instance {
		get {
			if(sharedInstance == null)
				sharedInstance = new PedagogicalComponent_v2();

			return sharedInstance;
		}
	}

	private int currentDenominator;
	private LearnerModelingComponent learnerModelingComponent;

	private SceneTopic currentTopic = SceneTopic.SIMILAR_ADD;

	//special case for dissimilar
	private Queue<FractionData> usedFractions;

	private PedagogicalComponent_v2() {
		Random.InitState (System.DateTime.Now.Millisecond);
		this.usedFractions = new Queue<FractionData> ();
	}

	public void InitializeLearnerModelingComponent() {
		this.learnerModelingComponent = new LearnerModelingComponent ();

		this.currentDenominator = this.learnerModelingComponent.RequestDenominator (currentTopic);
		UpdateCurrentDenominator ();
	}

	public void UpdateLearnerModel(List<StabilizationSession> sessions) {
		if (this.learnerModelingComponent == null)
			InitializeLearnerModelingComponent ();

		foreach (StabilizationSession session in sessions) {
			SceneTopic addTopic = SceneTopic.NONE;
			SceneTopic subTopic = SceneTopic.NONE;

			switch (session.GetTopic ()) {
			case SessionTags.SessionTopic.SIMILAR:
				addTopic = SceneTopic.SIMILAR_ADD;
				subTopic = SceneTopic.SIMILAR_SUB;
				break;
			case SessionTags.SessionTopic.EQUIVALENT:
				addTopic = SceneTopic.EQUIVALENT_ADD;
				subTopic = SceneTopic.EQUIVALENT_SUB;
				break;
			case SessionTags.SessionTopic.DISSIMILAR:
				addTopic = SceneTopic.DISSIMILAR_ADD;
				subTopic = SceneTopic.DISSIMILAR_SUB;
				break;
			}

			UpdateDenominatorMastery (session, addTopic, subTopic);

			UpdateNode (session, addTopic, subTopic);
		}

		this.learnerModelingComponent.UpdateLearnerModel ();
	}

	private void UpdateDenominatorMastery(StabilizationSession session, SceneTopic addTopic, SceneTopic subTopic) {
		if (this.learnerModelingComponent == null)
			InitializeLearnerModelingComponent ();
		//Update the denominator mastery
		List<int> correctAddition = new List<int>(session.GetDenominaorsIn(true, SessionTags.Operation.ADDITION));
		List<int> correctSubtraction = new List<int> (session.GetDenominaorsIn (true, SessionTags.Operation.SUBTRACTION));
		List<int> wrongAddition = new List<int> (session.GetDenominaorsIn (false, SessionTags.Operation.ADDITION));
		List<int> wrongSubtraction = new List<int> (session.GetDenominaorsIn (false, SessionTags.Operation.SUBTRACTION));

		List<int> denominators = new List<int> (session.GetAllDistinct ());

		float additionCount = 0f;
		float subtractionCount = 0f;

		float correctAdditionCount = 0f;
		float wrongAdditionCount = 0f;
		float correctSubtractionCount = 0f;
		float wrongSubtractionCount = 0f;

		float scoreAdd = 0f;
		float scoreSub = 0f;

		foreach (int denominator in denominators) {
			//number of times denominator was used correctly in addition
			correctAdditionCount = correctAddition.FindAll (x => x == denominator).Count;
			//number of times denominator was used wrongly in addition
			wrongAdditionCount = wrongAddition.FindAll (x => x == denominator).Count;
			//number of times denominator was used correctly in subtraction
			correctSubtractionCount = correctSubtraction.FindAll (x => x == denominator).Count;
			//number of times denominator was used wrongly in subraction
			wrongSubtractionCount = wrongSubtraction.FindAll (x => x == denominator).Count;

			//total occurences of denominator in addition
			additionCount = correctAdditionCount + wrongAdditionCount;
			//total occurences of denominator in subtraction
			subtractionCount = correctSubtractionCount + wrongSubtractionCount;

			//if denominator did occur in addition...
			if (additionCount != 0) {
				//correct over all occurences = score
				scoreAdd = correctAdditionCount / additionCount;

				// TODO: Removed this 3
//				scoreAdd = scoreAdd - (float)(session.SessionTime () / session.GetSceneAvgSolveTime ());

				//update the learner modeling component's mastery for that denominator in addTopic (addition topic)
				this.learnerModelingComponent.UpdateDenominatorMastery (denominator, scoreAdd, addTopic);
			}

			//if denominator did occur in subtraction...
			if (subtractionCount != 0) {
				//correct over all occurences = score
				scoreSub = correctSubtractionCount / subtractionCount;

				// TODO: Removed this 4
//				scoreSub = scoreSub - (float)(session.SessionTime () / session.GetSceneAvgSolveTime ());

				//update the learner modeling component's mastery for that denominator in subTopic (subtraction topic)
				this.learnerModelingComponent.UpdateDenominatorMastery (denominator, scoreSub, subTopic);
			}
		}
	}

	private void UpdateNode(StabilizationSession session, SceneTopic addTopic, SceneTopic subTopic) {
		if (this.learnerModelingComponent == null)
			InitializeLearnerModelingComponent ();
		//count the correct and wrong attempts for addition
		float correctAdditionCount = (float)session.Count(true, SessionTags.Operation.ADDITION);
		float wrongAdditionCount = (float)session.Count (false, SessionTags.Operation.ADDITION);

		//add them together to get total addition attempts
		float additionCount = correctAdditionCount + wrongAdditionCount;

		//if there are addition attempts...
		if(additionCount != 0) {
			//get score by dividing correct attempts over total attempts
			float score = correctAdditionCount / additionCount;
			Debug.LogError ("num den for score IS "+correctAdditionCount+"   "+additionCount);
//			score = score - (float)(session.SessionTime () / session.GetSceneAvgSolveTime ());
//			Debug.LogError ("SCORE MINUS IS "+(session.SessionTime () / session.GetSceneAvgSolveTime ()));
//			score = score;
			Debug.LogError ("SCORE IS "+score);
			//depending on topic, update the respective node
			switch (addTopic) {
			case SceneTopic.SIMILAR_ADD:
				this.learnerModelingComponent.UpdateNode(LearnerModel.Node.Similar, LearnerModel.Operation.ADDITION, score);
				break;
			case SceneTopic.EQUIVALENT_ADD:
				this.learnerModelingComponent.UpdateNode(LearnerModel.Node.Equivalent, LearnerModel.Operation.ADDITION, score);
				break;
			case SceneTopic.DISSIMILAR_ADD:
				this.learnerModelingComponent.UpdateNode(LearnerModel.Node.Dissimilar, LearnerModel.Operation.ADDITION, score);
				break;
			}
		}

		//count the correct and wrong attempts for subtraction
		float correctSubtractionCount = (float)session.Count (true, SessionTags.Operation.SUBTRACTION);
		float wrongSubtractionCount = (float)session.Count (false, SessionTags.Operation.SUBTRACTION);

		//add them together to get total subtraction attempts
		float subtractionCount = correctSubtractionCount + wrongSubtractionCount;

		//if there are subtraction attempts...
		if (subtractionCount != 0) {
			//get score by dividing correct attempts over total attempts
			float score = correctSubtractionCount / subtractionCount;

			// TODO Also removed
//			score = score - (float)(session.SessionTime () / session.GetSceneAvgSolveTime ());
			//depending on topic, update the respective node
			switch (subTopic) {
			case SceneTopic.SIMILAR_SUB:
				this.learnerModelingComponent.UpdateNode(LearnerModel.Node.Similar, LearnerModel.Operation.SUBTRACTION, score);
				break;
			case SceneTopic.EQUIVALENT_SUB:
				this.learnerModelingComponent.UpdateNode(LearnerModel.Node.Equivalent, LearnerModel.Operation.SUBTRACTION, score);
				break;
			case SceneTopic.DISSIMILAR_SUB:
				this.learnerModelingComponent.UpdateNode(LearnerModel.Node.Dissimilar, LearnerModel.Operation.SUBTRACTION, score);
				break;
			}
		}
	}

	public void UpdateCurrentDenominator() {
		if (this.learnerModelingComponent == null)
			InitializeLearnerModelingComponent ();

		float mastery = this.learnerModelingComponent.GetMasteryOfDenominator (this.currentDenominator, this.currentTopic);
		Debug.LogError ("Update Current Denom: mastery = " + mastery);
		//if the current denominator has met threshold...
		if (mastery > 0.7f) {
			//if learner modeling component still gives the same denom, then all denoms in that topic have broken threshold, thus...
			Debug.LogError("Checking requested denom...");
			if (this.currentDenominator == this.learnerModelingComponent.RequestDenominator (this.currentTopic)) {
				Debug.LogError ("Switching topic to...");
				switch (this.currentTopic) {
				case SceneTopic.SIMILAR_ADD:
					//change topic to similar sub
					Debug.LogError("Similar Sub");
					this.currentTopic = SceneTopic.SIMILAR_SUB;
					break;
				case SceneTopic.SIMILAR_SUB:
					Debug.LogError("Equivalent Add");
					//change topic to equivalent add
					this.currentTopic = SceneTopic.EQUIVALENT_ADD;
					break;
				case SceneTopic.EQUIVALENT_ADD:
					Debug.LogError("Equivalent Sub");
					//change topic to equivalent sub
					this.currentTopic = SceneTopic.EQUIVALENT_SUB;
					break;
				case SceneTopic.EQUIVALENT_SUB:
					Debug.LogError("Dissimilar Add");
					//change topic to dissimilar add
					this.currentTopic = SceneTopic.DISSIMILAR_ADD;
					break;
				case SceneTopic.DISSIMILAR_ADD:
					Debug.LogError("Dissimilar Sub");
					//change topic to dissimilar sub
					this.currentTopic = SceneTopic.DISSIMILAR_SUB;
					break;
				case SceneTopic.DISSIMILAR_SUB:
					Debug.LogError("Finished all. None.");
					//if this is the current topic and a switch of topic is requested, then all denominators have been mastered.
					//change topic to none to start pseudo-random selection of all denominators
					this.currentTopic = SceneTopic.NONE;
					break;
				case SceneTopic.NONE:
					Debug.LogError ("No more.");
					break;
				}
			}
		}
		this.currentDenominator = this.learnerModelingComponent.RequestDenominator (this.currentTopic);
		Debug.LogError ("Updated current denom is: " + this.currentDenominator + " for topic " + this.currentTopic);
	}

	public FractionData RequestFraction(SceneTopic topic) {
		if (this.learnerModelingComponent == null)
			InitializeLearnerModelingComponent ();

		FractionData fraction = new FractionData ();
		int denom = this.learnerModelingComponent.RequestDenominator (topic);
		int numerator = Random.Range (1, denom + 1);

		List<int> nums = new List<int> ();
		int temp = denom;

		switch (topic) {
		case SceneTopic.SIMILAR_ADD:
		case SceneTopic.SIMILAR_SUB:
			//do nothing
			break;
		case SceneTopic.EQUIVALENT_ADD:
		case SceneTopic.EQUIVALENT_SUB:
			if (denom % 2 == 0 && denom != 6) {
				nums.Add (denom);

				//using + increment to include 3/6
				temp = denom + 2;

				//for now, limiting the denom to lower than 10 due to block slicing limitations
				while (temp < 10) {
					if (!nums.Contains (temp))
						nums.Add (temp);
					temp += 2;
				}
				temp = denom - 2;
				while (temp > 1) {
					if (!nums.Contains (temp))
						nums.Add (temp);
					temp -= 2;
				}

				denom = nums [Random.Range (0, nums.Count)];

				//control if its 6
				if (denom == 6)
					numerator = 3;
				else
					numerator = Random.Range (1, denom + 1);
			} else if (denom % 3 == 0 && denom != 6) {
				nums.Add (denom);

				temp = denom + 3;

				while (temp < 10) {
					if (!nums.Contains (temp))
						nums.Add (temp);
					temp += 3;
				}

				temp = denom - 3;
				while (temp > 2) {
					if (!nums.Contains (temp))
						nums.Add (temp);
					temp -= 3;
				}

				denom = nums [Random.Range (0, nums.Count)];

				//control if its 6
				if (denom == 6)
					numerator = 2 * Random.Range (1, 3);
				else
					numerator = Random.Range (1, denom + 1);
			} else if(denom % 6 == 0) {
				nums.Add (denom);
				nums.Add (2);
				nums.Add (3);

				temp = denom + 6;
				while (temp < 10) {
					if (!nums.Contains (temp))
						nums.Add (temp);
					temp += 6;
				}

				temp = denom - 6;
				while (temp > 5) {
					if (!nums.Contains (temp))
						nums.Add (temp);
					temp -= 6;
				}

				denom = nums [Random.Range (0, nums.Count)];
				numerator = Random.Range (1, denom + 1);
			}
			break;
		case SceneTopic.DISSIMILAR_ADD:
		case SceneTopic.DISSIMILAR_SUB:
			//50% or more chance to encounter currentDenominator during dissimilar
			for (int i = 0; i < 5; i++) {
				nums.Add (denom);
			}
			for (int i = 0; i < 5; i++) {
				nums.Add (this.learnerModelingComponent.RequestDenominator (SceneTopic.NONE));
			}

			denom = nums [Random.Range (0, nums.Count)];
			numerator = Random.Range (1, denom + 1);
			break;
		}

		fraction.numerator = numerator;
		fraction.denominator = denom;

		this.usedFractions.Enqueue (fraction);

		return fraction;
	}

	public FractionData RequestFraction() {
		if (this.learnerModelingComponent == null)
			InitializeLearnerModelingComponent ();

		FractionData fraction = new FractionData ();
		int denom = this.currentDenominator;
		int numerator = Random.Range (1, denom + 1);

		List<int> nums = new List<int> ();
		int temp = denom;

		switch (this.currentTopic) {
		case SceneTopic.SIMILAR_ADD:
		case SceneTopic.SIMILAR_SUB:
			//do nothing
			break;
		case SceneTopic.EQUIVALENT_ADD:
		case SceneTopic.EQUIVALENT_SUB:
			if (denom % 2 == 0 && denom != 6) {
				FractionData[] fractemp = new FractionData[usedFractions.Count];
				usedFractions.CopyTo (fractemp, 0);

				List<FractionData> fracs = new List<FractionData> (fractemp);
				if (!fracs.Contains (fracs.Find (x => x.denominator == denom)))
					nums.Add (denom);
				else {
					//using + increment to include 3/6
					temp = denom + 2;

					//for now, limiting the denom to lower than 10 due to block slicing limitations
					while (temp < 10) {
						if (!nums.Contains (temp))
							nums.Add (temp);
						temp += 2;
					}
					temp = denom - 2;
					while (temp > 1) {
						if (!nums.Contains (temp))
							nums.Add (temp);
						temp -= 2;
					}
				}

				denom = nums [Random.Range (0, nums.Count)];

				//control if its 6
				if (denom == 6)
					numerator = 3;
				else
					numerator = Random.Range (1, denom + 1);
			} else if (denom % 3 == 0 && denom != 6) {
				FractionData[] fractemp = new FractionData[usedFractions.Count];
				usedFractions.CopyTo (fractemp, 0);

				List<FractionData> fracs = new List<FractionData> (fractemp);
				if (!fracs.Contains (fracs.Find (x => x.denominator == denom)))
					nums.Add (denom);
				else {
					temp = denom + 3;

					while (temp < 10) {
						if (!nums.Contains (temp))
							nums.Add (temp);
						temp += 3;
					}

					temp = denom - 3;
					while (temp > 2) {
						if (!nums.Contains (temp))
							nums.Add (temp);
						temp -= 3;
					}
				}

				denom = nums [Random.Range (0, nums.Count)];

				//control if its 6
				if (denom == 6)
					numerator = 2 * Random.Range (1, 3);
				else
					numerator = Random.Range (1, denom + 1);
			} else if(denom % 6 == 0) {
				FractionData[] fractemp = new FractionData[usedFractions.Count];
				usedFractions.CopyTo (fractemp, 0);

				List<FractionData> fracs = new List<FractionData> (fractemp);
				if (!fracs.Contains (fracs.Find (x => x.denominator == denom)))
					nums.Add (denom);
				else {
					nums.Add (2);
					nums.Add (3);

					temp = denom + 6;
					while (temp < 10) {
						if (!nums.Contains (temp))
							nums.Add (temp);
						temp += 6;
					}

					temp = denom - 6;
					while (temp > 5) {
						if (!nums.Contains (temp))
							nums.Add (temp);
						temp -= 6;
					}
				}

				denom = nums [Random.Range (0, nums.Count)];
				numerator = Random.Range (1, denom + 1);
			}
			break;
		case SceneTopic.DISSIMILAR_ADD:
		case SceneTopic.DISSIMILAR_SUB:
			//50% or more chance to encounter currentDenominator during dissimilar
			for (int i = 0; i < 5; i++) {
				nums.Add (denom);
			}
			for (int i = 0; i < 5; i++) {
				nums.Add (this.learnerModelingComponent.RequestDenominator (SceneTopic.NONE));
			}

			denom = nums [Random.Range (0, nums.Count)];
			numerator = Random.Range (1, denom + 1);
			break;
		}

		fraction.numerator = numerator;
		fraction.denominator = denom;

		this.usedFractions.Enqueue (fraction);

		return fraction;
	}

	//used by charms
	public int RequestDenominator() {
		if (this.usedFractions.Count > 0)
			return this.usedFractions.Dequeue ().denominator;
		else
			return 0;
	}

	public SceneTopic CurrentTopic() {
		return this.currentTopic;
	}

	//is called every time a fresh set of hollow blocks are created
	public void ClearFractionsQueue() {
		this.usedFractions.Clear ();
	}

	public void SaveLearnerModel() {
		if (this.learnerModelingComponent == null)
			InitializeLearnerModelingComponent ();
		this.learnerModelingComponent.SaveLearnerModel ();
	}

	public void PrintLearnerModel() {
		string log = "";

		log += "Similar: " + this.learnerModelingComponent.Infer (LearnerModel.Node.Similar);
		log += "Equivalent: " + this.learnerModelingComponent.Infer (LearnerModel.Node.Equivalent);
		log += "Dissimilar: " + this.learnerModelingComponent.Infer (LearnerModel.Node.Dissimilar);

		Debug.LogError (log);
	}
}
