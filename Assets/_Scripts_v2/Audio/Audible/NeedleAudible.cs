using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NeedleAudible : Audible {
	[SerializeField] protected AudibleNames.Needle id = AudibleNames.Needle.BASIC;

	public AudibleNames.Needle GetID() {
		return this.id;
	}
}
