using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PartitionableObjectMarker : MonoBehaviour {

	private float numerator;
	private float denominator;

	private Vector2 uiPosition;
	private float spriteXExtents, spriteYExtents;

	public GameObject fractionPrefab;
	private GameObject fraction;

	private TextMesh numeratorTxt, denominatorTxt, bar;

	// Use this for initialization
	void Awake () {
		numerator = 0;
		denominator = 0;

		SpriteRenderer sprite = gameObject.GetComponent<SpriteRenderer> ();
		spriteXExtents = sprite.bounds.extents.x;
		spriteYExtents = sprite.bounds.extents.y;

		float offsetX = 0.15f;
		float offsetY = 0.2f;
		uiPosition = new Vector2 (gameObject.transform.position.x + spriteXExtents + offsetX, gameObject.transform.position.y + spriteYExtents + offsetY);

		foreach (Transform child in gameObject.GetComponentsInChildren<Transform>(true)) {
			if (child.gameObject.CompareTag ("FractionLabel"))
				fraction = child.gameObject;
		}

		if (fraction == null) {
			fraction = Instantiate (fractionPrefab, uiPosition, Quaternion.identity);
			fraction.transform.SetParent (this.gameObject.transform);
		}

		TextMesh[] textMeshes = fraction.GetComponentsInChildren<TextMesh> ();

		foreach (TextMesh txtmesh in textMeshes) {
			txtmesh.gameObject.GetComponent<MeshRenderer> ().sortingLayerName = "Hollow Label";
			if (txtmesh.gameObject.CompareTag ("FractionLabelNumerator"))
				numeratorTxt = txtmesh;
			if (txtmesh.gameObject.CompareTag ("FractionLabelDenominator"))
				denominatorTxt = txtmesh;
		}

		numeratorTxt.text = numerator.ToString ();
		denominatorTxt.text = denominator.ToString ();
	}

	public void Enable() {
		this.fraction.SetActive (true);
	}

	public void Disable() {
		this.fraction.SetActive (false);
	}
	
	// Update is called once per frame
	void Update () {
		if (this.fraction.activeInHierarchy) {
			numeratorTxt.text = numerator.ToString ();
			denominatorTxt.text = denominator.ToString ();
		}
	}

	public void UpdateValue(float numerator, float denominator) {
		this.numerator = numerator;
		this.denominator = denominator;
	}

	public float GetValue() {
		return (this.numerator / this.denominator);
	}
}
