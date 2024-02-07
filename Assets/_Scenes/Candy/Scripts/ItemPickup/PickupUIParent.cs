using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PickupUIParent : MonoBehaviour {
	[SerializeField] private NeedlePickupUI needlePickup;
	[SerializeField] private HammerPickupUI hammerPickup;
	[SerializeField] private ThreadPickupUI threadPickup;
	[SerializeField] private CharmPickupUI charmPickup;



	void Awake() {
		EventBroadcaster.Instance.AddObserver (EventNames.NEEDLE_PICKUP, OpenNeedlePickup);
		EventBroadcaster.Instance.AddObserver (EventNames.THREAD_PICKUP, OpenThreadPickup);
		EventBroadcaster.Instance.AddObserver (EventNames.HAMMER_PICKUP, OpenHammerPickup);
		EventBroadcaster.Instance.AddObserver (EventNames.CHARM_PICKUP, OpenCharmPickup);
		this.Close ();
	}

	void OnDestroy() {
		EventBroadcaster.Instance.RemoveObserver (EventNames.NEEDLE_PICKUP);
		EventBroadcaster.Instance.RemoveObserver (EventNames.THREAD_PICKUP);
		EventBroadcaster.Instance.RemoveObserver (EventNames.HAMMER_PICKUP);
		EventBroadcaster.Instance.RemoveObserver (EventNames.CHARM_PICKUP);
	}
	public bool IsOpen() {
		if (gameObject.activeInHierarchy) {
			return true;
		}
		return false;
	}
	public void OpenNeedlePickup() {
		this.Open ();
		this.CloseAll ();
		if (this.GetNeedlePickup () != null) {
			this.GetNeedlePickup ().Show ();
		}
	}

	public void OpenHammerPickup() {
		this.Open ();
		this.CloseAll ();
		if (this.GetHammerPickup () != null) {
			this.GetHammerPickup ().Show ();
		}
	}

	public void OpenThreadPickup() {
		this.Open ();
		this.CloseAll ();
		if (this.GetThreadPickup () != null) {
			this.GetThreadPickup ().Show ();
		}
	}
	public void OpenCharmPickup() {
		this.Open ();
		this.CloseAll ();
		if (this.GetCharmPickup () != null) {
			this.GetCharmPickup ().Show ();
		}
	}
	public void Open() {
		gameObject.SetActive (true);
	}

	public void ButtonClose() {
		SoundManager.Instance.Play (AudibleNames.Button.BASIC, false);
		this.Close ();
		EventBroadcaster.Instance.PostEvent (EventNames.PICKUP_ITEM_CLOSE);
	}

	public void Close() {
		gameObject.SetActive (false);
	}


	// Call this before opening to ensure no other screen is open
	public void CloseAll() {
		if (this.GetNeedlePickup () != null) {
			this.GetNeedlePickup ().Close ();
		}

		if (this.GetHammerPickup () != null) {
			this.GetHammerPickup ().Close ();
		}

		if (this.GetThreadPickup () != null) {
			this.GetThreadPickup ().Close ();
		}

		if (this.GetCharmPickup () != null) {
			this.GetCharmPickup ().Close ();
		}
	}


	public NeedlePickupUI GetNeedlePickup() {
		if (this.needlePickup == null) {
			this.needlePickup = GetComponentInChildren<NeedlePickupUI> ();
		}
		return this.needlePickup;
	}

	public HammerPickupUI GetHammerPickup() {
		if (this.hammerPickup == null) {
			this.hammerPickup = GetComponentInChildren<HammerPickupUI> ();
		}
		return this.hammerPickup;
	}

	public ThreadPickupUI GetThreadPickup() {
		if (this.threadPickup == null) {
			this.threadPickup = GetComponentInChildren<ThreadPickupUI> ();
		}
		return this.threadPickup;
	}

	public CharmPickupUI GetCharmPickup() {
		if (this.charmPickup == null) {
			this.charmPickup = GetComponentInChildren<CharmPickupUI> ();
		}
		return this.charmPickup;
	}
}
