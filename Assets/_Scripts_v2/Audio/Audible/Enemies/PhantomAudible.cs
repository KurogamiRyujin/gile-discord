using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PhantomAudible : Audible {
	[SerializeField] protected AudibleNames.Phantom id = AudibleNames.Phantom.DEATH;

	public AudibleNames.Phantom GetID() {
		return this.id;
	}
}
