using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TutorialList : MonoBehaviour {
	[SerializeField] private TutorialScreenCanvas tutorialCanvas;

	public void Open() {
		this.gameObject.SetActive (true);
	}

	public void Close() {
		this.gameObject.SetActive (false);
	}


	public void OpenUnstableRoom() {
		SoundManager.Instance.Play (AudibleNames.Button.BASIC, false);
		this.GetTutorialCanvas ().OpenUnstableRoom ();
	}

	// TODO: Tutorials
	public void OpenItemCarry() {
		SoundManager.Instance.Play (AudibleNames.Button.BASIC, false);
		this.GetTutorialCanvas ().OpenCarryItems ();
	}
	public void OpenFixingBoxes() {
		SoundManager.Instance.Play (AudibleNames.Button.BASIC, false);
		this.GetTutorialCanvas ().OpenFixingBlocks ();
	}
	public void OpenUsingNeedle() {
		SoundManager.Instance.Play (AudibleNames.Button.BASIC, false);
		this.GetTutorialCanvas ().OpenUsingNeedle ();
	}
	public void OpenUsingHammer() {
		SoundManager.Instance.Play (AudibleNames.Button.BASIC, false);
		this.GetTutorialCanvas ().OpenUsingHammer ();
	}
	public void OpenPowerCharms() {
		SoundManager.Instance.Play (AudibleNames.Button.BASIC, false);
		this.GetTutorialCanvas ().OpenPowerCharms ();
	}
	public void OpenSubdividing() {
		SoundManager.Instance.Play (AudibleNames.Button.BASIC, false);
		this.GetTutorialCanvas ().OpenSubdividing ();
	}
	public TutorialScreenCanvas GetTutorialCanvas() {
		if (this.tutorialCanvas == null) {
			this.tutorialCanvas = GetComponentInParent<TutorialScreenCanvas> ();
		}
		return this.tutorialCanvas;
		
	}
}
