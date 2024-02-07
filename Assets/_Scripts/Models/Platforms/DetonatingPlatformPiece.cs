using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public class DetonatingPlatformPiece : PartitionableObject_v2 {
//	void Start() {
//		this.enabledTransparency ();
//		this.GetHintBubbleManager ().Deactivate ();
//		Debug.Log ("enabled transparency DEACTIVATE from detonating");
//	}
	void OnEnable() {
		partitions = new List<GameObject> ();
		InitiateFraction ();

		Init ();
	}

	public override void Fill (float fillCount, float totalFill) {
		Debug.Log ("Filled Count: " + filledCount);
		Debug.Log ("Partition Count: " + partitionCount);
		Debug.Log ("Fill Count: " + fillCount);
		Debug.Log ("Total Fill: " + totalFill);
		Debug.Log (((filledCount / partitionCount) + (fillCount / totalFill)));
		Debug.Log ("Epsilon: " + Mathf.Epsilon);

		if(entry.objectGiven == null)
			entry.objectGiven = new Tuple<int, int>((int)filledCount, (int)partitionCount);
		if(entry.actualAnswer == null)
			entry.actualAnswer = new Tuple<int, int>((int)partitionCount - (int)filledCount, (int)partitionCount);
		if(entry.attemptedAnswers == null)
			entry.attemptedAnswers = new List<Tuple<int, int>>();
		entry.attemptedAnswers.Add(new Tuple<int, int>((int)fillCount, (int)totalFill));
		entry.numberOfAttempts++;

		float numerator1 = filledCount;
		float denominator1 = partitionCount;
		float numerator2 = fillCount;
		float denominator2 = totalFill;

		print (numerator1);
		print (denominator1);

		print (numerator2);
		print (denominator2);

		if (denominator1 != denominator2) {
			numerator1 *= denominator2;
			denominator1 *= denominator2;
			numerator2 *= denominator1;
			denominator2 *= denominator1;
		}

		if (Mathf.Abs (((numerator1/denominator1) + (numerator2/denominator2)) - 1)  == 0) {
			for (int i = 0; i < fillCount; i++) {
				this.increasePartition ();
			}

			Solved ();
		} else {
			// Will we implement the animation that causes individual pieces to go into the object individually and melt when not whole?
			Debug.Log("Failed to fill...");
		}

		DataManager.UpdatePOEntry(DataManager.GetPOLastKey(), entry);

		this.enabledTransparency ();
	}

	protected void Init() {
		this.isTangible = true;
		this.isAnimatingFill = false;
//		gameObject.GetComponent<SpriteRenderer> ().sprite = tangibleSprite;

//		SpriteRenderer image = gameObject.GetComponentInChildren<SpriteRenderer> ();
//		image.color = new Color(image.color.r, image.color.g, image.color.b, 1.0f);
		this.objectCollider.isTrigger = false;
		this.rigidBody2d.bodyType = RigidbodyType2D.Static;

		this.enabledTransparency ();
//		this.GetComponentInChildren<HintBubbleManager> ().gameObject.SetActive (false);
	}

	public override void Solved () {
		Init ();
		entry.timeSolved = System.DateTime.Now;
	}

	public void Replenish() {
		Init ();
		this.GetComponentInChildren<HintBubbleManager> ().Deactivate ();
	}

	public void Detonate() {
		InitiateFraction ();

		this.isTangible = false;
		this.disabledTransparency ();
		this.objectCollider.isTrigger = true;
		this.rigidBody2d.bodyType = RigidbodyType2D.Kinematic;
//		SpriteRenderer image = gameObject.GetComponentInChildren<SpriteRenderer> ();

//		this.GetHintBubbleManager ().Activate ();

//		this.GetComponentInChildren<HintBubbleManager> ().Activate ();

//		this.GetComponentInChildren<HintBubbleManager> ().gameObject.SetActive(true);
//		this.disabledTransparency ();
//		image.color = new Color(image.color.r, image.color.g, image.color.b, 0.0f);
	}

	public void Purge() {
		Destroy (this.gameObject);
	}
}
