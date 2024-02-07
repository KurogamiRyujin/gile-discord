using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LCDInterfaceAudible : Audible {
	[SerializeField] protected AudibleNames.LCDInterface id = AudibleNames.LCDInterface.INCREASE;

	public AudibleNames.LCDInterface GetID() {
		return this.id;
	}
}
