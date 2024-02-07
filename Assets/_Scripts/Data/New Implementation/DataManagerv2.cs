using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using System.Linq;

public class DataManagerv2 : MonoBehaviour {

	void Awake() {
	}

	void OnDestroy() {
	}

	// Use this for initialization
	void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
		
	}

	public static void CreateEmptyEntry (Tuple<string, int> key) {
//		Tuple<string, int> key = new Tuple<string, int>("Hollow Block", id);

		PlayerDatav2.hollowBlockTally.Add (key, null);
	}

	public static void AddHollowBlockEntry(HollowBlockEntry entry) {
		Tuple<string, int> last = PlayerDatav2.hollowBlockTally.Keys.Last ();
		PlayerDatav2.hollowBlockTally.Add (last, entry);
		//		Debug.Log (last);
	}

	public static void RemoveHollowBlockEntry(Tuple<string, int> key) {
		PlayerDatav2.hollowBlockTally.Remove (key);
	}

	public static void UpdateHollowBlockEntry(Tuple<string, int> key, HollowBlockEntry entry) {
		if (PlayerDatav2.hollowBlockTally.ContainsKey (key)) {
			PlayerDatav2.hollowBlockTally [key] = entry;
		}

		PrintHollowBlockDictionary ();
	}

	public static HollowBlockEntry GetHollowBlockEntry(Tuple<string, int> key)
	{
		if (PlayerDatav2.hollowBlockTally.ContainsKey(key))
			return PlayerDatav2.hollowBlockTally[key];
		return null;
	}

	public static Tuple<string, int> GetHollowBlockLastKey() {
		return PlayerDatav2.hollowBlockTally.Keys.Last ();
	}

	public static bool DoesKeyExist(Tuple<string, int> key) {
		print ("Key " + key.Item1 + ", " + key.Item2);
		if (key.Item1.Equals ("Hollow Block")) {
			if (PlayerDatav2.hollowBlockTally.ContainsKey (key))
				return true;
			return false;
		}

		return false;
	}

	public static void PrintHollowBlockDictionary() {
		print ("Hollow block tally count: "+PlayerDatav2.hollowBlockTally.Keys.Count);
		//		foreach (Tuple<Entry.Type, int> key in PlayerData.tally.Keys) {
		//			print (key.Item1 + " " + key.Item2);
		//		}
		foreach (KeyValuePair<Tuple<string, int>, HollowBlockEntry> kvp in PlayerDatav2.hollowBlockTally) {
			print ("KEY: " + kvp.Key.Item1 + ", " + kvp.Key.Item2 + " VALUE: Name: " + kvp.Value.name + 
				", Topic: " + kvp.Value.topic + ", Attempted answers: " + String.Join (", ", kvp.Value.attemptedAnswers.Select (x => x.GetNumerator() + "/" + x.GetDenominator())));
		}
	}

	public static int GetNumberOfTotalAttempts(SceneTopic topic) {
		int count = 0;
		foreach (KeyValuePair<Tuple<string, int>, HollowBlockEntry> e in PlayerDatav2.hollowBlockTally) {
			print (e.Value.topic);
			if (e.Value.topic == topic)
				count++;
		}

		print ("Number of total attempts: " + count);
		return count;
	}

	public static int GetNumberOfCorrectAttempts(SceneTopic topic) {
		int count = 0;
		foreach (KeyValuePair<Tuple<string, int>, HollowBlockEntry> e in PlayerDatav2.hollowBlockTally) {
			print (e.Value.topic);
			if (e.Value.topic == topic)
				count++;
		}

		print ("Number of correct attempts: " + count);
		return count;
	}
}
