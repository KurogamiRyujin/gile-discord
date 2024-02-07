using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public class LCDEntry: Entry {

	public int hammerValue { get; set; }
	public string name { get; set; }
	public int yarnballValue { get; set; }
	public int initialNumerator { get; set; }
	public int initialDenominator { get; set; }
	public int convertedDenominator { get; set; }
	public int actualNumerator { get; set; } 	
	public List<int> attemptedNumerators { get; set; }
	public Entry.Topic topic { get; set; }

	public LCDEntry() {
		this.name = null;
		this.yarnballValue = 0;
		this.initialNumerator = 0;
		this.initialDenominator = 0;
		this.convertedDenominator = 0;
		this.actualNumerator = 0;
		this.attemptedNumerators = null;
		this.topic = Entry.Topic.Conversion;
		this.hammerValue = 0;
	}

}
