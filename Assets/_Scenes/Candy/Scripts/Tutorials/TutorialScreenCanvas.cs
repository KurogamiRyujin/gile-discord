using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TutorialScreenCanvas : MonoBehaviour {
	[SerializeField] private TutorialList screenTutoriaList;
	[SerializeField] private TutorialItems screenTutorialItems;

	private bool isFromTutorial = false;
	private bool isFromTutorialUnstableRooms = false;
	private bool isFromTutorialCarryItems = false;
	private bool isFromTutorialFixingBlocks = false;
	private bool isFromTutorialUsingNeedle = false;
	private bool isFromTutorialUsingHammer = false;
	private bool isFromTutorialPowerCharms = false;


	private bool isFromTutorialSubdividing = false;
	void Awake() {
		EventBroadcaster.Instance.AddObserver (EventNames.FROM_TUTORIAL_CALL, TutorialCall);
		EventBroadcaster.Instance.AddObserver (EventNames.SHOW_TUTORIAL_UNSTABLE_ROOMS, ScriptOpenUnstableRooms);

		EventBroadcaster.Instance.AddObserver (EventNames.SHOW_TUTORIAL_CARRY_ITEM, ScriptOpenCarryItems);
		EventBroadcaster.Instance.AddObserver (EventNames.SHOW_TUTORIAL_FIXING_BLOCKS, ScriptOpenFixingBlocks);
		EventBroadcaster.Instance.AddObserver (EventNames.SHOW_TUTORIAL_USING_NEEDLE, ScriptOpenUsingNeedle);
		EventBroadcaster.Instance.AddObserver (EventNames.SHOW_TUTORIAL_USING_HAMMER, ScriptOpenUsingHammmer);
		EventBroadcaster.Instance.AddObserver (EventNames.SHOW_TUTORIAL_POWER_CHARMS, ScriptOpenPowerCharms);

	}
	void OnDestroy() {
		EventBroadcaster.Instance.RemoveObserver (EventNames.FROM_TUTORIAL_CALL);
		EventBroadcaster.Instance.RemoveObserver (EventNames.SHOW_TUTORIAL_UNSTABLE_ROOMS);
		EventBroadcaster.Instance.RemoveObserver (EventNames.SHOW_TUTORIAL_CARRY_ITEM);


		EventBroadcaster.Instance.RemoveObserver (EventNames.SHOW_TUTORIAL_FIXING_BLOCKS);
		EventBroadcaster.Instance.RemoveObserver (EventNames.SHOW_TUTORIAL_USING_NEEDLE);
		EventBroadcaster.Instance.RemoveObserver (EventNames.SHOW_TUTORIAL_USING_HAMMER);
		EventBroadcaster.Instance.RemoveObserver (EventNames.SHOW_TUTORIAL_POWER_CHARMS);
	}
	// To be called by tutorials
	public void ScriptOpenUnstableRooms() {
		EventBroadcaster.Instance.PostEvent (EventNames.FROM_TUTORIAL_CALL);
		this.isFromTutorialUnstableRooms = true;
		this.Open ();
		this.OpenTutorialItems ();
		this.OpenUnstableRoom ();
	}

	public void ScriptOpenCarryItems() {
		EventBroadcaster.Instance.PostEvent (EventNames.FROM_TUTORIAL_CALL);
		this.isFromTutorialCarryItems = true;
		this.Open ();
		this.OpenTutorialItems ();
		this.OpenCarryItems ();
	}

	public void ScriptOpenFixingBlocks() {
		EventBroadcaster.Instance.PostEvent (EventNames.FROM_TUTORIAL_CALL);
		this.isFromTutorialFixingBlocks = true;
		this.Open ();
		this.OpenTutorialItems ();
		this.OpenFixingBlocks ();
	}

	public void ScriptOpenUsingNeedle() {
		EventBroadcaster.Instance.PostEvent (EventNames.FROM_TUTORIAL_CALL);
		this.isFromTutorialUsingNeedle = true;
		this.Open ();
		this.OpenTutorialItems ();
		this.OpenUsingNeedle ();
	}

	public void ScriptOpenUsingHammmer() {
		EventBroadcaster.Instance.PostEvent (EventNames.FROM_TUTORIAL_CALL);
		this.isFromTutorialUsingHammer = true;
		this.Open ();
		this.OpenTutorialItems ();
		this.OpenUsingHammer ();
	}

	public void ScriptOpenPowerCharms() {
		EventBroadcaster.Instance.PostEvent (EventNames.FROM_TUTORIAL_CALL);
		this.isFromTutorialPowerCharms = true;
		this.Open ();
		this.OpenTutorialItems ();
		this.OpenPowerCharms ();
	}

	public void TutorialCall() {
		this.isFromTutorial = true;
	}

	void Start() {
		this.Close ();
	}

	public void Close() {
		this.gameObject.SetActive (false);
	}

	// Used to close all sub screens
	public void CloseAll() {
		this.GetScreenTutorialItems ().Close ();
		this.GetScreenTutorialList ().Close ();
	}

	public void BackButton() {
		if (this.isFromTutorial) {
			this.isFromTutorial = false;
			CheckPostEvents ();
			this.Close ();
		} else {
			this.OpenTutorialList ();
		}
	}
	// Add handler variable check here
	public void CheckPostEvents() {
		if (this.isFromTutorialUnstableRooms) {
			this.isFromTutorialUnstableRooms = false;
			EventBroadcaster.Instance.PostEvent (EventNames.CLOSE_TUTORIAL_UNSTABLE_ROOM);
		}
		if (this.isFromTutorialCarryItems) {
			this.isFromTutorialCarryItems = false;
			EventBroadcaster.Instance.PostEvent (EventNames.CLOSE_TUTORIAL_CARRY_ITEM);
		}
		if (this.isFromTutorialFixingBlocks) {
			this.isFromTutorialFixingBlocks = false;
			EventBroadcaster.Instance.PostEvent (EventNames.CLOSE_TUTORIAL_FIXING_BLOCKS);
		}
		if (this.isFromTutorialUsingNeedle) {
			this.isFromTutorialUsingNeedle = false;
			EventBroadcaster.Instance.PostEvent (EventNames.CLOSE_TUTORIAL_USING_NEEDLE);
		}
		if (this.isFromTutorialUsingHammer) {
			this.isFromTutorialUsingNeedle = false;
			EventBroadcaster.Instance.PostEvent (EventNames.CLOSE_TUTORIAL_USING_HAMMER);
		}
		if (this.isFromTutorialPowerCharms) {
			this.isFromTutorialPowerCharms = false;
			EventBroadcaster.Instance.PostEvent (EventNames.CLOSE_TUTORIAL_POWER_CHARMS);
		}
		if (this.isFromTutorialSubdividing) {
			this.isFromTutorialSubdividing = false;
			EventBroadcaster.Instance.PostEvent (EventNames.CLOSE_TUTORIAL_SUBDIVIDING);
		}
	}

	public void OpenTutorialList() {
		this.CloseAll ();
		this.GetScreenTutorialList ().Open ();
	}

	public void OpenTutorialItems() {
		this.CloseAll ();
		this.GetScreenTutorialItems ().Open ();
	}


	// To be called by TutorialList
	public void OpenUnstableRoom() {
		this.OpenTutorialItems ();
		this.GetScreenTutorialItems ().OpenUnstableTutorial ();
	}
	public void OpenCarryItems() {
		this.OpenTutorialItems ();
		this.GetScreenTutorialItems ().OpenCarryTutorial ();
	}

	public void OpenFixingBlocks() {
		this.OpenTutorialItems ();
		this.GetScreenTutorialItems ().OpenFixingBoxTutorial ();
	}

	public void OpenUsingNeedle() {
		this.OpenTutorialItems ();
		this.GetScreenTutorialItems ().OpenUsingNeedleTutorial ();
	}

	public void OpenUsingHammer() {
		this.OpenTutorialItems ();
		this.GetScreenTutorialItems ().OpenUsingHammerTutorial ();
	}
	public void OpenPowerCharms() {
		this.OpenTutorialItems ();
		this.GetScreenTutorialItems ().OpenPowerCharmsTutorial ();
	}
	public void OpenSubdividing() {
		this.OpenTutorialItems ();
		this.GetScreenTutorialItems ().OpenSubdividingTutorial ();
	}

	public void Open() {
		SoundManager.Instance.Play (AudibleNames.Button.BASIC, false);
		this.OpenTutorialList ();
		this.gameObject.SetActive (true);
	}
	// Mainly used by PlayerAttack to prevent Yuni from shooting the needle while in a UI Menu
	public bool IsOpen() {
		if (gameObject.activeInHierarchy) {
			return true;
		}
		return false;
	}
	public TutorialItems GetScreenTutorialItems() {
		if (this.screenTutorialItems == null) {
			this.screenTutorialItems = GetComponentInChildren<TutorialItems> ();
		}
		return this.screenTutorialItems;
	}
	public TutorialList GetScreenTutorialList() {
		if (this.screenTutoriaList == null) {
			this.screenTutoriaList = GetComponentInChildren<TutorialList> ();
		}
		return this.screenTutoriaList;
	}
}
