using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using System.Linq;
using SubsetSumUtils;
using UnityEngine.SceneManagement;
using SessionTags;

/// <summary>
/// Handles the tallying of the session.
/// 
/// Keeps track of the current blocks in the room and prompts the stability number line to update accordingly. Also prompts for the learner modeling component's update.
/// </summary>
public class DataRecorderv2 : MonoBehaviour {

	[SerializeField] private double sceneAvgSolveTime = 300d;

    /// <summary>
    /// Topic of the room's puzzle based on the ghost blocks and their values.
    /// </summary>
	private SessionTopic topic = SessionTopic.SIMILAR;

	public const string HOLLOW_BLOCK = "Hollow Block";

    /// <summary>
    /// List of all ghost blocks in the room.
    /// </summary>
	private List<HollowBlock> hollowBlocks;
    /// <summary>
    /// Subset sum of all possible combinations of filled blocks in the room that would sum to the stability number line's target value.
    /// </summary>
	private List<FractionSet> additionRight;
    /// <summary>
    /// Reference to the statbility number line.
    /// </summary>
	private StabilityNumberLine numberLine;
    /// <summary>
    /// Instance of the current session.
    /// Holds all the attempts made by the player until the room is stabilized.
    /// </summary>
	private StabilizationSession currentSession;
    /// <summary>
    /// List of all sessions in the room.
    /// A room may have more than one session if the stability was destabilized again after stabilization.
    /// </summary>
	private List<StabilizationSession> sessionList;
    /// <summary>
    /// Reference to the stability controller.
    /// </summary>
	private StabilityController stabilityController;

    /// <summary>
    /// Set of fractions taken from all the ghost blocks in the room.
    /// </summary>
	private FractionSet fractionSet;

    /// <summary>
	/// Unity Function. Called once upon creation of the object.
	/// </summary>
	void Awake() {
		this.stabilityController = GetComponent<StabilityController> ();
		hollowBlocks = new List<HollowBlock> ();
		additionRight = new List<FractionSet> ();
		sessionList = new List<StabilizationSession> ();
		fractionSet = new FractionSet ();
	
		EventBroadcaster.Instance.AddObserver (EventNames.RECORD_HOLLOW_BLOCK, this.RecordData);
		EventBroadcaster.Instance.AddObserver (EventNames.RECORD_ON_AREA_STABLE, RecordSession);
		EventBroadcaster.Instance.AddObserver (EventNames.ON_BLOCK_SPAWN, this.RegisterBlock);
		EventBroadcaster.Instance.AddObserver (EventNames.ON_BLOCK_DESTROY, this.UnregisterBlock);
		EventBroadcaster.Instance.AddObserver (EventNames.REQUEST_UPDATE_SESSION, this.UpdateSession);

		SceneManager.sceneUnloaded += this.OnSceneUnloaded;


		InitSession ();
	}

    /// <summary>
    /// Raises the destroy event.
    /// </summary>
	void OnDestroy() {
		EventBroadcaster.Instance.RemoveActionAtObserver (EventNames.RECORD_HOLLOW_BLOCK, this.RecordData);
		EventBroadcaster.Instance.RemoveActionAtObserver (EventNames.RECORD_ON_AREA_STABLE, this.RecordSession);
		EventBroadcaster.Instance.RemoveActionAtObserver (EventNames.ON_BLOCK_SPAWN, this.RegisterBlock);
		EventBroadcaster.Instance.RemoveActionAtObserver (EventNames.ON_BLOCK_DESTROY, this.UnregisterBlock);
		EventBroadcaster.Instance.RemoveActionAtObserver (EventNames.REQUEST_UPDATE_SESSION, this.UpdateSession);

	}

    /// <summary>
    /// Standard Unity Function. Called once in the game object's lifetime to initiate the script once it has been enabled.
    /// </summary>
    void Start () {
//		this.hollowBlocks.AddRange (FindObjectsOfType<HollowBlock> ());
		numberLine = FindObjectOfType<StabilityNumberLine> ();
		UpdateSession ();
	}

    /// <summary>
    /// Registers a new ghost block into the recorder.
    /// Registering a new block will prompt an update for the session.
    /// </summary>
    /// <param name="data"></param>
	private void RegisterBlock(Parameters data) {
		HollowBlock block = data.GetHollowBlockExtra ("hollowBlock");
		if (!this.hollowBlocks.Contains (block)) {
			this.hollowBlocks.Add (block);

			if (block.IsSolved ()) {
				fractionSet.fractions.Add (block.GetFraction ());
				Debug.Log ("Fraction added: " + block.GetFraction ().numerator + "/" + block.GetFraction ().denominator);
			}

			UpdateSession ();
		}
	}

    /// <summary>
    /// Unregister a ghost block from this record.
    /// Unregistering will prompt an update for the session.
    /// </summary>
    /// <param name="data"></param>
	private void UnregisterBlock(Parameters data) {
		HollowBlock block = data.GetHollowBlockExtra ("hollowBlock");
		if (this.hollowBlocks.Contains (block)) {
			this.hollowBlocks.Remove (block);

//			if (filledFractions.Contains (filledFractions.Find (x => x.numerator == block.GetFraction ().numerator
//			   && x.denominator == block.GetFraction ().denominator)))
//				filledFractions.Remove (filledFractions.Find (x => x.numerator == block.GetFraction ().numerator && x.denominator == block.GetFraction ().denominator));

			if (fractionSet.fractions.Remove (fractionSet.fractions.Find (x => x.numerator == block.GetFraction ().numerator && x.denominator == block.GetFraction ().denominator)))
				Debug.Log ("Removed Fraction: " + block.GetFraction ().numerator + "/" + block.GetFraction ().denominator);
			else
				Debug.Log ("Failed to Remove: " + block.GetFraction ().numerator + "/" + block.GetFraction ().denominator);

			UpdateSession ();
		}
	}

    /// <summary>
    /// Takes all currently registered ghost blocks and checks their values to determine what topic the puzzle is.
    /// Also updates the stability number line based on the currently registered blocks' values.
    /// </summary>
	private void UpdateSession() {
		List<int> denominators = new List<int> ();
		FractionSet fset = new FractionSet ();
		bool similar = true;
		bool oddEquiv = true;
		bool evenEquiv = true;

		int currentDenom = (int)hollowBlocks [0].GetDenominator ();

		for (int i = 0; i < hollowBlocks.Count; i++) {
			denominators.Add ((int) hollowBlocks[i].GetDenominator ());
			FractionData fraction = hollowBlocks [i].GetFraction ();

			if (i > 0) {
				if (similar && currentDenom == fraction.denominator)
					similar = true;
				else
					similar = false;

				if (evenEquiv && currentDenom % 2 == 0 && (currentDenom % fraction.denominator == 0 || fraction.denominator % currentDenom == 0 || (General.SimplifyFraction (fraction.numerator, fraction.denominator) [0] == 1.0f && General.SimplifyFraction (fraction.numerator, fraction.denominator) [1] == 2.0f)))
					evenEquiv = true;
				else
					evenEquiv = false;

				if (oddEquiv && currentDenom % 3 == 0 && (currentDenom % fraction.denominator == 0 || fraction.denominator % currentDenom == 0 || (General.SimplifyFraction (fraction.numerator, fraction.denominator) [0] == 1.0f && General.SimplifyFraction (fraction.numerator, fraction.denominator) [1] == 3.0f) || (General.SimplifyFraction (fraction.numerator, fraction.denominator) [0] == 2.0f && General.SimplifyFraction (fraction.numerator, fraction.denominator) [1] == 3.0f)))
					oddEquiv = true;
				else
					oddEquiv = false;
				
//				if (similar && fraction.denominator == (int)hollowBlocks [i - 1].GetDenominator ())
//					similar = true;
//				else
//					similar = false;
//
//				if (evenEquiv && (fraction.denominator % 2) == ((int)hollowBlocks [i - 1].GetDenominator () % 2))
//					evenEquiv = true;
//				else
//					evenEquiv = false;
//
//				if (oddEquiv && (fraction.denominator % 3) == ((int)hollowBlocks [i - 1].GetDenominator () % 3))
//					oddEquiv = true;
//				else
//					oddEquiv = false;
			}

			fset.fractions.Add (fraction);
		}

		if (similar)
			topic = SessionTopic.SIMILAR;
		else if (evenEquiv || oddEquiv)
			topic = SessionTopic.EQUIVALENT;
		else
			topic = SessionTopic.DISSIMILAR;

		this.currentSession = new StabilizationSession (topic, this.sceneAvgSolveTime);

		if (numberLine == null)
			this.numberLine = FindObjectOfType<StabilityNumberLine> ();

		FractionData target = this.stabilityController.UpdateStabilityNumberLine (hollowBlocks);

//		FractionData target = new FractionData ();
//		target.numerator = (int)numberLine.GetTargetNumerator ();
//		target.denominator = (int)numberLine.GetTargetDenominator ();

		foreach (FractionSet s in SubsetSumFractions.SubsetSum(fset, target)) {
			additionRight.Add (s);
		}
	}

    /// <summary>
    /// Initializes a new session. Subset sum set is cleared.
    /// </summary>
	private void InitSession() {
		additionRight.Clear ();
		this.currentSession = new StabilizationSession (topic, sceneAvgSolveTime);
	}

    /// <summary>
    /// Record the data of the player's attempt.
    /// Evaluates if the attempt is right or wrong by comparing the set of fllled blocks with all the instances of the subset sum set.
    /// The comparision looks if the set of attempts are closer or farther from any subset sum.
    /// </summary>
    /// <param name="data"></param>
	private void RecordData(Parameters data) {
		HollowBlock block = data.GetHollowBlockExtra ("hollowBlock");
		bool solved = data.GetBoolExtra ("solved", true);

		FractionSet oldFracSet = new FractionSet ();
		oldFracSet.fractions.AddRange (fractionSet.fractions);
		FractionData blockFraction = block.GetFraction ();

		if (solved)
			fractionSet.fractions.Add (block.GetFraction ());
		else
			fractionSet.fractions.Remove (fractionSet.fractions.Find (x => x.numerator == block.GetFraction ().numerator
			&& x.denominator == block.GetFraction ().denominator));

		Debug.Log ("Old size: " + oldFracSet.fractions.Count);

		FractionSet newFracSet = new FractionSet ();
		newFracSet.fractions.AddRange (fractionSet.fractions);

		Debug.Log ("New size: " + newFracSet.fractions.Count);

		bool found = false;
		bool improve = false;
		int oldCounter = 0;
		int newCounter = 0;

		foreach (FractionSet s in additionRight) {
			FractionSet oldTemp = oldFracSet;
			FractionSet newTemp = newFracSet;
			oldCounter = 0;
			newCounter = 0;

			for (int i = 0; i < s.fractions.Count; i++) {
				if (oldTemp.fractions.Remove (oldTemp.fractions.Find (x => x.numerator == s.fractions [i].numerator
				    && x.denominator == s.fractions [i].denominator)))
					oldCounter++;
				else
					oldCounter--;

				if (newTemp.fractions.Remove (newTemp.fractions.Find (x => x.numerator == s.fractions [i].numerator
				    && x.denominator == s.fractions [i].denominator)))
					newCounter++;
				else
					newCounter--;
			}

			oldCounter -= oldFracSet.fractions.Count;
			newCounter -= newFracSet.fractions.Count;

			Debug.Log ("New: " + newCounter + " VS Old: " + oldCounter);

			if (newCounter > oldCounter) {
				improve = true;
				break;
			}
		}

		if (solved) {//This means it is Addition
			if (improve)
				this.currentSession.Tally (blockFraction.denominator, true, Operation.ADDITION);
			else
				this.currentSession.Tally (blockFraction.denominator, false, Operation.ADDITION);
		} else {//Else, its Subtraction
			if (improve)
				this.currentSession.Tally (blockFraction.denominator, true, Operation.SUBTRACTION);
			else
				this.currentSession.Tally (blockFraction.denominator, false, Operation.SUBTRACTION);
		}
	}

//	private void RecordData(Parameters data) {
//		HollowBlock block = data.GetHollowBlockExtra ("hollowBlock");
//		bool solved = data.GetBoolExtra ("solved", true);
//
//		FractionData blockFraction = block.GetFraction ();
//
//		if (block.IsSolved ())
//			fractionSet.fractions.Add (block.GetFraction ());
//		else
//			fractionSet.fractions.Remove (block.GetFraction ());
//
//		bool found = false;
//		float[] simplifiedFraction;
//		float[] simplifiedBlockFraction;
//
//		simplifiedBlockFraction = General.SimplifyFraction (blockFraction.numerator, blockFraction.denominator);
//
//		//TODO: must consider it correct subtraction if removing one of multiple occurences
//		Debug.LogError ("additionRight CNT "+additionRight.Count);
//		foreach (FractionSet s in additionRight) {
//			Debug.LogError ("s.fractions CNT "+s.fractions.Count);
//
//
//
//			Debug.LogError (blockFraction.numerator + "/" + blockFraction.denominator + " Fraction Count: "
//			+ s.fractions.FindAll (x => General.SimplifyFraction (x.numerator, x.denominator) [0] == General.SimplifyFraction (blockFraction.numerator, blockFraction.denominator) [0]
//			&& General.SimplifyFraction (x.numerator, x.denominator) [1] == General.SimplifyFraction (blockFraction.numerator, blockFraction.denominator) [1]).Count);
//
//			foreach (FractionData fraction in s.fractions) {
//				simplifiedFraction = General.SimplifyFraction (fraction.numerator, fraction.denominator);
//
//				Debug.LogError ("FRAC PRINT "+simplifiedFraction[0]+ "/"+simplifiedFraction[1]);
//				Debug.LogError ("BLOCK FRAC PRINT "+simplifiedBlockFraction[0]+ "/"+simplifiedBlockFraction[1]);
//				if (simplifiedFraction[0] == simplifiedBlockFraction[0]
//					&& simplifiedFraction[1] == simplifiedBlockFraction[1])
//					found = true;
////				if (fraction.numerator == blockFraction.numerator && fraction.denominator == blockFraction.denominator)
////					found = true;
//			}
//		}
//
//		if (solved) {//This means it is Addition
//			if (found)
//				this.currentSession.Tally (blockFraction.denominator, true, Operation.ADDITION);
//			else
//				this.currentSession.Tally (blockFraction.denominator, false, Operation.ADDITION);
//		} else {//Else, its Subtraction
//			if (!found)
//				this.currentSession.Tally (blockFraction.denominator, true, Operation.SUBTRACTION);
//			else
//				this.currentSession.Tally (blockFraction.denominator, false, Operation.SUBTRACTION);
//		}
//	}

    /// <summary>
    /// Records the current session and puts it into the list of sessions of the room.
    /// Creates a new session through InitSession.
    /// </summary>
	private void RecordSession() {
		this.currentSession.RecordTime ();
		this.sessionList.Add (this.currentSession);
		ConsoleLogCurrentSession ();

		InitSession ();
	}
    
	private void ConsoleLogCurrentSession() {
		string log = "Current Session Topic: " + this.topic.ToString () + "\n";

		log += "Correct Addition: " + this.currentSession.Count (true, Operation.ADDITION) + "\n";
		log += "Correct Subtraction: " + this.currentSession.Count (true, Operation.SUBTRACTION) + "\n";
		log += "Wrong Addition: " + this.currentSession.Count (false, Operation.ADDITION) + "\n";
		log += "Wrong Subtraction: " + this.currentSession.Count (false, Operation.SUBTRACTION) + "\n";

		Debug.Log (log);
	}

    /// <summary>
    /// When the current room is unloaded, push sessions into the pedagogical component to be assessd and generate a score from the attempts.
    /// The scores will be used to update the learner model.
    /// </summary>
    /// <param name="current"></param>
	private void OnSceneUnloaded(Scene current) {
		Debug.Log ("SCENE UNLOADED");
		DataManagerv3.AddSessions (current.name, this.sessionList);
		DataManagerv3.RecordSessionTimes ();
		PedagogicalComponent_v2.Instance.UpdateLearnerModel (this.sessionList);
		PedagogicalComponent_v2.Instance.SaveLearnerModel ();
		PedagogicalComponent_v2.Instance.UpdateCurrentDenominator ();
		SceneManager.sceneUnloaded -= this.OnSceneUnloaded;
	}

    /// <summary>
	/// Standard Unity Function. Called once every frame.
	/// </summary>
	void Update() {
		if (Input.GetKeyDown (KeyCode.Space)) {
			Debug.Log ("Saved");
			PedagogicalComponent_v2.Instance.SaveLearnerModel ();
		}
	}
}
