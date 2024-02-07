using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HollowBlockEntryv2 {
	public string name { get; set; }
	//one entry for each hollow block interacted
	//add a new list of attempted answers on block break
	//the list represents the values of the skyfragment pieces
	public List<AttemptedAnswerv2> attemptedAnswers { get; set; }

	public HollowBlockEntryv2(string name) {
		this.name = name;
		this.attemptedAnswers = new List<AttemptedAnswerv2> ();
	}
}
