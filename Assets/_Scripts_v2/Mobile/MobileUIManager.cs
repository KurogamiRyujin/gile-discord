using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MobileUIManager {

	private MobileUI mobileUIReference;
	private MenuController menuController;

	public void SubscribeMobileUI(MobileUI mobileUI) {
		this.mobileUIReference = mobileUI;
	}

	public void UnsubscribeMobileUI(MobileUI mobileUI) {
		Debug.LogError ("Unsubscribed Mobile UI");
		//only the interface that subscribed can unsubscribe
		if (mobileUI == this.mobileUIReference)
			mobileUIReference = null;
	}


	public void SubscribeMenuController(MenuController menu) {
		this.menuController = menu;
	}

	public void UnubscribeMenuController(MenuController menu) {
		//only the interface that subscribed can unsubscribe
		if (menu == this.menuController)
			menuController = null;
	}
	public void HideMenu() {
		this.menuController.Hide ();
	}

	// Shows left, right, and jump only
//	public void ToggleBaseControls(bool flag) {
//		#if UNITY_ANDROID
//		this.ToggleMobileControls(true); // Show mobile parent
//		// Then show proper buttons
//		Parameters parameters = new Parameters ();
//		parameters.PutExtra ("FLAG", flag);
//		EventBroadcaster.Instance.PostEvent (EventNames.TOGGLE_LEFT_RIGHT_BUTTONS, parameters);
//		EventBroadcaster.Instance.PostEvent (EventNames.TOGGLE_JUMP, parameters);
//		EventBroadcaster.Instance.PostEvent (EventNames.TOGGLE_INTERACT, parameters);
//
//		parameters = new Parameters ();
//		parameters.PutExtra ("FLAG", !flag);
//		EventBroadcaster.Instance.PostEvent (EventNames.TOGGLE_SWITCH_WEAPON_BUTTON, parameters);
//		EventBroadcaster.Instance.PostEvent (EventNames.TOGGLE_PICKUP_BUTTON, parameters);
//
//		#endif
//	}

	public void ToggleBaseWithPickupControls(bool flag) {
		#if UNITY_ANDROID
		this.ToggleMobileControls(true); // Show mobile parent
		// Then show proper buttons
		Parameters parameters = new Parameters ();
		parameters.PutExtra ("FLAG", flag);
		EventBroadcaster.Instance.PostEvent (EventNames.TOGGLE_LEFT_RIGHT_BUTTONS, parameters);
		EventBroadcaster.Instance.PostEvent (EventNames.TOGGLE_JUMP, parameters);
		EventBroadcaster.Instance.PostEvent (EventNames.TOGGLE_INTERACT, parameters);

		EventBroadcaster.Instance.PostEvent (EventNames.TOGGLE_PICKUP_BUTTON, parameters);
//		parameters = new Parameters ();
//		parameters.PutExtra ("FLAG", !flag);
		EventBroadcaster.Instance.PostEvent (EventNames.TOGGLE_SWITCH_WEAPON_BUTTON, parameters);

		#endif
	}

	// Shows all mobile buttons
	public void ToggleMobileControls(bool flag) {
		#if UNITY_ANDROID
		Parameters parameters = new Parameters ();
		parameters.PutExtra ("FLAG", flag);
		EventBroadcaster.Instance.PostEvent (EventNames.TOGGLE_MOBILE_UI, parameters);

		EventBroadcaster.Instance.PostEvent (EventNames.TOGGLE_LEFT_RIGHT_BUTTONS, parameters);
		EventBroadcaster.Instance.PostEvent (EventNames.TOGGLE_JUMP, parameters);
		EventBroadcaster.Instance.PostEvent (EventNames.TOGGLE_INTERACT, parameters);
		EventBroadcaster.Instance.PostEvent (EventNames.TOGGLE_SWITCH_WEAPON_BUTTON, parameters);
		EventBroadcaster.Instance.PostEvent (EventNames.TOGGLE_PICKUP_BUTTON, parameters);

//		if(!flag) {
		GameController_v7.Instance.GetMobileUIManager ().SetLeftPressed (false);
		GameController_v7.Instance.GetMobileUIManager ().SetRightPressed (false);
//		}
		#endif
	}

	public void ToggleSwitchWeaponButton(bool flag) {
		#if UNITY_ANDROID
		Parameters parameters = new Parameters ();
		parameters.PutExtra ("FLAG", flag);
		EventBroadcaster.Instance.PostEvent (EventNames.TOGGLE_SWITCH_WEAPON_BUTTON, parameters);
		#endif
	}

	public void TogglePickupButton(bool flag) {
		#if UNITY_ANDROID
		Parameters parameters = new Parameters ();
		parameters.PutExtra ("FLAG", flag);
		EventBroadcaster.Instance.PostEvent (EventNames.TOGGLE_PICKUP_BUTTON, parameters);
		#endif
	}
	public MobileUI GetMobileUI() {
		return this.mobileUIReference;
	}
	public float GetInputValue() {
		return this.mobileUIReference.GetInputValue ();
	}

	public bool IsJumping() {
		return this.mobileUIReference.IsJumpPressed ();
	}
	public bool IsInteracting() {
		return this.mobileUIReference.IsInteractPressed ();
	}
	public bool IsCharging() {
		return this.mobileUIReference.IsChargePressed ();
	}
	public bool IsMenu() {
		return this.mobileUIReference.IsMenuPressed ();
	}

	public bool IsMenuSubButton() {
		return this.menuController.IsPlayPressed ();
	}

	public bool IsHelping() {
		return this.mobileUIReference.IsHelpPressed ();
	}
	// Note: Must only be called ONCE (PlayerAttack called dibs)
	public void ReleaseChargeButton() {
		Debug.Log ("ON CHARGING CALL");
		EventBroadcaster.Instance.PostEvent (EventNames.ON_CHARGING);
		this.mobileUIReference.Charge_Up ();
	}

	public bool IsRightPressed() {
		return this.mobileUIReference.IsRightPressed ();
	}
	public bool IsLeftPressed() {
		return this.mobileUIReference.IsLeftPressed ();
	}
	public bool IsSwitchPressed() {
		return this.mobileUIReference.IsSwitchPressed ();
	}
	public void SetSwitch(bool value) {
		this.mobileUIReference.switchPressed = false;
	}
	public void SetRightPressed(bool value) {
		this.mobileUIReference.SetRightPressed (value);
	}
	public void SetLeftPressed(bool value) {
		this.mobileUIReference.SetLeftPressed (value);
	}

	public void SetJumping(bool value) {
		this.mobileUIReference.SetJumping (value);
	}
	public void SetInteracting(bool value) {
		this.mobileUIReference.SetInteracting (value);
	}
}
