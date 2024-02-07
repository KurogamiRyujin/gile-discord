using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FirstTimeFallTrigger : Cutscene {

	[SerializeField] private DialogueTrigger dialogueTrigger;
	[SerializeField] private DialogueManager dialogueManager;

	[SerializeField] private PlayerMovement playerController;
	[SerializeField] private PlayerAttack playerAttack;

	// Use this for initialization
	void Start () {
		Init ();
	}

	protected override void Init () {

		base.Init ();
	}

	public override void PlayScenes () {
		if (!this.isTriggered && !this.isPlaying) {
			base.PlayScenes ();

			StartCoroutine (FirstTimeFall ());
		}
	}

	private IEnumerator FirstTimeFall() {
		Debug.Log ("Triggered First Time Fall.");

		EnableDialogue ();
		disablePlayerControls ();
		this.dialogueManager = dialogueTrigger.GetDialogueManager ();
		while (dialogueManager.isPlaying) {
			yield return null;
		}

		EnablePlayerControls ();

		Debug.Log ("First Time Fall ended.");
	}

	protected override void disablePlayerControls () {
		playerController.isInDialogue ();
		playerAttack.canAttack (false);
		playerController.canMove (false);
	}

	private void EnablePlayerControls() {
		ToggleMobileUI (true);
		playerAttack.canAttack (true);
		playerController.canMove (true);
	}

	private void EnableDialogue () {
		dialogueTrigger.TriggerDialogue ();
	}
	// TODO Mobile : Consider making static reference. MovementTutorialCutscene also references it
	void ToggleMobileUI(bool flag) {
		#if UNITY_ANDROID
//		EventManager.Instance.ToggleMobileControls(flag);

//		GameController_v7.Instance.GetEventManager().ToggleMobileControls(flag);
		Parameters parameters = new Parameters ();
		parameters.PutExtra ("FLAG", flag);
		EventBroadcaster.Instance.PostEvent (EventNames.TOGGLE_MOBILE_UI, parameters);
		#endif
	}

	void OnTriggerEnter2D(Collider2D other) {
		if (other.gameObject.CompareTag ("Player")) {
			playerController = other.gameObject.GetComponent<PlayerMovement> ();
			playerAttack = other.gameObject.GetComponent<PlayerAttack> ();

			PlayScenes ();
			this.gameObject.GetComponent<Collider2D> ().enabled = false;
		}
	}
}
