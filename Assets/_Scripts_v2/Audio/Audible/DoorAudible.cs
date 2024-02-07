using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DoorAudible : Audible {
	[SerializeField] protected AudibleNames.Door id = AudibleNames.Door.CLOSE;

	public AudibleNames.Door GetID() {
		return this.id;
	}
}
