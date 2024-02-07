using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LCDTutorial : Cutscene {

	[SerializeField] private Dialogue dialogues;
	[SerializeField] private string[] errorDialogue;
	[SerializeField] private string[] errorHint;
	[SerializeField] private string[] finishedLCD;
	[SerializeField] private EnemyHealth enemyHealth1;
	[SerializeField] private EnemyHealth enemyHealth2;
	[SerializeField] private GameObject lcdPanel;
	[SerializeField] private Animator crossairAnim;

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
			StartCoroutine (SleepingEnemy ());
		}
	}

	private IEnumerator SleepingEnemy() {
		enableDialogue ();//I see another possessed popcorn but its sleeping, stability is very different
		while (GameController_v7.Instance.GetDialogueManager ().IsDialogueBoxPlaying ()) {
			disablePlayerControls ();
			yield return null;
		}

		EnablePlayerControls ();

		while ((!playerAttack.getHammerObject ().IsAttacking () || !playerAttack.getNeedleThrowing ().flying) && !GameController_v7.Instance.GetPauseController().IsPaused())
			yield return null;

		//if player did not use Yarnball first
		if (enemyHealth1.isAlive) {
			enableDialogue ();//As I thought, try hammering the yarnball
			while (GameController_v7.Instance.GetDialogueManager ().IsDialogueBoxPlaying ()) {
				disablePlayerControls ();
				yield return null;
			}

			//Hit yarnball with hammer
			GameController_v7.Instance.GetDialogueManager ().DisplayMessage (DialogueManager_v2.DialogueType.HINT, dialogues, null);

			EnablePlayerControls ();

			while (!crossairAnim.GetCurrentAnimatorStateInfo (0).IsName ("Crosshair_enter"))
				yield return null;

			enableDialogue ();//click on the point you want me to send this yarnball flying
			while (GameController_v7.Instance.GetDialogueManager ().IsDialogueBoxPlaying ()) {
				disablePlayerControls ();
				yield return null;
			}

			EnablePlayerControls ();

			while (!lcdPanel.activeInHierarchy)
				yield return null;

			enableDialogue ();//match soul's stability with the yarnball by adjusting the values
			while (GameController_v7.Instance.GetDialogueManager ().IsDialogueBoxPlaying ()) {
				disablePlayerControls ();
				yield return null;
			}

			//Click on the arrows near the numbers to align the bars. Click enter when done.
			GameController_v7.Instance.GetDialogueManager ().DisplayMessage (DialogueManager_v2.DialogueType.HINT, dialogues, null);

			EnablePlayerControls ();

			//NOTE: commented out due to error after EnemyHealth was updated
//			while (enemyHealth1.GetDenominator () != playerAttack.getHammerObject().GetDenominator())
//				yield return null;

			enableDialogue ();//our thread's temporarily synced with the phantom's soul! Now's our chance
			while (GameController_v7.Instance.GetDialogueManager ().IsDialogueBoxPlaying ()) {
				disablePlayerControls ();
				yield return null;
			}

			//Defeat the enemy as usual
			GameController_v7.Instance.GetDialogueManager ().DisplayMessage (DialogueManager_v2.DialogueType.HINT, dialogues, null);

			EnablePlayerControls ();

			bool tried = false;
			int count = 1;

			while (enemyHealth1.isAlive) {
				//NOTE: commented out due to error after EnemyHealth was updated
//				if (enemyHealth1.GetDenominator () == playerAttack.getHammerObject ().GetDenominator ())
//					count = 1;
//
//				if (enemyHealth1.GetDenominator () != playerAttack.getHammerObject ().GetDenominator () && count == 1) {
//					tried = true;
//					count = 0;
//				}

				if (enemyHealth1.isAlive && !GameController_v7.Instance.GetPauseController().IsPaused() && tried) {
					playerController.isInDialogue ();//whoops, that went wrong, let's hit it with the yarnball again
					GameController_v7.Instance.GetDialogueManager ().DisplayMessage (DialogueManager_v2.DialogueType.DIALOGUE, errorDialogue, null);
					while (GameController_v7.Instance.GetDialogueManager ().IsDialogueBoxPlaying ()) {
						disablePlayerControls ();
						yield return null;
					}

					//Equipped denominator reverts back after attacking an enemy once. Perform Soul Syncing once again.
					GameController_v7.Instance.GetDialogueManager ().DisplayMessage (DialogueManager_v2.DialogueType.HINT, errorHint, null);

					EnablePlayerControls ();
					tried = false;
				}

				yield return null;
			}
		}

		//NOTE: commented out due to error after EnemyHealth was updated
//		StartCoroutine (LCDEnd ());
	}

	private IEnumerator LCDEnd() {
		playerController.isInDialogue ();
		GameController_v7.Instance.GetDialogueManager ().DisplayMessage (DialogueManager_v2.DialogueType.DIALOGUE, finishedLCD, null);
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
