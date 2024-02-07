using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public class Entry {
	public enum Type {PartitionableObject, LCD, Enemy}; 
	public enum Topic {None, Similar, Dissimilar, Conversion}; 

//	public Type identifier { get; set; } //This will be the identifier. It will be used to determine which columns to fill up
//	public int number { get; set; } //This indicates the count of the object per type.
	public string name { get; set; } //The object's given name. This can be anything and will only be used to help identify the object. Can be null.
//	public DateTime timeActivated { get; set; } //This is the time that the object is made available for player interaction.
//	public DateTime timeInteracted { get; set; } //This is the timestamp when the player interacted with the object. This is for interactions not using the needle.
	public List<DateTime> timeWeaponInteracted { get; set; }//This is a list of timestamps of when the player hit the object with the needle. This should have the same length as Time Needle Removed
	public List<DateTime> timeWeaponRemoved { get; set; }//This is the list of timestamps it took for the player to enter their answer (remove the needle).

//	public Tuple<int, int> objectGiven { get; set; }
	public Tuple<int,int> actualAnswer { get; set; }
	public List<Tuple<int,int>> attemptedAnswers { get; set; }

	public DateTime? timeSolved { get; set; }//This is the timestamp when the player entered the correct answer.
	public int interactionCount { get; set; }//Counts the number of times the player interacted with the object using the needle.
	public int numberOfAttempts { get; set; }//Counts the number of times the player interacted with the object using the needle in an attempt to solve the box. An interaction is considered as an attempt if the player enters a non-zero numerator (partition fill).

	public Topic topic { get; set; }

	public Entry() {

	}

//	public Entry(Type identifier, string name, int number, DateTime activated) {
//		identifier = identifier;
//		number = number;
//		name = name;
//		timeNeedleInteracted = activated;
//	}
}
