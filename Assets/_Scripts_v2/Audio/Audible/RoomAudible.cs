using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RoomAudible : Audible {
	[SerializeField] protected AudibleNames.Room id = AudibleNames.Room.BREAKING;

	public AudibleNames.Room GetID() {
		return this.id;
	}
}
