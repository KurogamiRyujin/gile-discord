using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CloneBuilder_v2 {

	private static CloneBuilder_v2 sharedInstance = null;

	private CloneBuilder_v2() {
	}

	public static CloneBuilder_v2 Instance {
		get {
			if (sharedInstance == null) {
				sharedInstance = new CloneBuilder_v2();
			}

			return sharedInstance;
		}
	}

	public GameObject SpawnClone(GameObject parent, Vector2 clonePosition, Color highlightColor, Color disabledColor, float scaleReduction) {
		GameObject holder = GameObject.Instantiate (parent, clonePosition, Quaternion.identity);
		holder.GetComponent<PartitionableObject_v2> ().RemoveComponents ();
		holder.GetComponentInChildren<ClonePosition> ().RemoveComponents ();
		holder.GetComponentInChildren<UnderflowPosition> ().RemoveComponents ();
		holder.GetComponentInChildren<OverflowPosition> ().RemoveComponents ();
		holder.GetComponentInChildren<ResultsUI> ().RemoveComponents ();
		holder.GetComponentInChildren<HintBubbleManager> ().RemoveComponents ();

		holder.transform.SetParent (parent.transform);



		SpriteRenderer sprite = holder.GetComponent<SpriteRenderer> ();
//		float opacity = 0.5f;
//		Color color = new Color(sprite.color.r,sprite.color.g,sprite.color.b,opacity);
//		sprite.color = color;

		holder.AddComponent<PartitionedClone_v2> ();

//		holder.GetComponent<PartitionedClone_v2> ().highlightColor = new Color(0.078f, 1f, 0.2f);
		holder.GetComponent<PartitionedClone_v2> ().scaleReduction = scaleReduction;

		PartitionableObject_v2 original = parent.GetComponent<PartitionableObject_v2> ();
		holder.GetComponent<PartitionedClone_v2> ().SetUnderflowPosition (original.GetComponentInChildren<UnderflowPosition>().transform.position);
		holder.GetComponent<PartitionedClone_v2> ().SetOverflowPosition (original.GetComponentInChildren<OverflowPosition>().transform.position);
		holder.GetComponent<PartitionedClone_v2> ().SetOriginalPosition (original.transform.position);
		holder.GetComponent<PartitionedClone_v2> ().SetResultsUI (original.GetComponentInChildren<ResultsUI> ());


		holder.GetComponent<PartitionedClone_v2> ().highlightColor = original.cloneHighlightColor;
		holder.GetComponent<PartitionedClone_v2> ().disabledColor = original.GetDisabledColor();
		holder.GetComponent<PartitionedClone_v2> ().highlightOutlineColor = original.cloneHighlightOutlineColor;
		holder.GetComponent<PartitionedClone_v2> ().disabledOutlineColor = original.GetDisabledOutlineColor();

		holder.GetComponent<PartitionedClone_v2> ().filledColor = original.GetHighlightColor();
		holder.GetComponent<PartitionedClone_v2> ().filledOutlineColor = original.GetHighlightOutlineColor();
//		holder.GetComponent<PartitionedClone_v2> ().gameObject.GetComponentInChildren<HintBubbleManager> ().RemoveComponents ();
		holder.SetActive(true);

		return holder;
	}
}
