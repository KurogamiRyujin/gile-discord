using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using SessionTags;
using System;

public class StabilizationSession {

	private SessionTopic topic;

	private double sceneAvgSolveTime;
	private DateTime startTime;
	private DateTime endTime;
	private TimeSpan elapsedTime;

	private List<int> correctAddition;
	private List<int> correctSubtraction;

	private List<int> wrongAddition;
	private List<int> wrongSubtraction;

	private List<int> allDistinct;

	public StabilizationSession(SessionTopic topic, double avgSolveTime) {
		this.topic = topic;
		this.sceneAvgSolveTime = avgSolveTime;

		this.correctAddition = new List<int> ();
		this.correctSubtraction = new List<int> ();
		this.wrongAddition = new List<int> ();
		this.wrongSubtraction = new List<int> ();

		this.allDistinct = new List<int> ();

		this.startTime = DateTime.Now;
	}

	public double GetSceneAvgSolveTime() {
		return this.sceneAvgSolveTime;
	}

	public void RecordTime() {
		this.endTime = DateTime.Now;

		this.elapsedTime = this.endTime - this.startTime;
	}

	public string StartTime() {
		return this.startTime.ToString ();
	}

	public string EndTime() {
		return this.endTime.ToString ();
	}

	public double SessionTime() {
		return this.elapsedTime.TotalSeconds;
	}

	public void SetTopic(SessionTopic topic) {
		this.topic = topic;
	}

	public void Tally(int denom, bool correctlyUsed, Operation operation) {
		switch (operation) {
		case Operation.ADDITION:
			if (correctlyUsed) {
				correctAddition.Add (denom);
				Debug.Log ("CORRECT AD'N");
			} else {
				wrongAddition.Add (denom);
				Debug.Log ("NOT CORRECT AD'N");
			}
			break;
		case Operation.SUBTRACTION:
			if (correctlyUsed)
				correctSubtraction.Add (denom);
			else
				wrongSubtraction.Add (denom);
			break;
		}

		if (!this.allDistinct.Contains (denom))
			this.allDistinct.Add (denom);
	}

	public int Count(bool correct, Operation operation) {
		if (correct) {
			if (operation == Operation.ADDITION)
				return this.correctAddition.Count;
			else if (operation == Operation.SUBTRACTION)
				return this.correctSubtraction.Count;
			else {
				Debug.Log ("No such operation exists.");
				return 0;
			}
		} else {
			if (operation == Operation.ADDITION)
				return this.wrongAddition.Count;
			else if (operation == Operation.SUBTRACTION)
				return this.wrongSubtraction.Count;
			else {
				Debug.Log ("No such operation exists.");
				return 0;
			}
		}
	}

	public int[] GetDenominaorsIn(bool correct, Operation operation) {
		if (correct) {
			if (operation == Operation.ADDITION)
				return this.correctAddition.ToArray ();
			else if (operation == Operation.SUBTRACTION)
				return this.correctSubtraction.ToArray ();
			else {
				Debug.Log ("No such operation exists.");
				return null;
			}
		} else {
			if (operation == Operation.ADDITION)
				return this.wrongAddition.ToArray ();
			else if (operation == Operation.SUBTRACTION)
				return this.wrongSubtraction.ToArray ();
			else {
				Debug.Log ("No such operation exists.");
				return null;
			}
		}
	}

	public int[] GetAllDistinct() {
		return this.allDistinct.ToArray ();
	}

	public SessionTopic GetTopic() {
		return this.topic;
	}
}
