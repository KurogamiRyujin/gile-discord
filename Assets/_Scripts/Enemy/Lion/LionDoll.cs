using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LionDoll : MonoBehaviour {

	[SerializeField] private GameObject lion;
	private int direction = 1;

	// Use this for initialization
	void Start () {
		direction *= (int) (transform.localScale.x / Mathf.Abs (transform.localScale.x));
		StartCoroutine (InitiateTransformationCountdown (3));
	}

	void Update() {
		direction *= (int) (transform.localScale.x / Mathf.Abs (transform.localScale.x));
	}

	public void FaceOtherDirection() {
		Vector2 scale = new Vector2 (-transform.localScale.x, transform.localScale.y);
		transform.localScale = scale;
	}
	
	private IEnumerator InitiateTransformationCountdown(int seconds) {
		for (int i = 0; i < seconds; i++)
			yield return new WaitForSeconds (1.0f);

		TransformToLion ();
	}

	private void TransformToLion() {
		GameObject temp = Instantiate (lion, this.gameObject.transform.position, Quaternion.identity);
		temp.transform.localScale = new Vector2 (temp.transform.localScale.x * direction, temp.transform.localScale.y);
		Destroy (this.gameObject);
	}
}
