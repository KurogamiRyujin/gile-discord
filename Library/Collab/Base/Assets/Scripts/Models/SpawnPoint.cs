using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpawnPoint : MonoBehaviour {

	public GameObject playerCharacter;
	public string comingFrom;
	public int spawnNumber;

	public void SpawnPlayer() {
		Debug.Log ("Spawning...");
		Debug.Log ("Instantiated");
		Instantiate (playerCharacter, this.gameObject.transform.position, Quaternion.identity);
//		playerCharacter.SetActive(true);
		/*if (playerCharacter == null) {
			Debug.Log ("Instantiated");
			playerCharacter = Instantiate (playerCharacter, this.gameObject.transform.position, Quaternion.identity);
		} else {
			Debug.Log ("Brought to Spawn Point coming from " + this.comingFrom);
			playerCharacter.transform.SetPositionAndRotation (this.gameObject.transform.position, Quaternion.identity);
		}*/
	}
}
