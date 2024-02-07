using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HammerAndEnemyTutorial : Cutscene {

	[SerializeField] private Dialogue dialogues;
	[SerializeField] private string[] invulnerableEnemy;
	[SerializeField] private string[] breakTheBox;
	[SerializeField] private string[] breakTheBoxHint;

	private GameObject player;

	private PlayerMovement playerController;
	private PlayerAttack playerAttack;
	[SerializeField] private PickupObject pickupObject;
	[SerializeField] private EnemyHealth enemy;
	[SerializeField] private HitCheck enemyHit;
	[SerializeField] private EnemySightedTrigger enemySighted;
	[SerializeField] private StabilityNumberLine stabilityNumberLine;
	[SerializeField] private BoxSighted boxSighted;

	public override void PlayScenes() {
		if (!isPlaying && !isTriggered) {
			base.PlayScenes();
			disablePlayerControls ();
			StartCoroutine (HammerSighted ());
		}
	}

	private IEnumerator HammerSighted() {
		playerAttack.SetHasHammer (false);

		enableDialogue ();//Spotted Hammer
		while (GameController_v7.Instance.GetDialogueManager ().IsDialogueBoxPlaying ()) {
			disablePlayerControls ();
			yield return null;
		}

//		ToggleMobileUI (true);
//		GameController_v7.Instance.GetMobileUIManager().ToggleMobileControls(true);
//		playerController.canMove (true);
//		playerAttack.canAttack (true);
		EnablePlayerControls();

		bool interacted = false;

		while (!interacted) {
			//if the needle is still in the world, interacted is false
//			if (!pickupObject.pickup.activeInHierarchy) {

			if (pickupObject.IsTaken()) {
				interacted = true;
				// disable pickup needle hint
//				pickupObject.gameObject.GetComponentInChildren<FloatingButtonHint>().isTangible = false;
			}

			yield return null;
		}
		enableDialogue ();// We might use it on obstacles // Hammer also has sky fragment
		while (GameController_v7.Instance.GetDialogueManager ().IsDialogueBoxPlaying ()) {
			disablePlayerControls ();
			yield return null;
		}
		EnablePlayerControls ();
		//Switch weapons hint
		GameController_v7.Instance.GetDialogueManager ().DisplayMessage (DialogueManager_v2.DialogueType.HINT, dialogues, null);

//		ToggleMobileUI (true);
//		playerController.canMove (true);
//		playerAttack.canAttack (true);

		while (!playerAttack.UsingHammer ())
			yield return null;

		GameController_v7.Instance.GetDialogueManager ().HintSuccess ();

		StartCoroutine (EnemySighted ());
	}

	private IEnumerator EnemySighted() {
		while (!enemySighted.HasSighted ())
			yield return null;

		enableDialogue (); //I see an enemy ahead, suggest hammer
		Parameters data;
		while (GameController_v7.Instance.GetDialogueManager ().IsDialogueBoxPlaying ()) {
			data = new Parameters();
			data.PutExtra ("x", enemy.gameObject.transform.position.x);
			data.PutExtra ("y", enemy.gameObject.transform.position.y);
			EventBroadcaster.Instance.PostEvent (EventNames.ZOOM_CAMERA_TOWARDS, data);
			disablePlayerControls ();
			yield return null;
		}
		data = new Parameters();
		data.PutExtra ("shouldZoomIn", false);
		EventBroadcaster.Instance.PostEvent (EventNames.SHOULD_CAMERA_ZOOM_IN, data);

//		ToggleMobileUI (true);
//		playerController.canMove (true);
//		playerAttack.canAttack (true);

		EnablePlayerControls ();

		StartCoroutine (Fighting ());
//		StartCoroutine (Searching ());
	}

//	private IEnumerator Searching() {
//	
//	}
	private IEnumerator Fighting() {
		bool tried = false;
		bool sighted = false;
		int count = 1;
//		while (stabilityNumberLine.GetStabilityPointer ().GetNumerator () / (float)stabilityNumberLine.GetStabilityPointer ().GetDenominator ()
//		       != stabilityNumberLine.GetTargetMarker ().GetNumerator () / (float)stabilityNumberLine.GetTargetMarker ().GetDenominator ()) {
		while (stabilityNumberLine.GetStabilityPointer ().GetNumerator () != stabilityNumberLine.GetTargetMarker ().GetNumerator () ||
			stabilityNumberLine.GetStabilityPointer ().GetDenominator () != stabilityNumberLine.GetTargetMarker ().GetDenominator ()) {
			Debug.Log ("while stability");
			if (enemyHit.isHit) {
				Debug.Log ("ENEMY IS HIT");
				GameController_v7.Instance.GetPauseController ().Pause ();

				//Its not dying! Fix stability!
				GameController_v7.Instance.GetDialogueManager ().DisplayMessage (DialogueManager_v2.DialogueType.DIALOGUE, invulnerableEnemy, null);
				while (GameController_v7.Instance.GetDialogueManager ().IsDialogueBoxPlaying ()) {
					disablePlayerControls ();
					yield return null;
				}

				EnablePlayerControls ();
//				ToggleMobileUI (true);
//				playerController.canMove (true);
//				playerAttack.canAttack (true);

				GameController_v7.Instance.GetPauseController ().Continue ();
				enemyHit.isHit = false;
				tried = true;
			}

			if (tried && count == 1) {
				GameController_v7.Instance.GetDialogueManager ().DisplayMessage (DialogueManager_v2.DialogueType.HINT, dialogues, null);
				count = 0;
			}

			if (boxSighted.IsSighted () && !sighted) {
				sighted = true;
				Debug.Log ("Box sighted");
				GameController_v7.Instance.GetDialogueManager ().HintSuccess ();
				GameController_v7.Instance.GetDialogueManager ().DisplayMessage (DialogueManager_v2.DialogueType.DIALOGUE, breakTheBox, null);

				while (GameController_v7.Instance.GetDialogueManager ().IsDialogueBoxPlaying ()) {
					disablePlayerControls ();
					yield return null;
				}

				GameController_v7.Instance.GetDialogueManager ().DisplayMessage (DialogueManager_v2.DialogueType.HINT, breakTheBoxHint, null);

				EnablePlayerControls ();
			}
			yield return null;
		}

		GameController_v7.Instance.GetDialogueManager ().HintSuccess ();

//		while (GameController_v7.Instance.GetDialogueManager ().IsHintBoxUp ())
//			yield return null;

		if (dialogues.currentTextIndex != 5)
			dialogues.currentTextIndex = 5;

		//Defeat the enemy
		GameController_v7.Instance.GetDialogueManager ().DisplayMessage (DialogueManager_v2.DialogueType.HINT, dialogues, null);

		while (enemy.isAlive)
			yield return null;

		GameController_v7.Instance.GetDialogueManager ().HintSuccess ();


		GameController_v7.Instance.GetDialogueManager ().DisplayMessage (DialogueManager_v2.DialogueType.DIALOGUE, dialogues, null);

		while (GameController_v7.Instance.GetDialogueManager ().IsDialogueBoxPlaying ()) {
			disablePlayerControls ();
			yield return null;
		}

		EnablePlayerControls ();
	}

	protected override void Init () {
		base.Init ();
	}

	void enableDialogue() {
		playerController.isInDialogue ();
		GameController_v7.Instance.GetDialogueManager ().DisplayMessage (DialogueManager_v2.DialogueType.DIALOGUE, dialogues, null);
	}

	protected override void disablePlayerControls () {
//		ToggleMobileUI (false);
		GameController_v7.Instance.GetMobileUIManager().ToggleMobileControls(false);
		playerController.canMove (false);
		playerAttack.canAttack (false);
	}

	private void EnablePlayerControls() {
		//		ToggleMobileUI (true);
		GameController_v7.Instance.GetMobileUIManager().ToggleMobileControls(true);
		playerController.canMove (true);
		playerAttack.canAttack (true);
	}

//	private void ToggleMobileUI(bool flag) {
//		#if UNITY_ANDROID
////		EventManager.Instance.ToggleMobileControls(flag);
////		GameController_v7.Instance.GetEventManager().ToggleMobileControls(flag);
//		Parameters parameters = new Parameters ();
//		parameters.PutExtra ("FLAG", flag);
//		EventBroadcaster.Instance.PostEvent (EventNames.TOGGLE_MOBILE_UI, parameters);
//		#endif
//	}

	void OnTriggerEnter2D(Collider2D other) {
		Debug.Log ("Tutorial Trigger : Tag is "+other.tag);
		if (other.GetComponent<PlayerYuni>() != null) {
			player = other.gameObject;
			PlayerYuni playerStats = player.GetComponent<PlayerYuni> ();
			playerAttack = playerStats.GetPlayerAttack ();
			playerController = playerStats.GetPlayerMovement ();
			PlayScenes ();
		}
	}
}
