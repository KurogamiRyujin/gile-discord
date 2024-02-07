using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PopcornAudible : Audible {
	[SerializeField] protected AudibleNames.Popcorn id = AudibleNames.Popcorn.WALK;

	public AudibleNames.Popcorn GetID() {
		return this.id;
	}
}
