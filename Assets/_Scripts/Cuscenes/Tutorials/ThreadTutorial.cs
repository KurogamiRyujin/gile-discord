using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// Cutscene for the thread tutorial.
/// </summary>
public class ThreadTutorial : Cutscene {

	[SerializeField] private Dialogue dialogues;
	[SerializeField] private PickupObject pickupThread;

	private GameObject player;

	private PlayerMovement playerController;
	private PlayerAttack playerAttack;

	void Start() {
		Init ();
	}

	public override void PlayScenes() {
		if (!isPlaying && !isTriggered) {
			base.PlayScenes();
			disablePlayerControls ();
			StartCoroutine (ThreadSpotted ());
		}
	}

	private IEnumerator ThreadSpotted () {
		enableDialogue ();//A spool?
		while (GameController_v7.Instance.GetDialogueManager ().IsDialogueBoxPlaying ()) {
			disablePlayerControls ();
			yield return null;
		}

		EnablePlayerControls ();

		while (!pickupThread.IsTaken())
			yield return null;

		enableDialogue ();//show something cool
		while (GameController_v7.Instance.GetDialogueManager ().IsDialogueBoxPlaying ()) {
			disablePlayerControls ();
			yield return null;
		}

		EnablePlayerControls ();
	}

	void enableDialogue() {
		playerController.isInDialogue ();
		GameController_v7.Instance.GetDialogueManager ().DisplayMessage (DialogueManager_v2.DialogueType.DIALOGUE, dialogues, null);
	}

	protected override void disablePlayerControls () {
//		ToggleMobileUI (false);

		GameController_v7.Instance.GetMobileUIManager ().ToggleMobileControls (false);
		playerController.canMove (false);
		playerController.isInDialogue ();
		playerAttack.canAttack (false);
	}

	private void EnablePlayerControls() {
//		ToggleMobileUI (true);
		GameController_v7.Instance.GetMobileUIManager ().ToggleMobileControls (true);

		playerController.canMove (true);
		playerAttack.canAttack (true);
	}

//	private void ToggleMobileUI(bool flag) {
//		#if UNITY_ANDROID
////		EventManager.Instance.ToggleMobileControls(flag);
//
////		GameController_v7.Instance.GetEventManager().ToggleMobileControls(flag);
//		Parameters parameters = new Parameters ();
//		parameters.PutExtra ("FLAG", flag);
//		EventBroadcaster.Instance.PostEvent (EventNames.TOGGLE_MOBILE_UI, parameters);
//		#endif
//	}

	void OnTriggerEnter2D(Collider2D other) {
		Debug.Log ("Tutorial Trigger : Tag is "+other.tag);
		if (other.gameObject.GetComponent<PlayerYuni>() != null) {
			player = other.gameObject;
			PlayerYuni playerStats = player.GetComponent<PlayerYuni> ();
			playerAttack = playerStats.GetPlayerAttack ();
			playerController = playerStats.GetPlayerMovement ();
			PlayScenes ();
		}
	}
}
