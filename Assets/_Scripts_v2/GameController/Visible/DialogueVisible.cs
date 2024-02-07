using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DialogueVisible : Visible {

	[SerializeField] protected string id = null;

	public string GetID() {
		return this.id;
	}
}
