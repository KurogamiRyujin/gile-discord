using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using System.Linq;

public class PlayerData : MonoBehaviour {

	public static Dictionary<Tuple<Entry.Type, int>, POEntry> POTally;
	public static Dictionary<Tuple<Entry.Type, int>, EnemyEntry> enemyTally;
	public static Dictionary<Tuple<Entry.Type, int>, LCDEntry> LCDTally;
	public static int partitionableObjectIndex;
	public static int enemyIndex;
	public static int LCDIndex;

	private static PlayerData sharedInstance = null;
	public static PlayerData Instance {
		get {
			return sharedInstance;
		}
	}

	void Awake() {
		sharedInstance = this;

		POTally = new Dictionary<Tuple<Entry.Type, int>, POEntry> ();
		enemyTally = new Dictionary<Tuple<Entry.Type, int>, EnemyEntry> ();
		LCDTally = new Dictionary<Tuple<Entry.Type, int>, LCDEntry> ();
		partitionableObjectIndex = 1;
		enemyIndex = 1;
		LCDIndex = 1;
		General.DontDestroyChildOnLoad (this.gameObject);
	}

	void OnDestroy() {
		sharedInstance = null;
	}

	// Use this for initialization
	void Start () {
//		POTally = new Dictionary<Tuple<Entry.Type, int>, POEntry> ();
//		enemyTally = new Dictionary<Tuple<Entry.Type, int>, EnemyEntry> ();
//		LCDTally = new Dictionary<Tuple<Entry.Type, int>, LCDEntry> ();
//		partitionableObjectIndex = 1;
//		enemyIndex = 1;
//		LCDIndex = 1;
//		Entry entry = new Entry (Entry.Type.PartitionableObject, "Hi", 1, System.DateTime.Now);
//		tally.Add (1, entry);
//		AddEntry (entry);
	}

	public Dictionary<Tuple<Entry.Type, int>, POEntry> GetTally() {
		return POTally;
	}

	public Dictionary<Tuple<Entry.Type, int>, EnemyEntry> GetEnemyTally() {
		return enemyTally;
	}

	public Dictionary<Tuple<Entry.Type, int>, LCDEntry> GetLCDTally() {
		return LCDTally;
	}

	public static void IncrementPOIndex() {
		partitionableObjectIndex++;
	}

	public void IncrementEnemyIndex() {
		enemyIndex++;
	}

	public void IncrementLCDIndex() {
		LCDIndex++;
	}

	// Update is called once per frame
	void Update () {
		
	}
}
