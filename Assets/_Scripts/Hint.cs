using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public class Hint : MonoBehaviour {
	public static int DOOR_HINT_INDEX = 0;
	public static int PICKUP_HINT_INDEX = 1;

	float waitingTime;
	DialogueManager dialogueManager;
	DialogueTrigger dialogueTrigger;

	Coroutine co;

	// Use this for initialization
	void Start () {
		waitingTime = 20.0f;
//		dialogueTrigger = GetComponent<DialogueTrigger> ();
//		dialogueManager = GameObject.Find ("DialogueManager").GetComponent<DialogueManager> ();
		// EventManager.DisplayHintMethods += ShowHint;
		// EventManager.RemoveHintMethods += HideHint;
	}

	void Awake() {
		EventBroadcaster.Instance.AddObserver (EventNames.DISPLAY_HINT, ShowHint);
		EventBroadcaster.Instance.AddObserver (EventNames.REMOVE_HINT, HideHint);
	}

	void OnDestroy() {
		// EventManager.DisplayHintMethods -= ShowHint;
		// EventManager.RemoveHintMethods -= HideHint;
		EventBroadcaster.Instance.RemoveObserver(EventNames.DISPLAY_HINT);
		EventBroadcaster.Instance.RemoveObserver (EventNames.REMOVE_HINT);
	}

	// Update is called once per frame
	void Update () {

	}

	public IEnumerator StartTimer(int index) {
		Debug.Log ("Timer started");
		DateTime timecurr = System.DateTime.Now;
		Debug.Log (timecurr);
		yield return new WaitForSeconds (waitingTime);
		Debug.Log ("Timer ended");
		dialogueTrigger.dialogue.currentTextIndex = index - 1;
		dialogueTrigger.TriggerDialogue ();
		while (dialogueManager.isPlaying) {
			ToggleMobileUI (false);
			yield return null;
		}

		ToggleMobileUI (true);
		yield break;
	}

	void ToggleMobileUI(bool flag) {
		#if UNITY_ANDROID
//		EventManager.Instance.ToggleMobileControls(flag);

//		GameController_v7.Instance.GetEventManager().ToggleMobileControls(flag);

		Parameters parameters = new Parameters ();
		parameters.PutExtra ("FLAG", flag);
		EventBroadcaster.Instance.PostEvent (EventNames.TOGGLE_MOBILE_UI, parameters);
		#endif
	}

//	void ShowHint(GameObject collided) {
//		if(collided.layer == LayerMask.NameToLayer("Door Hint")) {
//			Debug.Log ("I can go inside this thing");
//			co = StartCoroutine (StartTimer (DOOR_HINT_INDEX));
//		}
//		else if(collided.layer == LayerMask.NameToLayer("Pickup Hint")) {
//			Debug.Log ("I can pick this up");
//			co = StartCoroutine (StartTimer (PICKUP_HINT_INDEX));
//		}
//	}

	void ShowHint(Parameters parameters) {
		int collidedLayer = parameters.GetIntExtra ("LAYER_VALUE", 0);
		if(collidedLayer == LayerMask.NameToLayer("Door Hint")) {
			Debug.Log ("I can go inside this thing");
			co = StartCoroutine (StartTimer (DOOR_HINT_INDEX));
		}
		else if(collidedLayer == LayerMask.NameToLayer("Pickup Hint")) {
			Debug.Log ("I can pick this up");
			co = StartCoroutine (StartTimer (PICKUP_HINT_INDEX));
		}
	}

	void HideHint(){
		if (co != null) {
			StopCoroutine (co);
			Debug.Log ("Coroutine for hint stopped");
		}
	}
}
