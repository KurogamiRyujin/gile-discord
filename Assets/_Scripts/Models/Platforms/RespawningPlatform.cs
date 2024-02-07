using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RespawningPlatform : MonoBehaviour {

	public int respawnTimer = 2;
	public int collapseTimer = 5;

	public GameObject platformPrefab;
	private GameObject platformObject;
	private bool isSpawning;
	private bool isPlatformCollapsing;
	private PartitionableObject partitionableObject;

	// Use this for initialization
	void Start () {
		Spawn ();
		isSpawning = false;
		isPlatformCollapsing = false;
	}
	
	// Update is called once per frame
	void Update () {
		if (platformObject == null && !isSpawning) {
			isSpawning = true;
			isPlatformCollapsing = false;
			StartCoroutine (SpawnPlatform ());
		}

		if (platformObject != null && partitionableObject.IsTangible () && !isPlatformCollapsing) {
			isPlatformCollapsing = true;
			StartCoroutine (StartPlatformCollapse ());
		}
	}

	private IEnumerator SpawnPlatform() {
		for (int i = 0; i < respawnTimer; i++) {
			yield return new WaitForSeconds (1.0f);
		}

		isSpawning = false;
		Spawn ();
	}

	private IEnumerator StartPlatformCollapse() {
		for (int i = 0; i < collapseTimer; i++) {
			yield return new WaitForSeconds (1.0f);
		}
//		platformObject.AddComponent<DestroyOnFall> ();
		platformObject.GetComponent<Rigidbody2D> ().bodyType = RigidbodyType2D.Dynamic;
	}

	private void Spawn() {
		platformObject = Instantiate (platformPrefab, transform.position, Quaternion.identity);
		platformObject.transform.parent = transform;
		Debug.Log (platformObject.GetComponent<Rigidbody2D> ().bodyType);
		partitionableObject = platformObject.GetComponent<PartitionableObject> ();
	}
}
