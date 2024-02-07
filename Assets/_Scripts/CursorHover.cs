using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CursorHover : MonoBehaviour {
	private string targetableTag = "PartitionableObject";
	private string targetableLayer = "SkyBlock";
	public Texture2D defaultTexture;
	public Texture2D partitionableObjectTexture;
	public CursorMode cursorMode = CursorMode.Auto;
	public Vector2 hotSpot = Vector2.zero;

	private Ray2D ray;
	private RaycastHit2D hit;

	void Awake() {
//		#if UNITY_ANDROID
//		Destroy(gameObject);
//		#endif
	}

	void Start() {
		hotSpot = this.getHotSpot (defaultTexture);
		Cursor.SetCursor (defaultTexture, hotSpot, cursorMode);
		Physics.queriesHitTriggers = true;
	}


	Vector2 getHotSpot(Texture2D texture) {
		return new Vector2 (defaultTexture.width/2, defaultTexture.height/2);
	}

	void OnMouseEnter() {
//		hit = Physics2D.Raycast (Camera.main.ScreenToWorldPoint (Input.mousePosition), Vector2.zero);
//		Debug.Log ("RAY ENTER");
//		if(hit != null) {
//			Debug.Log ("HITS "+hit.collider.gameObject.layer);
//			if (hit.collider.gameObject.layer == LayerMask.NameToLayer (this.targetableLayer)) {
//				Debug.Log ("Entered hotspot");
//				hotSpot = this.getHotSpot (partitionableObjectTexture);
//				Cursor.SetCursor (partitionableObjectTexture, hotSpot, cursorMode);
//			}
//		}

		if (gameObject.CompareTag (this.targetableTag) ||
			gameObject.layer == LayerMask.NameToLayer(this.targetableLayer)) {
			hotSpot = this.getHotSpot (partitionableObjectTexture);
			Cursor.SetCursor (partitionableObjectTexture, hotSpot, cursorMode);
		}
	}

	void OnMouseExit() {
		hotSpot = this.getHotSpot (defaultTexture);
		Cursor.SetCursor (defaultTexture, hotSpot, cursorMode);
	}
}
