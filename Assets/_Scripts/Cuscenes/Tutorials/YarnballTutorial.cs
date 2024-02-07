using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class YarnballTutorial : Cutscene {

	[SerializeField] private Dialogue dialogues;

	[SerializeField] private YarnballPedestal yarnballPedestal;

	private GameObject player;

	private PlayerMovement playerController;
	private PlayerAttack playerAttack;

	void Start() {
		Init ();
	}

	protected override void Init () {
		base.Init ();
	}

	public override void PlayScenes() {
		if (!isPlaying && !isTriggered) {
			base.PlayScenes();
			disablePlayerControls ();
			StartCoroutine (YarnballSpotted ());
		}
	}

	private IEnumerator YarnballSpotted() {
		enableDialogue ();//spotted yarnball
		while (GameController_v7.Instance.GetDialogueManager ().IsDialogueBoxPlaying ()) {
			disablePlayerControls ();
			yield return null;
		}

		//Press "S" to pickup
		GameController_v7.Instance.GetDialogueManager ().DisplayMessage (DialogueManager_v2.DialogueType.HINT, dialogues, null);

		EnablePlayerControls ();

		while (playerAttack.getEquippedDenominator() != yarnballPedestal.yarnball.GetDenominator())
			yield return null;

		enableDialogue ();//Now thread is similar with object
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
		ToggleMobileUI (false);
		playerController.canMove (false);
		playerAttack.canAttack (false);
	}

	private void EnablePlayerControls() {
		ToggleMobileUI (true);
		playerController.canMove (true);
		playerAttack.canAttack (true);
	}

	private void ToggleMobileUI(bool flag) {
		#if UNITY_ANDROID
//		EventManager.Instance.ToggleMobileControls(flag);

//		GameController_v7.Instance.GetEventManager().ToggleMobileControls(flag);
		Parameters parameters = new Parameters ();
		parameters.PutExtra ("FLAG", flag);
		EventBroadcaster.Instance.PostEvent (EventNames.TOGGLE_MOBILE_UI, parameters);
		#endif
	}

	void OnTriggerEnter2D(Collider2D other) {
		Debug.Log ("Tutorial Trigger : Tag is "+other.tag);
		if (other.gameObject.CompareTag ("Player")) {
			player = other.gameObject;
			PlayerYuni playerStats = player.GetComponent<PlayerYuni> ();
			playerAttack = playerStats.GetPlayerAttack ();
			playerController = playerStats.GetPlayerMovement ();
			PlayScenes ();
		}
	}
}
