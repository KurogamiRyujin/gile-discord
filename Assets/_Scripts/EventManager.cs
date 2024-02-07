using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using System;

public class EventManager {
//	private static EventManager sharedInstance = null;

	public delegate void DisplayHint(GameObject collided);
	public static event DisplayHint DisplayHintMethods;

	public delegate void RemoveHint();
	public static event RemoveHint RemoveHintMethods;

	public delegate void DisableInteract();
	public event DisableInteract DisableInteractMethods;

	public delegate void DisableJump();
	public static event DisableJump DisableJumpMethods;

	public delegate void ToggleSwitch(bool flag);
	public static event ToggleSwitch ToggleSwitchMethods;

	public delegate void EnableButtons();
	public static event EnableButtons EnableButtonsMethods;

	public delegate void DisplaySceneTitle();
	public static event DisplaySceneTitle DisplaySceneTitleMethods;

	public delegate void RemoveSceneTitle();
	public static event RemoveSceneTitle RemoveSceneTitleMethods;

	public delegate void ResetSceneTitleTrigger();
	public static event ResetSceneTitleTrigger ResetSceneTitleTriggerMethods;

	public delegate void SetTriggerClick();
	public static event SetTriggerClick SetTriggerClickMethods;

	public delegate void SetTriggerDown();
	public static event SetTriggerDown SetTriggerDownMethods;

	public delegate void ToggleMobileUI(bool flag);
	public event ToggleMobileUI ToggleMobileUIMethods;

	public delegate void ToggleLeftRight(bool flag);
	public event ToggleLeftRight ToggleLeftRightMethods;

//	public static EventManager Instance {
//		get {
//			if (sharedInstance == null) {
//				sharedInstance = new EventManager ();
//			}
//
//			return sharedInstance;
//		}
//	}

//	void Awake() {
//		General.DontDestroyChildOnLoad (this.gameObject);
//	}

	// Use this for initialization
//	void Start () {
//		
//	}

	// Update is called once per frame
//	void Update () {
//
//	}

	public static void DisplayHintOnTrigger(GameObject collided) {
		DisplayHintMethods (collided);
	}

	public static void RemoveHintOnTrigger() {
		RemoveHintMethods ();
	}

	public void DisableInteractButton() {
		DisableInteractMethods ();
	}

	public static void DisableJumpButton() {
		DisableJumpMethods ();
	}

	public static void EnableBothButtons() {
		EnableButtonsMethods ();
	}

	public static void ToggleSwitchButton(bool flag) {
		ToggleSwitchMethods (flag);
	}

	public static void DisplaySceneTitleOnTrigger() {
		DisplaySceneTitleMethods ();
	}

	public static void RemoveSceneTitleOnTrigger() {
		RemoveSceneTitleMethods ();
	}

	public static void ResetTriggerSceneTitle() {
//		ResetSceneTitleTriggerMethods (); // TODO Add scene title triggers
	}

	public static void SetTriggerToClick() {
		SetTriggerClickMethods ();
	}

	public static void SetTriggerToDown() {
		SetTriggerDownMethods ();
	}

	public void ToggleMobileControls(bool flag) {
		if(ToggleMobileUIMethods != null)
			ToggleMobileUIMethods (flag);
	}

	public void ToggleLeftRightButtons(bool flag) {
		if(ToggleLeftRightMethods != null)
			ToggleLeftRightMethods (flag);
	}

}
