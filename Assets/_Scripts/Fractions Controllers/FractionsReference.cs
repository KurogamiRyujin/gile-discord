using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FractionsReference {
	private static FractionsReference instance;
	private List<int> validDenominators;
	private List<System.Action<List<int>>> listeners;
//	private AbstractPedagogicalComponent pedagogicalComponent;

	private List<int> chosenEnemyDenominators;

	private FractionsReference() {
		Random.InitState (65162);
		validDenominators = new List<int> ();
		chosenEnemyDenominators = new List<int> ();
		listeners = new List<System.Action<List<int>>> ();

//		UpdateValidDenominators ();
	}

	public void UpdateValidDenominators(List<int> validDenominators) {
//		if (pedagogicalComponent == null) {
//			pedagogicalComponent = GameObject.FindGameObjectWithTag ("GameController").GetComponent<AbstractPedagogicalComponent> ();
//			Debug.LogError ("Attempted to find pedagogical component"); // TODO
//		}

//		this.validDenominators = validDenominators;

//		this.validDenominators = DataManager.GetTopNDenominators(StringConstants.TableNames.ALL, 3, false);

		this.validDenominators = validDenominators;
//		List<int> topDenominators = DataManager.GetTopNDenominators (StringConstants.TableNames.ALL, 3, false);
//		if (topDenominators.Count != 0) {
//			foreach (int denom in topDenominators) {
//				validDenominators.Add (denom);
//			}
////			validDenominators.AddRange (topDenominators);
//		}

//		if (pedagogicalComponent == null/* || validDenominators.Count <= 0*/) {
//			if(pedagogicalComponent == null)
//				Debug.LogError ("Oh fuck");
//			if (validDenominators.Count <= 0)
//				Debug.LogError ("0 counts"); // TODO
//			EmergencyDenominatorGenerate ();
//		}
//		else
//			validDenominators = pedagogicalComponent.GetValidDenominators ();
	}

	private void EmergencyDenominatorGenerate() {
		Debug.Log ("Emergency Denominator Generation Activated");
		validDenominators = new List<int> ();

		for (int i = 0; i < Random.Range (3, 30); i++) {
			validDenominators.Add (Random.Range (3, 10));
		}
		Debug.Log ("Denominators:" + validDenominators.Count);
	}

	public void AddObserver(System.Action<List<int>> listener) {
		if (!this.listeners.Contains (listener)) {
			listeners.Add (listener);
			Debug.Log ("Listener Added");
		}
	}

	public void RemoveObserver(System.Action<List<int>> listener) {
		if (this.listeners.Contains (listener))
			listeners.Remove (listener);
	}

	public static FractionsReference Instance() {
		if (instance == null) {
			instance = new FractionsReference ();
		}

		return instance;
	}

	public void UpdateFractionRange(System.Action<List<int>> listener) {
		if (this.listeners.Contains (listener)) {
//			if (validDenominators.Count <= 0)
//				EmergencyDenominatorGenerate ();
			listener (validDenominators);
			Debug.Log ("Called it");
		}
	}

	//Returns a list of valid denominators for all that use denominators
	public List<int> RequestDenominators() {
		List<int> denominators = new List<int> ();

		denominators.AddRange (this.validDenominators);

		return denominators;
	}

	//Just in case pooling is implemented
	public void UpdateFractionsRange() {
		foreach (System.Action<List<int>> listener in listeners) {
//			if (validDenominators.Count <= 0)
//				EmergencyDenominatorGenerate ();
			listener (validDenominators);
		}
	}

	//Adds a denominator chosen by an enemy
	public void AddEnemyDenominator(int denominator) {
		if(!chosenEnemyDenominators.Contains(denominator))
			this.chosenEnemyDenominators.Add (denominator);
	}

	//Clears chosen enemy denominators when scene is unloaded
	public void ClearEnemyDenominators() {
		this.chosenEnemyDenominators.Clear ();
	}

	//used by Yarnball pedestal to know which denominators are used by enemies
	public List<int> RequestEnemyDenominators() {
		List<int> denominators = new List<int> ();

		denominators.AddRange (this.chosenEnemyDenominators);

		return denominators;
	}
}
