using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public class POEntry : Entry {
	
	public Tuple<int, int> objectGiven { get; set; }

	public POEntry() {
        this.name = null;
        this.timeWeaponInteracted = null;
        this.timeWeaponRemoved = null;
        this.objectGiven = null;
        this.attemptedAnswers = null;
        this.timeSolved = null;
        this.interactionCount = 0;
        this.numberOfAttempts = 0;
		this.topic = Topic.Similar;
    }
}
