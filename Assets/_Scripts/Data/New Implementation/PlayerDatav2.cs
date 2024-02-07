using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public class PlayerDatav2 : MonoBehaviour {

	public static Dictionary<Tuple<string, int>, HollowBlockEntry> hollowBlockTally;
	public static int hollowBlockIndex;

	private static PlayerDatav2 sharedInstance = null;
	public static PlayerDatav2 Instance {
		get {
			return sharedInstance;
		}
	}

	void Awake() {
		sharedInstance = this;

		hollowBlockTally = new Dictionary<Tuple<string, int>, HollowBlockEntry> ();
		hollowBlockIndex = 1;
		General.DontDestroyChildOnLoad (this.gameObject);
	}

	void OnDestroy() {
		sharedInstance = null;
	}

	public Dictionary<Tuple<string, int>, HollowBlockEntry> GetHollowBlockTally() {
		return hollowBlockTally;
	}

	public static void IncrementHollowBlockIndex() {
		hollowBlockIndex++;
	}
}
