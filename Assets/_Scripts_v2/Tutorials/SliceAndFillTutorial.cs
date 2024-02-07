using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SliceAndFillTutorial : Cutscene {

	[SerializeField] private Dialogue dialogues;
	[SerializeField] private YarnballPedestal yarnballPedestal;
	[SerializeField] private HollowBlock hollowBlock;

	private PlayerYuni player;
	private PlayerAttack playerAttack;
	private PlayerMovement playerController;

	protected override void Init() {
		this.name = "Slice and Fill Tutorial Cutscene";
		base.Init ();
	}

	protected override void disablePlayerControls() {
//		ToggleMobileUI (false);
		GameController_v7.Instance.GetMobileUIManager ().ToggleMobileControls (false);

		playerController.canMove (false);
		playerAttack.canAttack (false);
	}

	void enableDialogue() {
		playerController.isInDialogue ();
		GameController_v7.Instance.GetDialogueManager ().DisplayMessage (DialogueManager_v2.DialogueType.DIALOGUE, dialogues, null);
	}


	private IEnumerator FloatingSkyBlock() {
		enableDialogue ();//floating sky block
		while (GameController_v7.Instance.GetDialogueManager ().IsDialogueBoxPlaying ()) {
			disablePlayerControls ();
			yield return null;
		}
		GameController_v7.Instance.GetMobileUIManager ().ToggleMobileControls (true);

		//Pickup the yarnball
		GameController_v7.Instance.GetDialogueManager ().DisplayMessage (DialogueManager_v2.DialogueType.HINT, dialogues, null);
		playerController.canMove (true);

		while (this.yarnballPedestal.yarnball.GetDenominator() != 0)
			yield return null;
		GameController_v7.Instance.GetMobileUIManager ().ToggleMobileControls (true);

		GameController_v7.Instance.GetDialogueManager ().HintSuccess ();

		GameController_v7.Instance.GetDialogueManager ().DisplayMessage (DialogueManager_v2.DialogueType.HINT, dialogues, null);

		playerAttack.canAttack (true);

		while (!this.hollowBlock.IsSolved ())
			yield return null;

		GameController_v7.Instance.GetDialogueManager ().HintSuccess ();
	}

	void OnTriggerEnter2D(Collider2D other) {
		if (other.GetComponent<PlayerYuni>() != null) {
			player = other.gameObject.GetComponent<PlayerYuni> ();
			playerAttack = player.GetPlayerAttack ();
			playerController = player.GetPlayerMovement ();
			PlayScenes ();
		}
	}

	public override void PlayScenes() {
		if (!isPlaying && !isTriggered) {
			base.PlayScenes();
			playerAttack.canAttack (false);
			StartCoroutine (FloatingSkyBlock ());
		}
	}
}
