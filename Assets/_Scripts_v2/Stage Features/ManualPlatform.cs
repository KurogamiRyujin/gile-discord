using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ManualPlatform : Interactable {
    
	[SerializeField] private HollowBlock hollowBlockKey;
	[SerializeField] private float speed = 1.0f;
	[SerializeField] private Transform targetPoint;
	[SerializeField] private bool deathBreak;
	[SerializeField] private bool isTarget; //at Target position
	[SerializeField] private bool isMoving; //at Target position
	[SerializeField] private bool isPlayingHint;
	[SerializeField] private Vector2 currentDestination;
	[SerializeField] private Dialogue hintDialogue;
    
	[SerializeField] private ManualPlatformImage manualPlatformImage;
    
	private Vector2 defaultPosition;
    
	void Awake() {
		this.isPlayingHint = false;
		EventBroadcaster.Instance.AddObserver (EventNames.ON_PLAYER_DEATH, this.OnPlayerDeath);
	}
    
	void OnDestroy() {
		EventBroadcaster.Instance.RemoveObserver (EventNames.ON_PLAYER_DEATH);
	}
    
	void Start() {
		if (this.hollowBlockKey != null) {
			this.hollowBlockKey.SetLiftable (false);
		}
	}
    
	public void OnPlayerDeath() {
		if (hollowBlockKey != null && this.deathBreak) {
			this.hollowBlockKey.DeathReturn ();
			this.ToDefault ();
//			this.Break ();
		}
	}
    
	public bool IsInteractable() {
		if (this.hollowBlockKey != null && this.hollowBlockKey.IsSolved () && !this.isMoving) {
			return true;
		} else {
			return false;
		}
	}
    
	public void Break() {
		this.hollowBlockKey.Break ();
	}

    void OnEnable () {
		this.defaultPosition = this.transform.position;
		this.currentDestination = this.defaultPosition;
	}
    
	void FixedUpdate () {
		Move (currentDestination);
	}
    
    public override void Interact() {
		if (this.hollowBlockKey != null && this.hollowBlockKey.IsSolved () &&
		    !isMoving) { // Can only interact if not already moving
			if (this.isTarget) { // If already at Target
				this.ToDefault ();
			} else {
				this.ToTarget ();
			}
		} else if(this.hollowBlockKey != null && !this.hollowBlockKey.IsSolved ()) {
			SoundManager.Instance.Play (AudibleNames.Trampoline.LOCK, false);
			HintPlayer ();
		}
	}
    
	public void HintPlayer() {
		if (!isPlayingHint) {
			this.hintDialogue.currentTextIndex = -1;
			if (GameController_v7.Instance.GetDialogueManager ().DisplayMinorHint (hintDialogue)) {
				this.isPlayingHint = true;
				StartCoroutine (HintCloseRoutine ());
			}
		}
	}
    
	IEnumerator HintCloseRoutine() {
		yield return new WaitForSeconds (5.0f);
		GameController_v7.Instance.GetDialogueManager ().CloseMinorHint ();
		this.isPlayingHint = false;
	}
    
	public ManualPlatformImage GetManualPlatformImage() {
		if(this.manualPlatformImage == null) {
			this.manualPlatformImage = GetComponentInChildren<ManualPlatformImage> ();
		}
		return this.manualPlatformImage;
	}
    
	public void ToTarget() {
		//		Move (targetPoint.position);
		this.isMoving = true;
		this.currentDestination = targetPoint.position;
		this.isTarget = true;
		this.GetManualPlatformImage ().ToTarget ();
	}
    
	public void ToDefault() {
		//		Move (defaultPosition);
		this.isMoving = true;
		this.currentDestination = defaultPosition;
		this.isTarget = false;
		this.GetManualPlatformImage ().ToDefault ();
	}
    
	private void Move (Vector2 target) {
		Vector2 pos = this.transform.position;

		if (pos != target) {
			float step = speed * Time.deltaTime;
			this.transform.position = Vector2.MoveTowards (this.transform.position, target, step);
		} else {
			this.isMoving = false;
		}
	}
    
	void OnTriggerStay2D(Collider2D other) {
		if (other.GetComponent<PlayerYuni>() != null) {
			Debug.Log ("MANUAL PLATFORM OVERLAP");
			this.GetPlayerYuni ().OverlapManualPlatform (this);

			#if UNITY_STANDALONE
			if (Input.GetButtonDown ("Fire2")) {
				Interact();
			}
			#endif
		}
	}
    
	void OnTriggerExit2D(Collider2D other) {
		if (other.GetComponent<PlayerYuni> () != null) {
			this.GetPlayerYuni ().LeaveManualPlatform (this);
		}
	}
}
