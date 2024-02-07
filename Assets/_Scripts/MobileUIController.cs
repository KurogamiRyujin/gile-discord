using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// Controller to handle mobile UI
/// </summary>
public class MobileUIController : MonoBehaviour {

    //	[SerializeField] GameObject mobile;
    /// <summary>
    /// Reference to the left button
    /// </summary>
    [SerializeField] GameObject btnLeft1;//, btnLeft2;

    /// <summary>
    /// Reference to the right button
    /// </summary>
    [SerializeField] GameObject btnRight1;//, btnRight2;

    /// <summary>
    /// Reference to the jump button
    /// </summary>
    [SerializeField] GameObject btnJump;

    /// <summary>
    /// Reference to the interact button
    /// </summary>
	[SerializeField] GameObject btnInteract;

    /// <summary>
    /// Reference to the switch weapon button
    /// </summary>
	[SerializeField] GameObject btnSwitch;

    /// <summary>
    /// Reference to the pickup button
    /// </summary>
	[SerializeField] GameObject btnPickup;

    /// <summary>
    /// Reference to the player
    /// </summary>
	[SerializeField] PlayerYuni player;

    /// <summary>
    /// Flag for automatically acquiring the hammer
    /// </summary>
    [SerializeField] bool isForceAcquire;

    // Use this for initialization
    /// <summary>
    /// Unity Function. Called once upon creation of the object 
    /// </summary>
    void Awake () {
//		btnJump = GameObject.Find ("Jump");
//		btnInteract = GameObject.Find ("Interact");
//		btnSwitch = GameObject.Find ("SwitchWeapon");
//		btnLeft1 = GameObject.Find ("Left");
//		btnLeft2 = GameObject.Find ("Left (1)");
//		btnRight1 = GameObject.Find ("Right");
//		btnRight2 = GameObject.Find ("Right (1)");

		//		btnJump = gameObject.transform.Find ("Jump").gameObject;
		//		btnInteract = gameObject.transform.Find ("Interact").gameObject;
		//		btnSwitch = gameObject.transform.Find ("SwitchWeapon").gameObject;
//		btnLeft2.SetActive(false);
//		btnRight2.SetActive (false);
		btnInteract.SetActive (false);
        this.isForceAcquire = false;
//		mobile = GameObject.Find ("Mobile UI");
//		EventManager.Instance.ToggleMobileUIMethods += Toggle;

//		GameController_v7.Instance.GetMobileUIManager().GetMobileUI().ge += Toggle;
		EventBroadcaster.Instance.AddObserver(EventNames.TOGGLE_MOBILE_UI, ToggleMobileUI);
		EventBroadcaster.Instance.AddObserver(EventNames.TOGGLE_BASE_MOBILE_UI, ToggleBaseMobileUI);
		EventBroadcaster.Instance.AddObserver (EventNames.TOGGLE_INTERACT, ToggleInteract);
		EventBroadcaster.Instance.AddObserver (EventNames.TOGGLE_PICKUP_BUTTON, TogglePickup);
		EventBroadcaster.Instance.AddObserver (EventNames.TOGGLE_JUMP, ToggleJump);
		EventBroadcaster.Instance.AddObserver (EventNames.TOGGLE_SWITCH_WEAPON_BUTTON, ToggleSwitch);
		EventBroadcaster.Instance.AddObserver (EventNames.TOGGLE_LEFT_RIGHT_BUTTONS, ToggleLeftRight);
        EventBroadcaster.Instance.AddObserver(EventNames.HAMMER_FORCE_ACQUIRE, HammerForceAcquire);



}
    /// <summary>
    /// Unity Function. Raises the destroy event
    /// </summary>
	void OnDestroy() {
//		EventManager.Instance.ToggleMobileUIMethods -= Toggle;

//		GameController_v7.Instance.GetEventManager ().ToggleMobileUIMethods -= Toggle;
		//EventBroadcaster.Instance.RemoveActionAtObserver(EventNames.TOGGLE_MOBILE_UI);
		//EventBroadcaster.Instance.RemoveActionAtObserver(EventNames.TOGGLE_INTERACT);
		//EventBroadcaster.Instance.RemoveActionAtObserver(EventNames.TOGGLE_JUMP);
		//EventBroadcaster.Instance.RemoveActionAtObserver(EventNames.TOGGLE_SWITCH_WEAPON_BUTTON);
		//EventBroadcaster.Instance.RemoveActionAtObserver(EventNames.TOGGLE_LEFT_RIGHT_BUTTONS);
		//EventBroadcaster.Instance.RemoveActionAtObserver(EventNames.TOGGLE_BASE_MOBILE_UI);
		//EventBroadcaster.Instance.RemoveActionAtObserver(EventNames.TOGGLE_PICKUP_BUTTON);


        EventBroadcaster.Instance.RemoveActionAtObserver(EventNames.TOGGLE_MOBILE_UI, ToggleMobileUI);
        EventBroadcaster.Instance.RemoveActionAtObserver(EventNames.TOGGLE_BASE_MOBILE_UI, ToggleBaseMobileUI);
        EventBroadcaster.Instance.RemoveActionAtObserver(EventNames.TOGGLE_INTERACT, ToggleInteract);
        EventBroadcaster.Instance.RemoveActionAtObserver(EventNames.TOGGLE_PICKUP_BUTTON, TogglePickup);
        EventBroadcaster.Instance.RemoveActionAtObserver(EventNames.TOGGLE_JUMP, ToggleJump);
        EventBroadcaster.Instance.RemoveActionAtObserver(EventNames.TOGGLE_SWITCH_WEAPON_BUTTON, ToggleSwitch);
        EventBroadcaster.Instance.RemoveActionAtObserver(EventNames.TOGGLE_LEFT_RIGHT_BUTTONS, ToggleLeftRight);
        EventBroadcaster.Instance.RemoveActionAtObserver(EventNames.HAMMER_FORCE_ACQUIRE, HammerForceAcquire);
    }

    /// <summary>
    /// Acquire hammer
    /// </summary>
    public void HammerForceAcquire() {
        this.isForceAcquire = true;
    }

    /// <summary>
    /// Toggles the pickup button
    /// </summary>
    /// <param name="parameters">Parameters passed by posted event</param>
    void TogglePickup(Parameters parameters) {
		bool flag = parameters.GetBoolExtra ("FLAG", false);
		btnPickup.SetActive (flag);
	}

    /// <summary>
    /// Toggles the jump button
    /// </summary>
    /// <param name="parameters">Parameters passed by posted event</param>
	void ToggleJump(Parameters parameters) {
		bool flag = parameters.GetBoolExtra ("FLAG", false);
		if (!flag)
			gameObject.GetComponent<MobileUI> ().JumpButton_Up ();
		btnJump.SetActive (flag);
	}

    /// <summary>
    /// Toggles the interact button
    /// </summary>
    /// <param name="parameters">Parameters passed by posted event</param>
	void ToggleInteract(Parameters parameters) {
		bool flag = parameters.GetBoolExtra ("FLAG", false);
		if (!flag)
			gameObject.GetComponent<MobileUI> ().InteractButton_Up ();
		btnInteract.SetActive (flag);
	}

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

    //	void ToggleSwitch(bool flag) {
    //		btnSwitch.SetActive (flag);
    //	}

    /// <summary>
    /// Toggles the switch weapon button
    /// </summary>
    /// <param name="parameters">Parameters passed by posted event</param>
    void ToggleSwitch(Parameters parameters) {
		bool flag = parameters.GetBoolExtra ("FLAG", false);
		if (isForceAcquire || (flag && GetPlayer ().GetPlayerAttack ().HasHammer ())) {
			btnSwitch.SetActive (flag);
		} else {
			btnSwitch.SetActive (false);
		}
	}

    //	void ToggleLeftRight(bool flag) {
    //		btnLeft1.SetActive (flag);
    //		btnRight1.SetActive (flag);
    //		btnLeft2.SetActive (!flag);
    //		btnRight2.SetActive (!flag);
    //	}

    /// <summary>
    /// Toggles the left and right buttons
    /// </summary>
    /// <param name="parameters">Parameters passed by posted event</param>
    void ToggleLeftRight(Parameters parameters) {
		bool flag = parameters.GetBoolExtra ("FLAG", false);
		btnLeft1.SetActive (flag);
		btnRight1.SetActive (flag);

//		btnLeft2.SetActive (!flag);
//		btnRight2.SetActive (!flag);
	}

    /// <summary>
    /// Toggles mobile UI
    /// </summary>
    /// <param name="parameters">Parameters passed by posted event</param>
	void ToggleBaseMobileUI(Parameters parameters) {
//		Debug.Log ("<color=cyan>CALLED TOGGLE UI</color>");
		bool flag = parameters.GetBoolExtra ("FLAG", false);
		gameObject.SetActive (flag);
		//		mobile.SetActive (flag);
	}

    /// <summary>
    /// Toggles mobile UI
    /// </summary>
    /// <param name="parameters">Parameters passed by posted event</param>
	void ToggleMobileUI(Parameters parameters) {
//		Debug.Log ("<color=cyan>CALLED TOGGLE UI</color>");
		bool flag = parameters.GetBoolExtra ("FLAG", false);
		gameObject.SetActive (flag);
//		mobile.SetActive (flag);
	}

    /// <summary>
    /// Gets instance of player
    /// </summary>
    /// <returns>Returns instance of player</returns>
	public PlayerYuni GetPlayer() {
		if (this.player == null) {
			this.player = FindObjectOfType<PlayerYuni> ();
		}
		return this.player;
	}
}
