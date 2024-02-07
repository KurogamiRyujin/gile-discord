using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BreakableBoxAudible : Audible {
	[SerializeField] protected AudibleNames.BreakableBox id = AudibleNames.BreakableBox.BREAK;

	public AudibleNames.BreakableBox GetID() {
		return this.id;
	}
}
