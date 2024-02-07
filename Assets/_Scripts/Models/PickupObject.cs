using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PickupObject : MonoBehaviour {

	[SerializeField] private Interactable pickupPrefab;
//	[HideInInspector] public GameObject pickup;
	private Interactable pickup;
	[SerializeField] private PlayerYuni player;

	[SerializeField] float offsetX = 0.7f;
	[SerializeField] float offsetY = 15.7f;
	[SerializeField] Transform pickupPos;

	[SerializeField] HintBubbleManager hintItem;
	[SerializeField] ParticleSystem effectPickup;
	[SerializeField]private Animator pedestalAnimator;
	private bool playerInVicinity = false;
	private bool isTaken = false;

	// Use this for initialization
	void Awake () {
//		EventBroadcaster.Instance.AddObserver (EventNames.ON_CHARGING, this.Pickup);
	}

//	void OnDestroy() {
//		EventBroadcaster.Instance.RemoveObserver (EventNames.ON_CHARGING);
//	}

	void Start() {
		this.isTaken = true; // Set to true in order to enter Drop()
		this.Drop ();
	}

	public ParticleSystem GetEffect() {
		if (this.effectPickup == null) {
			this.GetComponentInChildren<ParticleSystem> ();
		}
		return this.effectPickup;
	}
	public bool IsTaken() {
		return this.isTaken;
	}
	void OnTriggerEnter2D(Collider2D other) {
		if (other.gameObject.GetComponent<PlayerYuni>() != null) {
			playerInVicinity = true;
			player = other.gameObject.GetComponent<PlayerYuni> ();
			this.GetHintItem ().Open ();
			this.GetPlayer ().SetPickup (this);
		} else
			playerInVicinity = false;
	}

	void OnTriggerStay2D(Collider2D other) {
		if (other.gameObject.GetComponent<PlayerYuni>() != null) {
			playerInVicinity = true;
			player = other.gameObject.GetComponent<PlayerYuni> ();
			this.GetPlayer ().SetPickup (this);
		} else
			playerInVicinity = false;
	}

	void OnTriggerExit2D(Collider2D other) {
		if (other.gameObject.GetComponent<PlayerYuni>() != null) {
			playerInVicinity = false;
			this.GetPlayer ().ResetPickup (this);
			this.GetHintItem ().Close ();
		}
	}

	public PlayerYuni GetPlayer() {
		if (this.player == null) {
			this.player = FindObjectOfType<PlayerYuni> ();
		}
		return this.player;
	}

	public void Pickup() {
		if (playerInVicinity) {
			this.Take ();
		}
	}

	public void Drop() {
		if (isTaken) {
			pickup = Interactable.Instantiate (pickupPrefab, pickupPos.position, Quaternion.identity);
			pickup.gameObject.transform.parent = this.gameObject.transform;
			pickup.gameObject.AddComponent<Levitation> ();

			this.isTaken = false;
			this.GetPedestalAnimator ().SetBool ("isTaken", this.isTaken);
			this.HintObserve ();
			if (this.GetEffect () != null) {
				this.GetEffect ().Play ();
			}
		}
	}

	public Animator GetPedestalAnimator () {
		if (this.pedestalAnimator == null) {
			this.pedestalAnimator = GetComponentInChildren<Animator> ();
		}
		return this.pedestalAnimator;
	}

	public void Update() {
		#if UNITY_STANDALONE
		if(Input.GetButtonDown ("Fire2"))
			this.Pickup ();
		#endif
	}

	public void Take() {
		if (!isTaken) {
			Debug.Log ("| TAKE |");
			this.isTaken = true;
			this.GetPedestalAnimator ().SetBool ("isTaken", this.isTaken);

			this.GetEffect ().Stop ();
			this.HintSleep ();

			this.pickup.SetPlayer (this.GetPlayer ());
			this.pickup.Interact ();
			SoundManager.Instance.Play (AudibleNames.LCDInterface.DECREASE, false);
		}
	}


	// Call this if you want the hint to pop up
	void HintObserve() {
		if (this.GetHintItem () != null) {
			this.GetHintItem ().Observe ();
		}
	}

	// Call this if you don't want the hint to pop up anymore
	void HintSleep() {
		if (this.GetHintItem () != null) {
			Debug.Log ("| SLEEP |");
			this.GetHintItem ().Sleep ();
		}
	}

	public HintBubbleManager GetHintItem() {
		if (this.hintItem == null) {
			this.hintItem = GetComponentInChildren<HintBubbleManager> ();
		}
		return this.hintItem;
	}
}
