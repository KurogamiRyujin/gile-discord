using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CloneObjectController : MonoBehaviour {

	public Transform partitionObject;
	List<GameObject> listPartitions;


	void Start() {
		// Partition according to string used
		gameObject.SetActive(false);
		//partitionObject.SetActive (false);
		partition();

	}

	void OnEnable() {
		partition();
	}

	void OnDisable() {
		
		destroyPartition();
		/*
		GameObject[] gameObjects = GameObject.FindGameObjectsWithTag ("Partition Instance");
		for (int i = 0; i < gameObjects.Length; i++) {
			Destroy(gameObjects[i]);
		}
		*/
	}

	public void destroyPartition() {
		foreach(GameObject temp in listPartitions) {
			Destroy(temp);
		}
	}
	public void partition() {
		listPartitions = new List<GameObject>();
		int partitionCount = 6;
		float width = gameObject.transform.localScale.x / partitionCount;
		float w = gameObject.GetComponent<SpriteRenderer> ().bounds.extents.x*2 / partitionCount;
		float height = gameObject.transform.localScale.y;
		float h = gameObject.GetComponent<SpriteRenderer> ().bounds.extents.y;
		Transform tempTransform;
//		GameObject temp;

		for(int x = 0; x < partitionCount; x++) {

			tempTransform = Instantiate(partitionObject, new Vector2(w, h), Quaternion.identity);
			//tempTransform.localPosition = new Vector3(transform.localPosition.x-width*x, height*y, tempTransform.position.z);
			tempTransform.localPosition = new Vector2(-gameObject.transform.localScale.x+(x*w),
				h);
			tempTransform.localScale = new Vector2(w, h);

			tempTransform.gameObject.SetActive(true);
			tempTransform.name = "partition_"+x;
			tempTransform.tag = "Partition Instance";
			tempTransform.transform.parent = gameObject.transform;
			listPartitions.Add(tempTransform.gameObject);
		}

		/*
		for(int y = 0; y < partitionCount; y++) {
			for(int x = 0; x < partitionCount; x++) {
				
				tempTransform = Instantiate(partitionObject, new Vector3(width, height, 0), Quaternion.identity);
				//tempTransform.localPosition = new Vector3(transform.localPosition.x-width*x, height*y, tempTransform.position.z);
				tempTransform.localPosition = new Vector3(-transform.localScale.x+(x*width),
															transform.localScale.y-(y*height), 0);
				tempTransform.localScale = new Vector3(width, height, tempTransform.localScale.z);

				tempTransform.gameObject.SetActive(true);
				tempTransform.name = "partition_"+x+y;
				tempTransform.tag = "Partition Instance";
				tempTransform.transform.parent = gameObject.transform;
				listPartitions.Add(tempTransform.gameObject);
			}
		}
		*/
	}

	void Update() {
	}

	void OnCollisionEnter2D(Collision2D other) {
		
	}
}
