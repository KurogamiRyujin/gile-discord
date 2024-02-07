using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TrampolineAudible : Audible {
	[SerializeField] protected AudibleNames.Trampoline id = AudibleNames.Trampoline.BOUNCE;

	public AudibleNames.Trampoline GetID() {
		return this.id;
	}
}
