using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TutorialScreen : MonoBehaviour {
	[SerializeField] private TutorialScreenAnimatable animatable;


	// Functions to  be used by ScreenParent
	public void Open() {
		this.gameObject.SetActive (true);
		this.PlayNext ();
	}

	public void Close() {
		this.gameObject.SetActive (false);
		this.PlayPrevious ();
	}



	public void PlayNext() {
		this.GetAnimatable ().Play ();
	}

	public void PlayPrevious() {
		this.GetAnimatable ().Close ();
	}


	public TutorialScreenAnimatable GetAnimatable() {
		if (this.animatable == null) {
			this.animatable = GetComponent<TutorialScreenAnimatable> ();
		}
		return this.animatable;
	}

	public void PlaySoundNeedle() {
		SoundManager.Instance.Play (AudibleNames.Needle.BASIC, false);
	}

	public void PlaySoundSuccess() {
		SoundManager.Instance.Play (AudibleNames.Results.SUCCESS, false);
	}
	public void PlayDoorClose() {
		SoundManager.Instance.Play (AudibleNames.Door.CLOSE, false);
	}
	public void PlayDoorOpen() {
		SoundManager.Instance.Play (AudibleNames.Door.OPEN, false);
	}
	public void PlaySkyFragmentDrop() {
		SoundManager.Instance.Play (AudibleNames.Room.PICKUP_FRAGMENT, false);
	}
	public void PlayPickupYarnball() {
		SoundManager.Instance.Play (AudibleNames.LCDInterface.DECREASE, false);
	}
	public void PlayIncrease() {
		SoundManager.Instance.Play (AudibleNames.LCDInterface.INCREASE, false);
	}
	public void PlayHammerSmash() {
		SoundManager.Instance.Play (AudibleNames.Hammer.HIT, false);
	}
}
