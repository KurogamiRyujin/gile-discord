using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public class HollowBlockEntry {
	public string name { get; set; }
	//one entry for each hollow block interacted
	//add a new list of attempted answers on block break
	//the list represents the values of the skyfragment pieces
//	public List<List<AttemptedAnswer>> attemptedAnswers { get; set; } 
	public List<AttemptedAnswer> attemptedAnswers { get; set; } 
	//contains a list of indices of correct attempts on attempted answers
	public List<int> correctAttempts { get; set; }
	public SceneTopic topic;

	public HollowBlockEntry() {
		this.name = "Hollow Block";
//		this.attemptedAnswers = new List<List<AttemptedAnswer>> ();
		this.attemptedAnswers = new List<AttemptedAnswer> ();
		this.correctAttempts = new List<int> ();
		this.topic = SceneTopic.NONE;
	}
}
