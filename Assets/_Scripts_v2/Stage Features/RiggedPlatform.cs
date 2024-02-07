using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RiggedPlatform : Triggerable {
    
	private Renderer visibility;
//	private Collider2D tangibility;
	private Collider2D[] tangibility;
    
	void Awake() {
		this.visibility = GetComponent<Renderer> ();
		this.tangibility = GetComponents<Collider2D> ();
	}
    
	public override void Trigger() {
		this.visibility.enabled = true;
		if (this.tangibility != null) {
			foreach (Collider2D collider2D in this.tangibility) {
				collider2D.enabled = true;
			}
		}
	}
    
	public override void Disarm() {
		this.visibility.enabled = false;
		if (this.tangibility != null) {
			foreach (Collider2D collider2D in this.tangibility) {
				collider2D.enabled = false;
			}
		}
	}
}
