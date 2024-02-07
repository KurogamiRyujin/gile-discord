using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Thread_Controller : MonoBehaviour {

	public GameObject threadSource;
	public GameObject threadEnd;

	private SpringJoint2D thread;
	public int maxThreadFrameCount;
	private int threadFrameCount;
	public LineRenderer lineRenderer;

	// Update is called once per frame
	void Update () {

	}

	void setThread() {
		Vector3 destinationPosition = threadEnd.transform.position;
		Vector3 originPosition = threadSource.transform.position;
		Vector3 direction = destinationPosition - originPosition;

		RaycastHit2D hit = Physics2D.Raycast (originPosition, direction, Mathf.Infinity);

		if (hit.collider != null) {
			SpringJoint2D newThread = threadSource.AddComponent<SpringJoint2D> ();

			newThread.enableCollision = false;
			newThread.frequency = .2f;
			newThread.connectedAnchor = hit.point;
			newThread.enabled = true;
		}
	}
}
