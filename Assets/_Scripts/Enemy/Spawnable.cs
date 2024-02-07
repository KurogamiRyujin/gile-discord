using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public interface Spawnable {
	void Spawn(Vector2 worldPosition);
	GameObject GetGameObject ();
}
