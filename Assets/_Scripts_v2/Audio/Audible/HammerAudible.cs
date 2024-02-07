using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HammerAudible : Audible {
	[SerializeField] protected AudibleNames.Hammer id = AudibleNames.Hammer.BASIC;

	public AudibleNames.Hammer GetID() {
		return this.id;
	}
}
