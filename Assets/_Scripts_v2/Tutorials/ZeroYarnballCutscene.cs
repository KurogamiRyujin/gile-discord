using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ZeroYarnballCutscene : MonoBehaviour {

	//	[SerializeField] private StabilityNumberLine numberLine;
	[SerializeField] private bool isTriggered;
	[SerializeField] private bool isPlaying;
	[SerializeField] private bool hasSet;

	private PlayerYuni player;
//	private PlayerAttack playerAttack;
//	private PlayerMovement playerController;
//	private bool hasCharm;
//	private bool isStable;



//	protected override void Init() {
//		this.name = "Zero Yarnball Cutscene";
////		base.Init ();
//	}

	void Awake() {
		this.hasSet = false;
	}
	private IEnumerator SetZero() {
		this.isPlaying = true;
		this.isTriggered = true;
		this.player.AcquireAll ();
		this.player.GetPlayerAttack ().canAttack (true);
//		this.player.GetPlayerAttack ().SetEquippedDenominator (0);
//		this.player.GetPlayerAttack ().SetUsingNeedle (false);
//		this.player.GetPlayerAttack ().SetUsingHammer (true);
//		this.player.GetPlayerAttack ().ChangeWeapons ();
		this.isPlaying = false;

//		EventBroadcaster.Instance.PostEvent (EventNames.ON_SWITCH_NEEDLE);
		GameController_v7.Instance.GetMobileUIManager().ToggleMobileControls(false);
		GameController_v7.Instance.GetMobileUIManager().ToggleMobileControls(true);
		yield return null;
	}

	void OnTriggerStay2D(Collider2D other) {
		if (other.gameObject.GetComponent<PlayerYuni> () != null &&
		    (!other.gameObject.GetComponent<PlayerYuni> ().GetPlayerAttack ().HasNeedle () ||
		    !other.gameObject.GetComponent<PlayerYuni> ().GetPlayerAttack ().HasHammer () ||
		    other.gameObject.GetComponent<PlayerYuni> ().GetPlayerAttack ().getEquippedDenominator () != 0) &&
		    !isPlaying && !isTriggered) {
			player = other.gameObject.GetComponent<PlayerYuni> ();
			PlayScenes ();
		}
		else if(!hasSet) {
			this.hasSet = true;
			GameController_v7.Instance.GetMobileUIManager().ToggleMobileControls(false);
			GameController_v7.Instance.GetMobileUIManager().ToggleMobileControls(true);
		}
	}
//	void OnTriggerEnter2D(Collider2D other) {
//		if (other.gameObject.GetComponent<PlayerYuni> () != null) {
//			GameController_v7.Instance.GetMobileUIManager().ToggleMobileControls(false);
//			GameController_v7.Instance.GetMobileUIManager().ToggleMobileControls(true);
//		}
//	}
	public void PlayScenes() {
		if (!isPlaying) {
//			base.PlayScenes();
			StartCoroutine (SetZero ());
		}
	}


}
