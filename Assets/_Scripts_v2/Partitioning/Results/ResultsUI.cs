using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ResultsUI : MonoBehaviour {

	// Manages all ResultAnimatables
	[SerializeField] private ResultSuccessAnimatable successAnimatable;
	void Awake () {
		this.successAnimatable = GetComponentInChildren<ResultSuccessAnimatable> ();	
	}
//	void Start () {
//		this.successAnimatable = GetComponentInChildren<ResultSuccessAnimatable> ();	
//	}

	public void RemoveComponents() {
		Destroy (this);
	}

	public void PlaySuccess() {
		Debug.Log ("ENTERED PLAY SUCCESS");
		this.successAnimatable.Show ();
	}

	public void PlayCraft() {
		Debug.Log ("ENTERED PLAY Craft");
		this.successAnimatable.Craft ();
	}

	// Return an 'AND' of all animatables
	public bool IsPlaying() {
		return (successAnimatable.IsPlaying());
	}
}
