using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ButtonAudible : Audible {
	[SerializeField] protected AudibleNames.Button id = AudibleNames.Button.BASIC;

	public AudibleNames.Button GetID() {
		return this.id;
	}
}
