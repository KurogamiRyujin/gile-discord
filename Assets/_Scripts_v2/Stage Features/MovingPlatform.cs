using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MovingPlatform : MonoBehaviour {
    
	[SerializeField] private HollowBlock hollowBlockKey;
	[SerializeField] private float speed = 1.0f;
	[SerializeField] private Transform targetPoint;

    // Just check one. If more than one is checked, it follows hierarchy.
    [SerializeField] private bool deathBreak;
	[SerializeField] private bool deathFix;
    
	[SerializeField] private bool moveOnHollow;
    
	private Vector2 defaultPosition;
    
	void Awake() {
		EventBroadcaster.Instance.AddObserver (EventNames.ON_PLAYER_DEATH, this.OnPlayerDeath);

		// If will prefill, Hollow Block should have SetPiecesReturnToSkyBlock to true.
		if (this.deathFix) {
			if (this.hollowBlockKey != null) {
				this.hollowBlockKey.SetPiecesReturnToSkyBlock (true);
			}
		}
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
		if (hollowBlockKey != null) {
			if (this.deathBreak) {
				this.hollowBlockKey.DeathReturn ();
				//			this.Break ();
			}
			else if (this.deathFix) {
				this.hollowBlockKey.DeathPrefill ();
			}
		}
	}
    
	public void Break() {
		this.hollowBlockKey.Break ();
	}
    
	void OnEnable () {
		this.defaultPosition = this.transform.position;
	}
    
    void Update () {
		if (this.hollowBlockKey != null)
			Debug.Log ("IsSolved is " + this.hollowBlockKey.IsSolved ());


		// Will move to target when hollow. The reverse of the original implementation
		if (moveOnHollow) {
			if (hollowBlockKey.IsSolved ()) {
				Move (defaultPosition);
			}
			else {
				Move (targetPoint.position);
			}
		}
		// Else the original implementation. Will move to target on fill.
		else {
			if (hollowBlockKey.IsSolved ()) {
				Move (targetPoint.position);
			}
			else {
				Move (defaultPosition);
			}
		}
	}
    
	private void Move (Vector2 target) {
		Vector2 pos = this.transform.position;

		if (pos != target) {
			float step = speed * Time.deltaTime;

			this.transform.position = Vector2.MoveTowards (this.transform.position, target, step);
		}
	}
}
