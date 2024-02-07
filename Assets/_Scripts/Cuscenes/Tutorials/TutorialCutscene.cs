using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TutorialCutscene : Cutscene {

	[SerializeField] private Dialogue dialogues;
	[SerializeField] private string[] errorDialogue1;
	[SerializeField] private string[] errorDialogue2;
	[SerializeField] private string[] errorHint;

	[SerializeField] private GameObject pedestal;
	[SerializeField] private GameObject block;

	private const float DEFAULT_CG_WAIT = 5.0f;
//	private const float FALLING_CG_WAIT = 6.5f;
	private const float MEMORY_CG_WAIT = 10.3f;
	private const float NEEDLE_CG_WAIT = 8.0f;
	private GameObject player;

	private PlayerMovement playerController;
	private PlayerAttack playerAttack;
	private PickupObject pickupObject;
	private bool approachedBox = false;

	void Start() {
		Init ();
	}


	protected override void Init() {
		this.name = "Tutorial Cutscene";
		pickupObject = pedestal.GetComponent<PickupObject> ();
		base.Init ();

		if (this.isTriggered) {
			Debug.Log ("Tutorial Cutscene isTriggered : Deactivating pededstal");
			this.pedestal.SetActive (false);
			foreach (Collider2D collider in GetComponentsInChildren<Collider2D>()) {
				collider.enabled = false;
			}
		}
	}

	protected override void disablePlayerControls() {
		ToggleMobileUI (false);
		playerController.canMove (false);
		playerAttack.canAttack (false);
	}

	void enableDialogue() {
		playerController.isInDialogue ();
		GameController_v7.Instance.GetDialogueManager ().DisplayMessage (DialogueManager_v2.DialogueType.DIALOGUE, dialogues, null);
	}

	private IEnumerator StrangeBox() {
		// Sky
//		GameController_v7.Instance.GetDialogueManager ().ShowCG ();
//		yield return new WaitForSecondsRealtime (DEFAULT_CG_WAIT);
//		GameController_v7.Instance.GetDialogueManager ().HideCG ();


		// Shatter
//		GameController_v7.Instance.GetDialogueManager ().ShowCG ();
//		SoundManager.Instance.Play (AudibleNames.BreakableBox.GLASS, false);
//		yield return new WaitForSecondsRealtime (DEFAULT_CG_WAIT);
//		GameController_v7.Instance.GetDialogueManager ().HideCG ();

		// Falling
//		GameController_v7.Instance.GetDialogueManager ().ShowCG ();
//		yield return new WaitForSecondsRealtime (FALLING_CG_WAIT);
//		GameController_v7.Instance.GetDialogueManager ().HideCG ();

		// As I looked at the sky
//		GameController_v7.Instance.GetDialogueManager ().ShowCG ();
//		yield return new WaitForSecondsRealtime (DEFAULT_CG_WAIT);
//		GameController_v7.Instance.GetDialogueManager ().HideCG ();


		// TODO: Where am I...

		playerAttack.canAttack (false);

		enableDialogue ();//Tutorial_4_StrangeBox_1
		while (GameController_v7.Instance.GetDialogueManager().IsDialogueBoxPlaying()) {
			disablePlayerControls ();
			yield return null;
		}

		ToggleMobileUI (true);
		playerController.canMove (true);

		while (!approachedBox) {
			yield return null;
		}

		enableDialogue ();//I can't touch this
		while (GameController_v7.Instance.GetDialogueManager ().IsDialogueBoxPlaying ()) {
			disablePlayerControls ();
			yield return null;
		}

		//flashback
//		GameController_v7.Instance.GetDialogueManager ().ShowCG ();
//		yield return new WaitForSecondsRealtime (MEMORY_CG_WAIT);
//		GameController_v7.Instance.GetDialogueManager ().HideCG ();

		if(playerController.m_FacingRight)
			playerController.Flip ();

		enableDialogue ();//Wait I remember now
		while (GameController_v7.Instance.GetDialogueManager().IsDialogueBoxPlaying()) {

			disablePlayerControls ();
			yield return null;
		}

		ToggleMobileUI (true);
		playerController.canMove (true);

		StartCoroutine (InteractionTutorial ());
	}

	private IEnumerator InteractionTutorial() {
		bool interacted = false;

		if(!playerController.m_FacingRight)
			playerController.Flip ();

		enableDialogue();//Spot Needle
		while (GameController_v7.Instance.GetDialogueManager().IsDialogueBoxPlaying()) {
			disablePlayerControls ();
			yield return null;
		}

		//Hint to move towards needle
		GameController_v7.Instance.GetDialogueManager ().DisplayMessage (DialogueManager_v2.DialogueType.HINT, dialogues, null);

		ToggleMobileUI (true);
		playerController.canMove (true);
		while (!interacted) {
			//if the needle is still in the world, interacted is false
//			if (!pickupObject.pickup.activeInHierarchy) {
			if (pickupObject.IsTaken()) {
				interacted = true;
				// disable pickup needle hint
				GameObject.Find("Hint_Needle").GetComponentInChildren<FloatingButtonHint>().isTangible = false;
			}

			yield return null;
		}

		StartCoroutine (NeedleTutorial ());
	}

	private IEnumerator NeedleTutorial() {
		Debug.Log ("Needle tutorial start");

		enableDialogue ();//Tutorial_4_PickedUpNeedle
		while (GameController_v7.Instance.GetDialogueManager().IsDialogueBoxPlaying()) {
			disablePlayerControls ();
			yield return null;
		}

		GameController_v7.Instance.GetDialogueManager ().ShowCG ();
		yield return new WaitForSecondsRealtime (NEEDLE_CG_WAIT);
		GameController_v7.Instance.GetDialogueManager ().HideCG ();

		enableDialogue ();//this happened cuz sky broke, we need to gather sky fragments
		while (GameController_v7.Instance.GetDialogueManager ().IsDialogueBoxPlaying ()) {
			disablePlayerControls ();
			yield return null;
		}

		GameController_v7.Instance.GetDialogueManager ().HideCG ();

		enableDialogue ();//the sky fragment in the needle is reacting to the box, I think this can fix it, can you make me throw it at the box?
		while (GameController_v7.Instance.GetDialogueManager ().IsDialogueBoxPlaying ()) {
			disablePlayerControls ();
			yield return null;
		}

		GameController_v7.Instance.GetDialogueManager ().DisplayMessage (DialogueManager_v2.DialogueType.HINT, dialogues, null);

		ToggleMobileUI (true);
		playerController.canMove (true);
		playerAttack.canAttack (true);

		PartitionableObject_v2 tutorialBox = null;

		while (tutorialBox == null) {
			tutorialBox = block.GetComponent<PartitionableObject_v2> ();
			tutorialBox.enableClone = false;
			yield return null;
		}

		bool tried = false;
		int index = 0;

		while (!tutorialBox.IsTangible ()) {
			switch (index) {
			case 0:
				if (GameController_v7.Instance.GetPauseController().IsPaused()) {
					Debug.Log ("IS PAUSED ENTERED");
					enableDialogue ();
					while (GameController_v7.Instance.GetDialogueManager().IsDialogueBoxPlaying()) {
						disablePlayerControls ();
						Debug.Log ("DialogueManager IN");
						yield return null;
					}
					Debug.Log ("DialogueManager DONE");
//					GameObject cloneObject = tutorialBox.GetComponentInChildren<PartitionedClone_v2> ().gameObject;

//					if (cloneObject != null) {
////						tutorialBox.GetComponentInChildren<HintBubbleManager> ().RemoveComponents ();
//						if (cloneObject.GetComponentInChildren<HintBubbleManager> () != null) {
//							cloneObject.GetComponentInChildren<HintBubbleManager> ().RemoveComponents ();
//						}
//					}
					tutorialBox.enableClone = true;
					enableDialogue ();
					while (GameController_v7.Instance.GetDialogueManager().IsDialogueBoxPlaying()) {
						disablePlayerControls ();
						yield return null;
					}

					//Hint to adjust fill
					GameController_v7.Instance.GetDialogueManager ().DisplayMessage (DialogueManager_v2.DialogueType.HINT, dialogues, null);

					index = 1;

					ToggleMobileUI (true);
					playerAttack.canAttack (true);
					playerController.canMove (true);
				}
				break;
			case 1:
				if (!GameController_v7.Instance.GetPauseController().IsPaused() && !tutorialBox.IsTangible () && !tutorialBox.IsAnimatingFill () && !tutorialBox.IsFilling ()) {
					playerController.isInDialogue ();
					GameController_v7.Instance.GetDialogueManager ().DisplayMessage (DialogueManager_v2.DialogueType.DIALOGUE, errorDialogue1, null);
					while (GameController_v7.Instance.GetDialogueManager().IsDialogueBoxPlaying()) {
						disablePlayerControls ();
						yield return null;
					}

					//Error Hint
					GameController_v7.Instance.GetDialogueManager ().DisplayMessage (DialogueManager_v2.DialogueType.HINT, errorHint, null);

					index = 2;

					ToggleMobileUI (true);
					playerAttack.canAttack (true);
					playerController.canMove (false);
				}
				break;
			case 2:
				if (tried) {
					if (!GameController_v7.Instance.GetPauseController().IsPaused() && !tutorialBox.IsTangible () && !tutorialBox.IsFilling () && !tutorialBox.IsAnimatingFill ()) {
						playerController.isInDialogue ();
						GameController_v7.Instance.GetDialogueManager ().DisplayMessage (DialogueManager_v2.DialogueType.DIALOGUE, errorDialogue2, null);
						while (GameController_v7.Instance.GetDialogueManager().IsDialogueBoxPlaying()) {
							disablePlayerControls ();
							yield return null;
						}

						//Error Hint
						GameController_v7.Instance.GetDialogueManager ().DisplayMessage (DialogueManager_v2.DialogueType.HINT, errorHint, null);

						index++;
						if (index > 2)
							index = 2;

						ToggleMobileUI (true);
						playerAttack.canAttack (true);
						playerController.canMove (true);

						tried = false;
					}
				} else if (GameController_v7.Instance.GetPauseController().IsPaused()) {
					tried = true;
				}
				break;
			}

			yield return null;
		}

		StartCoroutine (ContinueNextRoom ());
	}

	private IEnumerator ContinueNextRoom() {
		enableDialogue ();
		while (GameController_v7.Instance.GetDialogueManager().IsDialogueBoxPlaying()) {
			disablePlayerControls ();
			yield return null;
		}

		ToggleMobileUI (true);
		playerController.canMove (true);
		playerAttack.canAttack (true);
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

	public override void PlayScenes() {
		if (!isPlaying && !isTriggered) {
			base.PlayScenes();
			disablePlayerControls ();
			StartCoroutine (StrangeBox ());
		}
	}

	public void ApproachedBox() {
		this.approachedBox = true;
	}
}
