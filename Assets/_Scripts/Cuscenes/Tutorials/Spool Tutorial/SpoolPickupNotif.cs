using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// Cutscene to introduce the spool.
/// </summary>
public class SpoolPickupNotif : Cutscene {

	[SerializeField] private DialogueManager dialogueManager;
	private DialogueTrigger dialogueTrigger;

	private PickupThread pickupThread;
	private PlayerMovement playerController;
	private PlayerAttack playerAttack;

	// Use this for initialization
	void Start () {
		Init ();
	}

	protected override void Init () {
		dialogueTrigger = GetComponent<DialogueTrigger> ();
		playerController = GameObject.FindObjectOfType<PlayerYuni> ().gameObject.GetComponent<PlayerMovement> ();
		playerAttack = GameObject.FindObjectOfType<PlayerYuni> ().gameObject.GetComponent<PlayerAttack> ();
		pickupThread = GetComponent<PickupThread> ();

		base.Init ();
	}

	void Update() {
		if (pickupThread == null && !this.isTriggered) {
			this.isTriggered = true;
			StartCoroutine (TriggerDialogue ());
		}
	}

	private IEnumerator TriggerDialogue() {
		EnableDialogue ();
		dialogueManager = dialogueTrigger.GetDialogueManager ();
		while (dialogueManager.isPlaying) {
			disablePlayerControls ();
			yield return null;
		}

		Debug.Log ("Enable Controls");
		EnablePlayerControls ();
		this.enabled = false;
	}

	private void EnableDialogue() {
		playerController.isInDialogue ();
		dialogueTrigger.TriggerDialogue ();
	}

	protected override void disablePlayerControls () {
		playerController.canMove (false);
		playerAttack.canAttack (false);

	}

	private void EnablePlayerControls() {
		playerController.canMove (true);
		playerAttack.canAttack (true);
	}
}
