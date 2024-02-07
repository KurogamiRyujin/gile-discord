using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ToggleButton : MonoBehaviour {

//	GameObject btnJump;
//	GameObject btnInteract;
//	GameObject btnSwitch;
//	GameObject btnLeft1, btnLeft2;
//	GameObject btnRight1, btnRight2;
//
//	// Use this for initialization
//	void Awake () {
//		btnJump = GameObject.Find ("Jump");
//		btnInteract = GameObject.Find ("Interact");
//		btnSwitch = GameObject.Find ("SwitchWeapon");
//		btnLeft1 = GameObject.Find ("Left");
//		btnLeft2 = GameObject.Find ("Left (1)");
//		btnRight1 = GameObject.Find ("Right");
//		btnRight2 = GameObject.Find ("Right (1)");
////		btnJump = gameObject.transform.Find ("Jump").gameObject;
////		btnInteract = gameObject.transform.Find ("Interact").gameObject;
////		btnSwitch = gameObject.transform.Find ("SwitchWeapon").gameObject;
//		btnLeft2.SetActive(false);
//		btnRight2.SetActive (false);
//		btnInteract.SetActive (false);
//
////		EventManager.DisableInteractMethods += DisableInteract;
////		GameController_v7.Instance.GetEventManager().DisableInteractMethods += DisableInteract;
////		EventManager.DisableJumpMethods += DisableJump;
////		EventManager.EnableButtonsMethods += EnableButtons;
////		EventManager.ToggleSwitchMethods += ToggleSwitch;
////		EventManager.Instance.ToggleLeftRightMethods += ToggleLeftRight;
////		GameController_v7.Instance.GetEventManager().ToggleLeftRightMethods += ToggleLeftRight;
//
//		EventBroadcaster.Instance.AddObserver (EventNames.DISABLE_INTERACT, DisableInteract);
//		EventBroadcaster.Instance.AddObserver (EventNames.DISABLE_JUMP, DisableJump);
//		EventBroadcaster.Instance.AddObserver (EventNames.ENABLE_BUTTONS, EnableButtons);
//		EventBroadcaster.Instance.AddObserver (EventNames.TOGGLE_SWITCH_WEAPON_BUTTON, ToggleSwitch);
//		EventBroadcaster.Instance.AddObserver (EventNames.TOGGLE_LEFT_RIGHT_BUTTONS, ToggleLeftRight);
//
//	}
//
//	void OnDestroy() {
////		EventManager.DisableInteractMethods -= DisableInteract;
////		GameController_v7.Instance.GetEventManager().DisableInteractMethods -= DisableInteract;
////		EventManager.DisableJumpMethods -= DisableJump;
////		EventManager.EnableButtonsMethods -= EnableButtons;
////		EventManager.ToggleSwitchMethods -= ToggleSwitch;
//////		EventManager.Instance.ToggleLeftRightMethods -= ToggleLeftRight;
////		GameController_v7.Instance.GetEventManager().ToggleLeftRightMethods -= ToggleLeftRight;
//
//		EventBroadcaster.Instance.RemoveObserver (EventNames.DISABLE_INTERACT);
//		EventBroadcaster.Instance.RemoveObserver (EventNames.DISABLE_JUMP);
//		EventBroadcaster.Instance.RemoveObserver (EventNames.ENABLE_BUTTONS);
//		EventBroadcaster.Instance.RemoveObserver (EventNames.TOGGLE_SWITCH_WEAPON_BUTTON);
//		EventBroadcaster.Instance.RemoveObserver (EventNames.TOGGLE_LEFT_RIGHT_BUTTONS);
//
//	}
//	
//	// Update is called once per frame
//	void Update () {
//		
//	}
//
//	void DisableJump() {
//		gameObject.GetComponent<MobileUI> ().JumpButton_Up ();
//		btnJump.SetActive (false);
//		btnInteract.SetActive (true);
//	}
//
//	void DisableInteract() {
//		gameObject.GetComponent<MobileUI> ().InteractButton_Up ();
//		btnInteract.SetActive (false);
//		btnJump.SetActive (true);
//	}
//
////	void ToggleSwitch(bool flag) {
////		btnSwitch.SetActive (flag);
////	}
//
//	void ToggleSwitch(Parameters parameters) {
//		bool flag = parameters.GetBoolExtra ("FLAG", false);
//		btnSwitch.SetActive (flag);
//	}
//
////	void ToggleLeftRight(bool flag) {
////		btnLeft1.SetActive (flag);
////		btnRight1.SetActive (flag);
////		btnLeft2.SetActive (!flag);
////		btnRight2.SetActive (!flag);
////	}
//
//	void ToggleLeftRight(Parameters parameters) {
//		bool flag = parameters.GetBoolExtra ("FLAG", false);
//		btnLeft1.SetActive (flag);
//		btnRight1.SetActive (flag);
//		btnLeft2.SetActive (!flag);
//		btnRight2.SetActive (!flag);
//	}
//
//	void EnableButtons() {
//		btnInteract.SetActive (true);
//		btnJump.SetActive (true);
//	}

}
