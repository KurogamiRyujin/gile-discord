using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;

public class ScreenParent : MonoBehaviour {
	[SerializeField] private List<TutorialScreen> screens;
	[SerializeField] private int screenIndex;
	[SerializeField] private TutorialScreenCanvas tutorialCanvas;

	void Awake() {
		this.screenIndex = -1;
	}

	public void ParentClose() {
		this.Close ();
		this.GetTutorialCanvas ().Close ();
	}
	public void Close() {
		gameObject.SetActive (false);
		this.CloseAll ();
	}

	public void Open() {
		this.CloseAll ();
		this.screenIndex = -1;
		gameObject.SetActive (true);
		this.Next ();
	}

	public void Next() {
		if (this.IncrementScreenIndex ()) { // Don't play animation if it did not increment
			this.CloseAll ();
			this.GetScreens () [this.screenIndex].Open ();
			SoundManager.Instance.Play (AudibleNames.Button.BASIC, false);
		}
	}

	public void Previous() {
		if (this.DecrementScreenIndex ()) {
			this.CloseAll ();
			this.GetScreens () [this.screenIndex].Open ();
			SoundManager.Instance.Play (AudibleNames.Button.BASIC, false);
		}
	}

	public void CloseAll() {
		foreach (TutorialScreen screen in this.GetScreens()) {
			screen.Close ();
		}
	}



	public bool IncrementScreenIndex() {
		this.screenIndex += 1;
		if (this.screenIndex >= this.GetScreens ().Count) {
			this.screenIndex = this.GetScreens ().Count - 1;
			return false; // Return false if it did not increment
		}
		return true;
	}
	public bool DecrementScreenIndex() {
		this.screenIndex -= 1;
		if (this.screenIndex < 0) {
			this.screenIndex = 0;
			return false;  // Return false if it did not decrement
		}
		return true;
	}
	public List<TutorialScreen> GetScreens() {
		if (this.screens == null || this.screens.Count == 0) {
			this.screens = GetComponentsInChildren<TutorialScreen> ().ToList ();
		}
		return this.screens;
	}

	public TutorialScreenCanvas GetTutorialCanvas() {
		if (this.tutorialCanvas == null) {
			this.tutorialCanvas = GetComponentInParent<TutorialScreenCanvas> ();
		}
		return this.tutorialCanvas;
	}
}
