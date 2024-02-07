using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PuzzleSolvingTutorial : Cutscene {

	public GameObject ravineObject;

	private DynamicRavine ravine;
	private GameObject player;
	private PlayerMovement playerController;
	//	private PlayerPlatformerController playerController; changed, use PlayerMovement instead @Candy 
	private PlayerAttack playerAttack;
	private PickupObject pickupObject;
	private bool approachedBox = false;

	[SerializeField] DialogueTrigger dialogueTrigger;
	[SerializeField] DialogueManager dialogueManager;

	protected override void Init() {
		this.name = "Puzzle Solving Cutscene";
		dialogueTrigger = GetComponent<DialogueTrigger> ();
		dialogueManager = GameObject.Find ("DialogueManager").GetComponent<DialogueManager> ();
		base.Init ();
	}

	public override void PlayScenes() {
		if (!isPlaying && !isTriggered) {
			//			isPlaying = true;
			//			this.isTriggered = true;
			base.PlayScenes();
			disablePlayerControls ();
			//			playerAttack.enabled = false;
			//			playerController.canJump = false;
			//			playerController.canWalk = false;
			StartCoroutine (RavineIntro ());
		}
	}

	private IEnumerator RavineIntro() {
		yield return null;
	}

	protected override void disablePlayerControls() {
		playerController.canMove (false);
		playerAttack.canAttack (false);
	}

	void enableDialogue() {
		//		disablePlayerControls ();
		playerController.isInDialogue ();
		dialogueTrigger.TriggerDialogue ();
	}
}
