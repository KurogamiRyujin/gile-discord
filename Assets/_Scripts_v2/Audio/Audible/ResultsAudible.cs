using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ResultsAudible : Audible {
	[SerializeField] protected AudibleNames.Results id = AudibleNames.Results.SUCCESS;

	public AudibleNames.Results GetID() {
		return this.id;
	}
}
