using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// Cutscene to show the player it is possible to subdivie sky fragments further with the hammer and needle.
/// </summary>
public class SubdivisionTutorialCutscene : Cutscene {

	[SerializeField] private Dialogue dialogues;
	[SerializeField] private Dialogue hintDialogues;
//	[SerializeField] private StabilityNumberLine numberLine;

	private PlayerYuni player;
	private PlayerAttack playerAttack;
	private PlayerMovement playerController;
	private bool hasCharm;
	private bool isStable;



	[SerializeField] private TriggerSkyBlockSpotted triggerSkyBlockSpotted;


	[SerializeField] private TriggerPortalSpotted triggerPortalSpotted;

	[SerializeField] private TriggerCharmSpotted triggerCharmSpotted;

	[SerializeField] private TriggerSkyBlockBroken triggerSkyBlockBroken;
	[SerializeField] private TriggerSkyBlockBroken2 triggerSkyBlockBroken2;
	[SerializeField] private TriggerSkyBlockWholeListener triggerSkyBlockWhole;
//	[SerializeField] private TriggerBoxFix triggerBoxFix;
//	[SerializeField] private TriggerSkyBlockHit triggerSkyBlockHit;
//	[SerializeField] private SlidingDoor slidingDoor;
//	[SerializeField] private TriggerSkyBlockSpotted triggerSkyBlockSpotted;
//	[SerializeField] private TriggerCharmSpotted triggerCharmSpotted;
//	[SerializeField] private TriggerSkyBlockBroken triggerSkyBlockBroken;
//	[SerializeField] private TutorialHandlerPowerCharm triggerPowerCharm;

	void Awake() {
		this.hasCharm = false;
		this.isStable = false;
//		EventBroadcaster.Instance.AddObserver (EventNames.PICKUP_ITEM_CLOSE, CharmPickup);
		EventBroadcaster.Instance.AddObserver (EventNames.CHARM_PICKUP_SWITCH, CharmPickup);
		EventBroadcaster.Instance.AddObserver (EventNames.STABLE_AREA, Stabilize);
	}	
	void OnDestroy() {
		EventBroadcaster.Instance.RemoveObserver (EventNames.CHARM_PICKUP_SWITCH);
		EventBroadcaster.Instance.RemoveObserver (EventNames.STABLE_AREA);
	}
	public void Stabilize() {
		this.isStable = true;
	}
	void Start() {
		Init ();
	}
	public void CharmPickup() {
		this.hasCharm = true;
	}

	protected override void Init() {
		this.name = "Locked Door Cutscene";
		base.Init ();
	}


	protected override void disablePlayerControls() {
		playerController.canMove (false);
		playerAttack.canAttack (false);
	}

	void enableDialogue() {
		playerController.isInDialogue ();
		GameController_v7.Instance.GetDialogueManager ().DisplayMessage (DialogueManager_v2.DialogueType.DIALOGUE, dialogues, null);
	}

	public void DialogueStart() {
		enableDialogue ();
		GameController_v7.Instance.GetMobileUIManager().ToggleMobileControls(false);
		this.disablePlayerControls ();
	}

	public void DialogueEnd() {
		GameController_v7.Instance.GetMobileUIManager().ToggleBaseWithPickupControls(true);
		playerController.canMove (true);
		playerAttack.canAttack (true);
	}

	private IEnumerator SubdivisionTutorial() {
		// Hide Portal
		this.GetTriggerPortalSpotted().HidePortal();
		this.GetTriggerPortalSpotted().Ignore();
		this.GetSkyBlockBroken().Ignore();
		this.GetSkyBlockBroken2().Ignore();

		while (!this.GetTriggerSkyBlockSpotted ().IsTriggered ()) {
			yield return null;
		}


		// [D1]: Let's try to solve those boxes using this Sky Block.
		this.DialogueStart();
		while (GameController_v7.Instance.GetDialogueManager ().IsDialogueBoxPlaying ()) {
			yield return null;
		}
		this.DialogueEnd ();



		// [H1] Mission: Split the sky block using the needle
		GameController_v7.Instance.GetDialogueManager ().DisplayMessage (DialogueManager_v2.DialogueType.HINT, hintDialogues, null);


		// While sky blocks has not been split
		this.GetSkyBlockBroken().Observe();
		while (!this.GetSkyBlockBroken ().IsTriggered ()) {
			yield return null;
		}


		// Success: Split block
		GameController_v7.Instance.GetDialogueManager ().HintSuccess();


		// [D2] Oh no.. I don't think we can fix the boxes with this charm.
		// Let's put these sky fragments back to the sky block.

		this.DialogueStart();
		while (GameController_v7.Instance.GetDialogueManager ().IsDialogueBoxPlaying ()) {
			yield return null;
		}
		this.DialogueEnd ();



		// OPEN Returning To Skyblock TUTORIAL

		// SUBDIVISION TUTORIAL
		// If you slice a sky block with the wrong charm
		// you can return the sky fragments using the hammer

		// Hit the created sky fragments you want to return.
		// Sky fragments will go back to the block they came from.

		// You can then slice the sky block again.

		// Note that you can't slice a sky block that's too small.



		// [D3]: Let's put these fragments back to the sky block.
//		this.DialogueStart();
//		while (GameController_v7.Instance.GetDialogueManager ().IsDialogueBoxPlaying ()) {
//			yield return null;
//		}
//		this.DialogueEnd ();

	

		// [H2] Mission: Hit all the sky fragments with the hammer to return them to sky block
		GameController_v7.Instance.GetDialogueManager ().DisplayMessage (DialogueManager_v2.DialogueType.HINT, hintDialogues, null);

		// While sky block is not yet 1 whole
		this.GetSkyBlockWhole().Observe();
		while (!this.GetSkyBlockWhole ().IsTriggered ()) {
			yield return null;
		}


		// Success: Return pieces
		GameController_v7.Instance.GetDialogueManager ().HintSuccess();


		// [D3]: Alright, let's look around. Maybe there's another
		// charm around here.

		this.DialogueStart();
		while (GameController_v7.Instance.GetDialogueManager ().IsDialogueBoxPlaying ()) {
			yield return null;
		}
		this.DialogueEnd ();



		// Portal Show
		this.GetTriggerPortalSpotted().Observe();
		this.GetTriggerPortalSpotted().ShowPortal();
		while (!this.GetTriggerPortalSpotted ().IsTriggered ()) {
			yield return null;
		}

		// [D4]: What's this..? It looks like a portal.
		// Well we don't have anywhere else to go.
		// Let's see where it leads to
		this.DialogueStart();
		while (GameController_v7.Instance.GetDialogueManager ().IsDialogueBoxPlaying ()) {
			yield return null;
		}
		this.DialogueEnd ();



		// [H3] Mission: Go near the portal and use the HAND button to enter it
		GameController_v7.Instance.GetDialogueManager ().DisplayMessage (DialogueManager_v2.DialogueType.HINT, hintDialogues, null);

		while (!this.GetTriggerCharmSpotted ().IsTriggered ()) {
			yield return null;
		}


		// Success: Enter portal/Charm spotted
		GameController_v7.Instance.GetDialogueManager ().HintSuccess();


		// [D5]: A hidden room! And look, there's a charm. 
		// That should work. Let's take it.
		this.DialogueStart();
		while (GameController_v7.Instance.GetDialogueManager ().IsDialogueBoxPlaying ()) {
			yield return null;
		}
		this.DialogueEnd ();



		// [H4] Mission: Take the charm
		GameController_v7.Instance.GetDialogueManager ().DisplayMessage (DialogueManager_v2.DialogueType.HINT, hintDialogues, null);


		this.hasCharm = false;
		// TODO: while charm is not taken
		while (!hasCharm) {
			yield return null;
		}

		// Success: Charm taken
		GameController_v7.Instance.GetDialogueManager ().HintSuccess();



		// [D6]: We can now fix the ghost blocks with this.
		this.DialogueStart();
		while (GameController_v7.Instance.GetDialogueManager ().IsDialogueBoxPlaying ()) {
			yield return null;
		}
		this.DialogueEnd ();


		// [H5] Mission: Fix the ghost blocks to stabilize the room
		GameController_v7.Instance.GetDialogueManager ().DisplayMessage (DialogueManager_v2.DialogueType.HINT, hintDialogues, null);



		// After slicing the sky block
		this.GetSkyBlockBroken2().Observe();
		while (!this.GetSkyBlockBroken2 ().IsTriggered ()) {
			yield return null;
		}


		// [D7]: Okay. We need a 1/2 and two 1/4 sized pieces to fix those blocks...
		this.DialogueStart();
		while (GameController_v7.Instance.GetDialogueManager ().IsDialogueBoxPlaying ()) {
			yield return null;
		}
		this.DialogueEnd ();



		this.isStable = false;
		while (!this.isStable) {
			yield return null;
		}

		// Success: Fixed ghost blocks
		GameController_v7.Instance.GetDialogueManager ().HintSuccess();

		// [D8]: Good job!
		this.DialogueStart();
		while (GameController_v7.Instance.GetDialogueManager ().IsDialogueBoxPlaying ()) {
			yield return null;
		}
		this.DialogueEnd ();

		// SUBDIVISION 2 TUTORIAL
		// You can return a sky fragment to the sky block to
		// slice it again with the needle.

//		// While "How to use Needle" graphic is still open
//		this.GetTriggerPowerCharm ().Action ();
//		while (!this.GetTriggerPowerCharm ().IsFinished ()) {
//			yield return null;
//		}


		// Mission: Stabilize the room







//		yield return null;

//		this.GetSkyBlockHit ().Ignore ();
//		this.GetSkyBlockBroken().Ignore();
//
//		// [D1] More broken objects. But I don't see any sky fragments lying around.
//		// ... We need to fix those boxes
//		this.DialogueStart();
//		while (GameController_v7.Instance.GetDialogueManager ().IsDialogueBoxPlaying ()) {
//			yield return null;
//		}
//		this.DialogueEnd ();
//
//		// [H1] Mission: Explore the room
//		GameController_v7.Instance.GetDialogueManager ().DisplayMessage (DialogueManager_v2.DialogueType.HINT, hintDialogues, null);
//
//		// While sky block is not yet spotted
//		while (!this.GetTriggerSkyBlockSpotted ().IsTriggered ()) {
//			yield return null;
//		}
//
//		// Success: Explore the room
//		GameController_v7.Instance.GetDialogueManager ().HintSuccess();
//
//		// [D2] Hey that's a huge sky fragment. I guess it's better to call it a Sky Block
//		// It's stuck on the wall though. Let's try to knock it down using the needle
//		this.DialogueStart();
//		while (GameController_v7.Instance.GetDialogueManager ().IsDialogueBoxPlaying ()) {
//			yield return null;
//		}
//		this.DialogueEnd ();
//
//
//		// [H2] Mission: Knock down the sky block using the needle
//		GameController_v7.Instance.GetDialogueManager ().DisplayMessage (DialogueManager_v2.DialogueType.HINT, hintDialogues, null);
//
//
//		// While player has not hit skyblock
//		this.GetSkyBlockHit ().Observe ();
//		while (!this.GetSkyBlockHit ().IsTriggered ()) {
//			yield return null;
//		}
//
//
//		// Hide: Hide mission
//		GameController_v7.Instance.GetDialogueManager ().CloseMinorHint();
//
//
//		// [D3] No good! The needle's not strong enough... Let's look around.
//		this.DialogueStart();
//		while (GameController_v7.Instance.GetDialogueManager ().IsDialogueBoxPlaying ()) {
//			yield return null;
//		}
//		this.DialogueEnd ();
//
//
//		// Open sliding door
//		this.GetSlidingDoor ().Open ();
//
//		// [H3] Mission: Look for something to knock down the sky block
//		GameController_v7.Instance.GetDialogueManager ().DisplayMessage (DialogueManager_v2.DialogueType.HINT, hintDialogues, null);
//
//
//		// While charm is not yet spotted
//		while (!this.GetTriggerCharmSpotted ().IsTriggered ()) {
//			yield return null;
//		}
//		// Hide: Hide mission
//		GameController_v7.Instance.GetDialogueManager ().CloseMinorHint();
//
//		// [D4] What's that? Let's pick it up.
//		this.DialogueStart();
//		while (GameController_v7.Instance.GetDialogueManager ().IsDialogueBoxPlaying ()) {
//			yield return null;
//		}
//		this.DialogueEnd ();
//
//
//		// [H4] Mission: Pick up the charm
//		GameController_v7.Instance.GetDialogueManager ().DisplayMessage (DialogueManager_v2.DialogueType.HINT, hintDialogues, null);
//	
//
//		// While pickup UI has not been closed
//		while(!this.hasCharm) {
//			yield return null;
//		}
//		// Success: Pick up the charm
//		GameController_v7.Instance.GetDialogueManager ().HintSuccess();
//
//
//
//		// [D5] Hmm? It's a charm. Like those things people wear for luck.
//		// But this has 'Power' written on it and the number 4.
//		// ...
//		// I don't really believe in this stuff but...
//		// I'll tie it on the needle.
//		// Let's see if it can make it strong enough to break the block.
//
//		// TUTORIAL CHARM
//
//		this.DialogueStart();
//		while (GameController_v7.Instance.GetDialogueManager ().IsDialogueBoxPlaying ()) {
//			yield return null;
//		}
//		this.DialogueEnd ();
//
//
//		// While "How to use Needle" graphic is still open
//		this.GetTriggerPowerCharm ().Action ();
//		while (!this.GetTriggerPowerCharm ().IsFinished ()) {
//			yield return null;
//		}
//
//		// I don't really believe in this stuff but...
//		// I'll tie it on the needle.
//		// Let's see if it can make it strong enough to break the block.
//		this.DialogueStart();
//		while (GameController_v7.Instance.GetDialogueManager ().IsDialogueBoxPlaying ()) {
//			yield return null;
//		}
//		this.DialogueEnd ();
//
//
//
//		// [H5] Mission: Hit the sky block with the charmed needle
//		GameController_v7.Instance.GetDialogueManager ().DisplayMessage (DialogueManager_v2.DialogueType.HINT, hintDialogues, null);
//
//		// While sky blocks has not been split
//		this.GetSkyBlockBroken().Observe();
//		while (!this.GetSkyBlockBroken ().IsTriggered ()) {
//			yield return null;
//		}
//
//		// Success: Hit the sky block with the charmed needle
//		GameController_v7.Instance.GetDialogueManager ().HintSuccess();
//
//
//		// [D6] I can't believe that worked.
//		this.DialogueStart();
//		while (GameController_v7.Instance.GetDialogueManager ().IsDialogueBoxPlaying ()) {
//			yield return null;
//		}
//		this.DialogueEnd ();
//
//		// [H6] Mission: Use the sky fragments to fix the boxes and stabilize the room
//		GameController_v7.Instance.GetDialogueManager ().DisplayMessage (DialogueManager_v2.DialogueType.HINT, hintDialogues, null);
//		while (!this.isStable) {
//			yield return null;
//		}
//
//		// Success: [H6] Use the sky fragments to fix the boxes and stabilize the room
//		GameController_v7.Instance.GetDialogueManager ().HintSuccess();

	}

	void OnTriggerEnter2D(Collider2D other) {
		if (other.gameObject.GetComponent<PlayerYuni> ()) {
			player = other.gameObject.GetComponent<PlayerYuni> ();
			playerAttack = player.GetPlayerAttack ();
			playerController = player.GetPlayerMovement ();
			PlayScenes ();
		}
	}
//	public TriggerBoxFix GetBoxFixTrigger() {
//		if (this.triggerBoxFix == null) {
//			this.triggerBoxFix = GetComponentInChildren<TriggerBoxFix> ();
//		}
//		return this.triggerBoxFix;
//	}

//	public TriggerSkyBlockHit GetSkyBlockHit() {
//		if (this.triggerSkyBlockHit == null) {
//			this.triggerSkyBlockHit = GetComponentInChildren<TriggerSkyBlockHit> ();
//		}
//		return this.triggerSkyBlockHit;
//	}

//	public TutorialHandlerPowerCharm GetTriggerPowerCharm() {
//		if (this.triggerPowerCharm == null) {
//			this.triggerPowerCharm = GetComponentInChildren<TutorialHandlerPowerCharm> ();
//		}
//		return this.triggerPowerCharm;
//	}

	public override void PlayScenes() {
		if (!isPlaying && !isTriggered) {
			base.PlayScenes();
//			playerAttack.canAttack (false);
			StartCoroutine (SubdivisionTutorial ());
		}
	}

//	public SlidingDoor GetSlidingDoor() {
//		if(this.slidingDoor == null) {
//			this.slidingDoor = GetComponentInChildren<SlidingDoor> ();
//		}
//		return this.slidingDoor;
//	}

	public TriggerSkyBlockSpotted GetTriggerSkyBlockSpotted() {
		if(this.triggerSkyBlockSpotted == null) {
			this.triggerSkyBlockSpotted = GetComponentInChildren<TriggerSkyBlockSpotted> ();
		}
		return this.triggerSkyBlockSpotted;
	}

	public TriggerCharmSpotted GetTriggerCharmSpotted() {
		if(this.triggerCharmSpotted == null) {
			this.triggerCharmSpotted = GetComponentInChildren<TriggerCharmSpotted> ();
		}
		return this.triggerCharmSpotted;
	}

	public TriggerPortalSpotted GetTriggerPortalSpotted() {
		if(this.triggerPortalSpotted == null) {
			this.triggerPortalSpotted = GetComponentInChildren<TriggerPortalSpotted> ();
		}
		return this.triggerPortalSpotted;
	}

	public TriggerSkyBlockBroken GetSkyBlockBroken() {
		if (this.triggerSkyBlockBroken == null) {
			this.triggerSkyBlockBroken = GetComponentInChildren<TriggerSkyBlockBroken> ();
		}
		return this.triggerSkyBlockBroken;
	}
	public TriggerSkyBlockBroken2 GetSkyBlockBroken2() {
		if (this.triggerSkyBlockBroken2 == null) {
			this.triggerSkyBlockBroken2 = GetComponentInChildren<TriggerSkyBlockBroken2> ();
		}
		return this.triggerSkyBlockBroken2;
	}
	public TriggerSkyBlockWholeListener GetSkyBlockWhole() {
		if (this.triggerSkyBlockWhole == null) {
			this.triggerSkyBlockWhole = GetComponentInChildren<TriggerSkyBlockWholeListener> ();
		}
		return this.triggerSkyBlockWhole;
	}
}
