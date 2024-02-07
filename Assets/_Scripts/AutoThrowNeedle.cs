using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class AutoThrowNeedle : MonoBehaviour {
	[SerializeField] bool throwing;
	GameObject box;

	void Start() {
		
	}

	void Update() {
		if (throwing) {
			StartCoroutine(Shoot());
			StopAllCoroutines ();
			Debug.Log ("Finished");
			throwing = false;
		}
	}

	IEnumerator Shoot() {
		box = GameObject.FindGameObjectWithTag("PartitionableObject");
		Debug.Log ("Box position: " + box.transform.position);
		GameObject.FindGameObjectWithTag ("Player").GetComponent<PlayerAttack> ().Attack1 (box.transform.position);
		yield return null;
//		Ray ray = Camera.main.ScreenPointToRay(box.transform.position);
//					RaycastHit hit;
//					if (Physics.Raycast (ray, out hit)) {
//			Debug.DrawLine (ray.origin, hit.point, Color.red, 5.0f);
//					}
//		var pointer = new PointerEventData (EventSystem.current);
//		ExecuteEvents.Execute (box.gameObject, pointer, ExecuteEvents.pointerEnterHandler);
//		ExecuteEvents.Execute (box.gameObject, pointer, ExecuteEvents.pointerDownHandler);
//
//		yield return new WaitForSeconds (3.0f);
//
//		ExecuteEvents.Execute (box.gameObject, pointer, ExecuteEvents.pointerUpHandler);
//		ExecuteEvents.Execute (box.gameObject, pointer, ExecuteEvents.pointerClickHandler);
	}


}
