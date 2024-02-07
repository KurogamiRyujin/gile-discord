using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class YuniAudible : Audible {
	[SerializeField] protected AudibleNames.Yuni id = AudibleNames.Yuni.BASIC;

	public AudibleNames.Yuni GetID() {
		return this.id;
	}
}
