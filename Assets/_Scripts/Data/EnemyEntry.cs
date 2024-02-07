using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public class EnemyEntry : Entry {

	public int hammerValue { get; set; }
    public Tuple<int, int> initialValue { get; set; }
    public Tuple<int, int> targetValue { get; set; }
    public bool? isSimilar { get; set; }
    public bool? isProper { get; set; }
    public bool? isDeadThroughLCD { get; set; }

    public EnemyEntry()
    {
        this.name = null;
        this.timeWeaponInteracted = null;
        this.timeWeaponRemoved = null;
        this.initialValue = null;
        this.targetValue = null;
        this.attemptedAnswers = null;
        this.timeSolved = null;
        this.interactionCount = 0;
        this.numberOfAttempts = 0;
        this.isSimilar = null;
        this.isProper = null;
        this.isDeadThroughLCD = null;
		this.topic = Topic.None;
		this.hammerValue = 0;
    }
}
