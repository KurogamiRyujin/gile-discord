using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CloneBuilder {
	private static CloneBuilder instance = null;

	private CloneBuilder() {
	}

	public static CloneBuilder GetInstance() {
		if (instance == null) {
			instance = new CloneBuilder ();
		}

		return instance;
	}

	public GameObject SpawnClone(GameObject parent, Vector2 clonePosition, Color highlightColor, Color disabledColor, float scaleReduction) {
		GameObject holder = GameObject.Instantiate (parent, clonePosition, Quaternion.identity);
		holder.GetComponent<PartitionableObject> ().RemoveComponents ();
		holder.transform.SetParent (parent.transform);
//		holder.transform.SetParent(parent.gameObject.GetComponentInParent<Transform>());
		SpriteRenderer sprite = holder.GetComponent<SpriteRenderer> ();
		float opacity = 0.5f;
		Color color = new Color(sprite.color.r,sprite.color.g,sprite.color.b,opacity);
		sprite.color = color;

		holder.AddComponent<PartitionedClone> ();

//		holder.GetComponent<PartitionedClone> ().highlightColor = highlightColor; TODO: Remove highlightColor Parameter
		holder.GetComponent<PartitionedClone> ().highlightColor = new Color(0.078f, 1f, 0.2f);
		holder.GetComponent<PartitionedClone> ().disabledColor = disabledColor;
		holder.GetComponent<PartitionedClone> ().scaleReduction = scaleReduction;
		holder.SetActive(true);
		// TODO Remove
//		holder.GetComponent<PartitionableObject> ().partitionCount = parent.GetComponent<PartitionableObject> ().partitionCount;

		return holder;
	}
}
