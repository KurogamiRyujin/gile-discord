using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using System.Linq;

public class DataRecorder : MonoBehaviour {

	private HollowBlock[] hollowBlocks;	
	private Tuple<string, int> hollowBlockKey;
	private Tuple<string, int> currentKey;
	private SceneTag sceneTag;

	void Awake() {
		EventBroadcaster.Instance.AddObserver (EventNames.RECORD_ON_AREA_STABLE, RecordData);
	}

	void OnDestroy() {
		EventBroadcaster.Instance.RemoveObserver (EventNames.RECORD_ON_AREA_STABLE);
	}

	// Use this for initialization
	void Start () {
		sceneTag = GameObject.FindGameObjectWithTag ("SceneTag").GetComponent<SceneTag> ();
		hollowBlocks = FindObjectsOfType<HollowBlock>();
	}
	
	// Update is called once per frame
	void Update () {
		
	}

	void RecordData() {
		foreach (HollowBlock block in hollowBlocks) {
			if (block.IsSolved ()) {
				currentKey = new Tuple<string, int> ("Hollow Block", PlayerDatav2.hollowBlockIndex);
				DataManagerv2.CreateEmptyEntry (currentKey);
				HollowBlockEntry entry = new HollowBlockEntry ();
				entry.name = "Hollow Block " + PlayerDatav2.hollowBlockIndex;
				entry.topic = sceneTag.GetSceneTopic ();
				foreach (SkyFragmentPiece piece in block.GetSkyPieceContainer().GetSkyPieces()) {
					AttemptedAnswer attempt = new AttemptedAnswer ();
					attempt.SetNumerator (piece.GetNumerator ());
					attempt.SetDenominator (piece.GetDenominator ());
					entry.attemptedAnswers.Add (attempt);
				}
				DataManagerv2.UpdateHollowBlockEntry (DataManagerv2.GetHollowBlockLastKey (), entry);
				PlayerDatav2.IncrementHollowBlockIndex ();
			}
//			entry.correctAttempts = new List<int> ();
		}
		DataManagerv2.PrintHollowBlockDictionary ();
	}
}
