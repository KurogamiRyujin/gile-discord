using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class TutorialHandler : MonoBehaviour {
	[SerializeField] private bool isFinished;
	public abstract void Action ();
	void Awake() {
		this.Initialize ();
	}

	protected void Initialize() {
		this.isFinished = false;
	}

	public void TutorialClosed() {
		this.isFinished = true;
	}

	public bool IsFinished() {
		return this.isFinished;
	}
}
