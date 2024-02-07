using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HintBubbleManager : MonoBehaviour {


	private bool isActive = true;
	[SerializeField] private BubbleAnimatable bubbleAnimatable;
	private bool isUsingNeedle;
	private PlayerYuni player;

	void Awake () {
		this.bubbleAnimatable = GetComponent<BubbleAnimatable> ();
	}
	public PlayerYuni GetPlayer() {
		if (this.player == null) {
			this.player = GameObject.FindObjectOfType<PlayerYuni> ();
		}
		return this.player;
	}
	public void Deactivate() {
		this.isActive = false;
		this.HideHint ();
	}
	public void Activate() {
		this.isActive = true;
	}
	public bool IsUsingNeedle() {
		if (this.GetComponentInChildren<NeedleBubble> () != null) {
			this.isUsingNeedle = true;
		} else {
			this.isUsingNeedle = false;
		}
		return this.isUsingNeedle;
	}
	BubbleAnimatable GetBubbleAnimatable() {
		if (this.bubbleAnimatable == null) {
			this.bubbleAnimatable = GetComponent<BubbleAnimatable> ();
		}
		return this.bubbleAnimatable;
	}

	public void Open() {
		this.GetBubbleAnimatable ().Open ();
	}
	public void Close() {
		this.GetBubbleAnimatable ().Close ();
	}

	void OnTriggerEnter2D(Collider2D other) {
		if (other.gameObject.GetComponent<PlayerYuni> () != null) {
			if (this.isActive) {
				this.ShowHint ();
			}
		}
	}

	public void RemoveComponents() {
		Destroy (this.gameObject);
	}
	void OnTriggerExit2D(Collider2D other) {
		if (other.gameObject.GetComponent<PlayerYuni> () != null) {
			this.HideHint ();		
		}
	}
	void OnTriggerStay2D(Collider2D other) {
		if (other.gameObject.GetComponent<PlayerYuni> () != null) {
			if (this.isActive) {
				this.ShowHint ();
			} else {
				this.HideHint ();		
			}
		}
	}
	public void ShowHint() {
//		if (this.GetPlayer().GetPlayerAttack ().UsingHammer () && this.IsUsingNeedle() ||
//			(this.GetPlayer().GetPlayerAttack ().UsingNeedle () && !this.IsUsingNeedle())) {
		this.GetBubbleAnimatable ().Open ();

//		} else {
//			this.GetBubbleAnimatable ().Close ();
//		}

	}
	public void HardClose() {
		this.GetBubbleAnimatable ().HardClose ();
	}
	public void WakeUp() {
		this.Activate ();
		this.GetBubbleAnimatable ().WakeUp ();
	}
	public void Sleep() {
		this.Deactivate ();
		this.GetBubbleAnimatable ().Sleep ();
	}
	public void Observe() {
		this.GetBubbleAnimatable ().Observe ();
	}
	public void HideHint() {
		this.GetBubbleAnimatable ().Close ();
	}
	public void HideHintSimple() {
		this.GetBubbleAnimatable ().SimpleHide ();
	}
}
