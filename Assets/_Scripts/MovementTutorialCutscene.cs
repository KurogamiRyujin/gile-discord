using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MovementTutorialCutscene : Cutscene {

	private GameObject player;
//	private PlayerPlatformerController playerController;
	private PlayerMovement playerController;
	private PlayerAttack playerAttack;
	private DialogueTrigger dialogueTrigger;
	[SerializeField] private DialogueManager dialogueManager;

	[SerializeField] private bool hasTriggered = false;

	void Start() {
		Init ();
	}

	protected override void Init() {
		this.name = "Movement Tutorial Cutscene";
		dialogueTrigger = GetComponent<DialogueTrigger> ();
		if (dialogueManager == null) {
			dialogueManager = GameObject.FindGameObjectWithTag ("DialogueManager").GetComponent<DialogueManager> ();
		}
//		dialogueManager = GameObject.Find ("DialogueManager").GetComponent<DialogueManager> ();
		base.Init ();
//		playerController = GameObject.FindGameObjectWithTag ("Player").GetComponent<PlayerPlatformerController> ();
//		playerAttack = GameObject.FindGameObjectWithTag ("Player").GetComponent<PlayerAttack> ();
	}

	private IEnumerator Introduction() {
		Debug.Log ("Introduction");
		enableDialogue ();

		while (dialogueManager.isPlaying) {
			disablePlayerControls ();
			yield return null;
		}

		StartCoroutine (WalkTutorial ());
	}

	private IEnumerator WalkTutorial() {
		Debug.Log ("Walk tutorial start");
		Vector2 startPos = new Vector2 (player.transform.position.x, player.transform.position.y);
		bool hasMoved = false;

		enableDialogue ();

		while (dialogueManager.isPlaying) {
			disablePlayerControls ();
			yield return null;
		}

		ToggleMobileUI (true);
		playerController.canWalk = true;

		while (!hasMoved) {
			if (Mathf.Abs (Vector2.Distance(startPos, player.transform.position)) > 0.4 /*Mathf.Epsilon*/) {
				hasMoved = true;
			}
			yield return null;
		}

		Debug.Log ("Completed walk tutorial");
		StartCoroutine (JumpTutorial ());
	}

	private IEnumerator JumpTutorial() {
		Debug.Log ("Jump tutorial start");
		float startY = player.transform.position.y;
		bool hasJumped = false;

		enableDialogue ();

		while (dialogueManager.isPlaying) {
			disablePlayerControls ();
			yield return null;
		}

		ToggleMobileUI (true);
		playerController.canWalk = true;
		playerController.canJump = true;

		while (!hasJumped) {
			if ((player.transform.position.y - startY) > Mathf.Epsilon) {
				hasJumped = true;
			}

			yield return null;
		}

		Debug.Log ("Completed jump tutorial");
		StartCoroutine (ContinueNextRoom ());
	}

	private IEnumerator ContinueNextRoom() {
		enableDialogue ();
		while (dialogueManager.isPlaying) {
			disablePlayerControls ();
			yield return null;
		}

		ToggleMobileUI (true);
		playerController.canMove (true);
	}

	void ToggleMobileUI(bool flag) {
		#if UNITY_ANDROID
//		EventManager.Instance.ToggleMobileControls(flag); // TODO:
//		GameController_v7.Instance.GetEventManager().ToggleMobileControls(flag);
		Parameters parameters = new Parameters ();
		parameters.PutExtra ("FLAG", flag);
		EventBroadcaster.Instance.PostEvent (EventNames.TOGGLE_MOBILE_UI, parameters);

		#endif
	}

	protected override void disablePlayerControls () {
		ToggleMobileUI (false);
		playerController.canMove (false);
		playerAttack.canAttack (false);
	}

	// TODO: move this to a super class. duplicate with Tutorial Cutscene
	private void enableDialogue() {
		ToggleMobileUI (true);
		dialogueTrigger.TriggerDialogue ();
		playerController.isInDialogue ();
//		dialogueTrigger.TriggerDialogue ();
	}

	public override void PlayScenes() {
		if (!isPlaying && !isTriggered) {
//			isPlaying = true;
			base.PlayScenes();
			disablePlayerControls ();
			StartCoroutine (Introduction ());
		}
	}

	void OnTriggerEnter2D(Collider2D other) {
		if (other.CompareTag ("Player")) {
			this.hasTriggered = true;
			if (player == null) {
				player = other.gameObject;
				playerAttack = player.GetComponent<PlayerAttack> ();
				playerController = player.GetComponent<PlayerMovement> ();
			}
			PlayScenes ();
			Destroy (this.gameObject.GetComponent<BoxCollider2D> ());
		}
	}
}
