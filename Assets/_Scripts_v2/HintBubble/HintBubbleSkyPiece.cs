using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class HintBubbleSkyPiece : MonoBehaviour {
	public const string OVERLAPPING_PIECE = "overlappingPiece";
	[SerializeField] private GameObject mainParent;

	[SerializeField] private TextMeshPro textMesh;

	[SerializeField] private HintBubbleManager bubbleManager;
	[SerializeField] private SkyFragmentPiece skyPiece;
	[SerializeField] private bool isOpen;

	void Awake() {
		this.bubbleManager = GetComponent<HintBubbleManager> ();
		EventBroadcaster.Instance.AddObserver (EventNames.SHOW_PLAYER_CARRY, ShowHint);
		EventBroadcaster.Instance.AddObserver (EventNames.HIDE_PLAYER_CARRY, HideHint);
		EventBroadcaster.Instance.AddObserver (EventNames.HIDE_SIMPLE_PLAYER_CARRY, HideHintSimple);


		EventBroadcaster.Instance.AddObserver (EventNames.ON_PLAYER_GEM_ENTER, SkyOverlapUpdate);
		EventBroadcaster.Instance.AddObserver (EventNames.ON_PLAYER_LIFT_ENTER, LiftOverlapUpdate);
		this.isOpen = false;
	}

	void OnDestroy() {
		EventBroadcaster.Instance.RemoveObserver (EventNames.SHOW_PLAYER_CARRY);
		EventBroadcaster.Instance.RemoveObserver (EventNames.HIDE_PLAYER_CARRY);
		EventBroadcaster.Instance.RemoveObserver (EventNames.ON_PLAYER_GEM_ENTER);
		EventBroadcaster.Instance.RemoveObserver (EventNames.ON_PLAYER_LIFT_ENTER);
		EventBroadcaster.Instance.RemoveObserver (EventNames.HIDE_SIMPLE_PLAYER_CARRY);
	}

	public void SetSkyPiece(SkyFragmentPiece fragmentParent) {
		this.skyPiece = fragmentParent;
	}

	public SkyFragmentPiece GetSkyPiece() {
		if (this.skyPiece == null) {
			this.skyPiece = GetComponentInParent<SkyFragmentPiece> ();
		}
		return this.skyPiece;
	}
	public void HardClose() {
		this.GetBubbleManager ().HardClose ();
	}
	public void UpdateLabel (int numerator, int denominator) {
		this.GetTextMesh().text = numerator+"\n"+denominator;

	}

	void Update() {
		HandleFlip ();
	}

	public void HandleFlip() {
		this.gameObject.transform.localScale = new Vector3 (
			Mathf.Abs(gameObject.transform.localScale.x)*(this.ScaleSign()),
			gameObject.transform.localScale.y,
			gameObject.transform.localScale.z);
	}

	public void HideHintSimple() {
		// Will only check once (for initial)
//		if (this.isOpen) {
			this.GetBubbleManager().HideHintSimple();
//			Debug.Log ("<color=green>HIDE HINT SIMPLE</color>");
//		}
	}

	public void HideHint() {
		// Will only check once (for initial)
		if (this.isOpen) {
			this.GetBubbleManager().HideHint();
			Debug.Log ("<color=green>HIDE HINT</color>");
		}
	}

	// For player fragment label
	public void SkyOverlapUpdate(Parameters parameters) {
		SkyFragmentPiece overlappingPiece = parameters.GetObjectExtra (OVERLAPPING_PIECE) as SkyFragmentPiece;
		if (overlappingPiece != null) {
			this.UpdateLabel ((int)overlappingPiece.GetNumerator (), (int)overlappingPiece.GetDenominator ());
			this.ShowHint ();
		} else {
			this.HideHint ();
		}
	}
	public void LiftOverlapUpdate(Parameters parameters) {
		HollowBlock overlappingPiece = parameters.GetObjectExtra (OVERLAPPING_PIECE) as HollowBlock;
		if (overlappingPiece != null) {
			this.UpdateLabel ((int)overlappingPiece.GetNumerator (), (int)overlappingPiece.GetDenominator ());
			this.ShowHint ();
		} else {
			this.HideHint ();
		}
	}

//	public void CheckOverlappingPiece(Parameters parameters) {
//		SkyFragmentPiece overlappingPiece = parameters.GetObjectExtra (OVERLAPPING_PIECE) as SkyFragmentPiece;
//		// Hide if not overlapping piece
//		if (!this.SamePiece(this.GetSkyPiece(), overlappingPiece)) {
//			if (this.isOpen) {
//				this.GetBubbleManager().HideHint();
//			}
//		}
//	}

	public bool SamePiece(SkyFragmentPiece skyPiece, SkyFragmentPiece overlappingPiece) {
		if (overlappingPiece != null && skyPiece == overlappingPiece) {
			return true;
		} else {
			return false;
		}
	}

	public void ShowHint() {
		this.isOpen = true;
		this.GetBubbleManager().ShowHint();
	}

	public HintBubbleManager GetBubbleManager() {
		if (this.bubbleManager == null) {
			this.bubbleManager = GetComponent<HintBubbleManager> ();
		}
		return this.bubbleManager;
	}

	// Check if the parent has a negative/positive scale and mirror it to
	// negate the effects
	public int ScaleSign() {
		float sign = 1;
		if (GetMainParent ().transform.localScale.x < 0) {
			sign = -1;
		}
		return (int)sign;

	}

	public GameObject GetMainParent() {
//		return this.gameObject.transform.root.gameObject;
		if (this.mainParent == null) {
//			this.mainParent = GetComponentInParent<SkyFragmentPiece>().gameObject;
			this.mainParent = GetComponentInParent<PlayerYuni>().gameObject;

		}
		return this.mainParent;
	}

	public TextMeshPro GetTextMesh() {
		if (this.textMesh == null) {
			this.textMesh = GetComponentInChildren<TextMeshPro> ();
		}
		return this.textMesh;
	}
}
