using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using BNUtils;

/// <summary>
/// Entry for a denominator mastery which shows how much the learner has mastered a particular denominator.
/// </summary>
public class DenominatorMastery {
    /// <summary>
    /// Denominator for this entry.
    /// </summary>
	private int denominator;

    /// <summary>
    /// Dictionary for the mastery rates of the denominator for each sub-topic.
    /// </summary>
	private Dictionary<SceneTopic, float> masteries;

    /// <summary>
    /// Constructor
    /// </summary>
    /// <param name="denom">Denominator</param>
	public DenominatorMastery(int denom) {
		this.denominator = denom;

		masteries = new Dictionary<SceneTopic, float> ();
	}

    /// <summary>
    /// Retrieves the denominator for this entry.
    /// </summary>
    /// <returns>Denominator</returns>
	public int GetDenominator() {
		return this.denominator;
	}
    
    /// <summary>
    /// Adds a sub-topic to the denominator's mastery dictionary.
    /// </summary>
    /// <param name="topic">Sub-Topic</param>
	public void AddMastery(SceneTopic topic) {
		masteries.Add (topic, 0.5f);
	}

    /// <summary>
    /// Adds a sub-topic to the denominator's mastery dictionary with a given initial grade.
    /// </summary>
    /// <param name="topic">Sub-Topic</param>
    /// <param name="grade">Initial Grade</param>
	public void AddMastery(SceneTopic topic, float grade) {
		masteries.Add (topic, grade);
	}

    /// <summary>
    /// Function call to update a mastery with the score.
    /// </summary>
    /// <param name="topic">Sub-Topic</param>
    /// <param name="score">Score</param>
	public void UpdateMastery(SceneTopic topic, float score) {
		if (masteries.ContainsKey (topic))
			masteries [topic] = (score + masteries [topic]) / 2;
		else
			Debug.Log ("<color=red>No Topic Exists.</color>");
	}

    /// <summary>
    /// Infers the mastery rate of the specified sub-topic.
    /// </summary>
    /// <param name="topic">Sub-Topic</param>
    /// <returns>Mastery Rate</returns>
	public float Mastery(SceneTopic topic) {
		if (masteries.ContainsKey (topic))
			return masteries [topic];
		else {
			Debug.Log ("<color=red>No Topic Exist</color>");
			return 0.0f;
		}
	}
}
