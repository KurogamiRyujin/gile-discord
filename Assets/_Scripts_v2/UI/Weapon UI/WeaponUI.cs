using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WeaponUI : MonoBehaviour {
	[SerializeField] private HammerUIAnimatable hammerAnimatable;
	[SerializeField] private NeedleUIAnimatable needleAnimatable;

	private bool isPlayingCoroutine;

	void Start () {
		this.hammerAnimatable = GetComponentInChildren<HammerUIAnimatable> ();
		this.needleAnimatable = GetComponentInChildren<NeedleUIAnimatable> ();


		EventBroadcaster.Instance.AddObserver (EventNames.ON_SWITCH_NEEDLE, SwitchToNeedle);
		EventBroadcaster.Instance.AddObserver (EventNames.ON_SWITCH_HAMMER, SwitchToHammer);
	}

	void OnDestroy() {
		EventBroadcaster.Instance.RemoveObserver (EventNames.ON_SWITCH_NEEDLE);
		EventBroadcaster.Instance.RemoveObserver (EventNames.ON_SWITCH_HAMMER);
	}


	public void SwitchToNeedle() {
		if (!this.isPlayingCoroutine) {
			this.isPlayingCoroutine = true;
			StartCoroutine (NeedleEnter());
		}
	}

	public void SwitchToHammer() {
		if (!this.isPlayingCoroutine) {
			this.isPlayingCoroutine = true;
			if(gameObject.activeInHierarchy)
				StartCoroutine (HammerEnter ());
		}
	}

	private IEnumerator NeedleEnter() {
		hammerAnimatable.Close ();

		Debug.Log ("WAITING FOR HAMMER ANIMATABLE");
//		while (hammerAnimatable.IsPlaying ()) {
//			yield return null;
//		}

		Debug.Log ("EXIT HAMMER ANIMATABLE");
		needleAnimatable.Open ();

		this.isPlayingCoroutine = false;
		yield return null;
	}

	private IEnumerator HammerEnter() {
		if (needleAnimatable != null) {
			needleAnimatable.Close ();
		}

		Debug.Log ("WAITING FOR NEEDLE ANIMATABLE");
//		while (needleAnimatable.IsPlaying ()) {
//			yield return null;
//		}

		Debug.Log ("EXIT NEEDLE ANIMATABLE");
		if (hammerAnimatable != null) {
			hammerAnimatable.Open ();
		}

		this.isPlayingCoroutine = false;
		yield return null;
	}
}
