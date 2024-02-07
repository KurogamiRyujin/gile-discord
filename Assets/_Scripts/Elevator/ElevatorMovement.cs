using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ElevatorMovement : MonoBehaviour {

	[SerializeField] Transform[] positions;
	[SerializeField] Transform elevatorTransform;
	[SerializeField] ElevatorEnemySpawner enemySpawner;
	ElevatorAnimatable elevator;
	Transform currentPos;
	int currentIndex;
	float speed;
	bool movingUp, movingDown;
	bool isClosed;
	bool spawned;

	// Use this for initialization
	void Start () {
//		elevator = GameObject.FindGameObjectWithTag ("Elevator").GetComponent<ElevatorAnimatable> ();
		elevator = gameObject.GetComponentInChildren<ElevatorAnimatable>();
		currentIndex = 0;
		speed = 2f;
		movingDown = false;
		movingUp = true;
		isClosed = true;
		spawned = false;
	}
	
	// Update is called once per frame
	void Update () {
		if (Input.GetKeyDown (KeyCode.X) && enemySpawner.GetNumberOfActiveEnemies () != null) {
			if(enemySpawner.GetNumberOfActiveEnemies() == 0)
				ChangeDestination ();
		}

		Move ();
	}
		
	void Move() {
		elevatorTransform.localPosition = Vector2.MoveTowards (elevatorTransform.localPosition, positions [currentIndex].localPosition, speed * Time.deltaTime);

		if (elevatorTransform.localPosition == positions [currentIndex].localPosition) {
			if (isClosed) {
				elevator.OpenBars ();
				isClosed = false;
			}
			//spawn enemies
			if (!spawned) {
				spawned = true;
				enemySpawner.EnableSpawnPoints (currentIndex);
				FractionsReference.Instance ().UpdateFractionsRange ();
			}
//			Debug.Log ("Arrived");
		}
	}

	//change destination once enemies on that level are all dead
	public void ChangeDestination() {
		spawned = false;
		if (!isClosed) {
			elevator.CloseBars ();
			isClosed = true;
		}

		//move up
		if (currentIndex + 1 < positions.Length && movingUp) {
			currentIndex++;
		}

		//move down
		if (currentIndex - 1 > -1 && movingDown) {
			currentIndex--;
		}

		//toggle boolean flags
		if (currentIndex == positions.Length - 1) {
			movingUp = false;
			movingDown = true;
		} else if(currentIndex == 0) {
			movingUp = true;
			movingDown = false;
		}
	}
}
