using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Constants {
	public const string TAG_GROUND = "Ground";

	public static int GetGroundLayer() {
		return LayerMask.NameToLayer (TAG_GROUND);
	}
}
