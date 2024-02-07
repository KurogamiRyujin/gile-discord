using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TutorialItems : MonoBehaviour {
	[SerializeField] private CarryItemsScreenParent tutorialCarryItems;
	[SerializeField] private FixingBlocksScreenParent tutorialFixingBlocks;
	[SerializeField] private UnstableRoomsScreenParent tutorialUnstableRooms;
	[SerializeField] private UsingNeedleScreenParent tutorialUsingNeedle;
	[SerializeField] private UsingHammerScreenParent tutorialUsingHammer;
	[SerializeField] private UsingPowerCharmsScreenParent tutorialPowerCharms;
	[SerializeField] private SubdividingScreenParent tutorialSubdividing;

	// Local open screen functions
	public void OpenUnstableTutorial() {
		this.CloseAll ();
		this.GetTutorialUnstableRooms ().Open ();
	}
	public void OpenCarryTutorial() {
		this.CloseAll ();
		this.GetTutorialCarryItems ().Open ();
	}
	public void OpenFixingBoxTutorial() {
		this.CloseAll ();
		this.GetTutorialFixingBlocks ().Open ();
	}

	public void OpenUsingNeedleTutorial() {
		this.CloseAll ();
		this.GetTutorialUsingNeedle ().Open ();
	}
	public void OpenUsingHammerTutorial() {
		this.CloseAll ();
		this.GetTutorialUsingHammer ().Open ();
	}
	public void OpenPowerCharmsTutorial() {
		this.CloseAll ();
		this.GetTutorialPowerCharms ().Open ();
	}
	public void OpenSubdividingTutorial() {
		if (GetSubdividing () != null) {
			this.CloseAll ();
			this.GetSubdividing ().Open ();
		}
	}

	public void Open() {
		this.gameObject.SetActive (true);
	}
		
	// Use this to close the tutorial screen
	public void Close() {
		this.gameObject.SetActive (false);
		this.CloseAll ();
	}

	// Use this to close all subscreens, to be used by local open screens
	public void CloseAll() {
		this.GetTutorialCarryItems ().Close ();
		this.GetTutorialUnstableRooms ().Close ();
		this.GetTutorialFixingBlocks ().Close ();
		this.GetTutorialUsingNeedle ().Close ();
		this.GetTutorialUsingHammer ().Close ();
		this.GetTutorialPowerCharms ().Close ();
		if (this.GetSubdividing () != null) {
			this.GetSubdividing ().Close ();
		}
	}

	public UnstableRoomsScreenParent GetTutorialUnstableRooms() {
		if (this.tutorialUnstableRooms == null) {
			this.tutorialUnstableRooms.GetComponentInChildren<UnstableRoomsScreenParent> ();
		}
		return this.tutorialUnstableRooms;
	}
	public CarryItemsScreenParent GetTutorialCarryItems() {
		if (this.tutorialCarryItems == null) {
			this.tutorialCarryItems.GetComponentInChildren<CarryItemsScreenParent> ();
		}
		return this.tutorialCarryItems;
	}
	public SubdividingScreenParent GetSubdividing() {
		if (this.tutorialSubdividing == null) {
//			this.tutorialSubdividing.GetComponentInChildren<SubdividingScreenParent> ();
		}
		return this.tutorialSubdividing;
	}
	public FixingBlocksScreenParent GetTutorialFixingBlocks() {
		if (this.tutorialFixingBlocks == null) {
			this.tutorialFixingBlocks.GetComponentInChildren<FixingBlocksScreenParent> ();
		}
		return this.tutorialFixingBlocks;
	}
	public UsingNeedleScreenParent GetTutorialUsingNeedle() {
		if (this.tutorialUsingNeedle == null) {
			this.tutorialUsingNeedle.GetComponentInChildren<UsingNeedleScreenParent> ();
		}
		return this.tutorialUsingNeedle;
	}
	public UsingHammerScreenParent GetTutorialUsingHammer() {
		if (this.tutorialUsingHammer == null) {
			this.tutorialUsingHammer.GetComponentInChildren<UsingHammerScreenParent> ();
		}
		return this.tutorialUsingHammer;
	}

	public UsingPowerCharmsScreenParent GetTutorialPowerCharms() {
		if (this.tutorialPowerCharms == null) {
			this.tutorialPowerCharms.GetComponentInChildren<UsingPowerCharmsScreenParent> ();
		}
		return this.tutorialPowerCharms;
	}
}
