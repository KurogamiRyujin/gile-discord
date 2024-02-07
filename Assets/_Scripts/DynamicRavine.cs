using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DynamicRavine : MonoBehaviour {
	
	[SerializeField] private int ravineWidth = 1;

	private List<HollowBlock> blocks;
	private EdgeCollider2D edgeCollider;

	// Use this for initialization
	void Awake () {
		blocks = new List<HollowBlock> ();
		edgeCollider = GetComponent<EdgeCollider2D> ();
		if (edgeCollider != null)
			Debug.Log ("Not NUll");
	}

	void FixedUpdate() {
		Vector2 point = new Vector2 (ravineWidth, 0.0f);
		Vector2 origin = Vector2.zero;
		Vector2[] points = new Vector2[2];

		points [0] = origin;
		points [1] = point;

		edgeCollider.points = points;

		MaintainBlocksHeight ();
	}

	void OnTriggerEnter2D(Collider2D other) {
		if (other.gameObject.CompareTag ("HollowBlock") && this.blocks.Count < ravineWidth) {
			HollowBlock block = other.gameObject.GetComponent<HollowBlock> ();
			if(!this.blocks.Contains(block))
				this.blocks.Add (block);

			Debug.Log ("Hit");
		}
	}

	private void CheckFallingBlocks() {
		RaycastHit2D hit;

		Vector3 pos2 = new Vector3 (this.transform.position.x + ravineWidth, this.transform.position.y);

		hit = Physics2D.Raycast (this.transform.position, pos2);

		Debug.DrawLine (this.transform.position, pos2);

		Debug.Log ("BLock Count: " + this.blocks.Count);

		if (hit.collider.gameObject.CompareTag ("HollowBlock") && this.blocks.Count < ravineWidth) {
			HollowBlock block = hit.collider.gameObject.GetComponent<HollowBlock> ();
			this.blocks.Add (block);

			Debug.Log ("Hit");
		}
	}

	private void MaintainBlocksHeight() {
		foreach (HollowBlock block in this.blocks) {
			Vector3 pos = new Vector3 (this.transform.position.x + 0.5f + ((blocks.Count-1) * block.GetWidthWhole ()), this.transform.position.y, 0.0f);

			block.transform.rotation = Quaternion.Euler (Vector3.zero);
			block.transform.position = pos;
		}
	}
}
