using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public class PartitionablePlatform : PartitionableObject {

	void OnEnable() {
		partitions = new List<GameObject> ();

		Intangible ();
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
		this.isTangible = false;
		this.disabledTransparency ();
		this.objectCollider.isTrigger = true;
		this.rigidBody2d.bodyType = RigidbodyType2D.Kinematic;
	}

	protected override void Solved () {
		this.isTangible = true;
		this.isAnimatingFill = false;
		gameObject.GetComponent<SpriteRenderer> ().sprite = tangibleSprite;
		this.objectCollider.isTrigger = false;
		this.rigidBody2d.bodyType = RigidbodyType2D.Static;

		entry.timeSolved = System.DateTime.Now;
	}

	public void Intangible() {
		InitiateFraction ();

		Init ();
	}

	public void Purge() {
		Destroy (this.gameObject);
	}
}
