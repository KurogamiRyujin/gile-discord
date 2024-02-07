using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LCDSegmentCollider : SegmentCollider {


	void OnTriggerEnter2D(Collider2D other) {
		if (other.gameObject.CompareTag ("LCD Answer Pointer")) {
			PointerLabel fraction = other.gameObject.GetComponent<PointerLabel> ();
			fraction.SetValue (this.numerator, this.denominator);
		}
	}
}
