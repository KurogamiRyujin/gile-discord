using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ElevatorEnemySpawner : MonoBehaviour {

	[SerializeField] List<EnemySpawner> spawnPointsLevel1;
	[SerializeField] List<EnemySpawner> spawnPointsLevel2;
	[SerializeField] List<EnemySpawner> spawnPointsLevel3;
	[SerializeField] List<EnemySpawner> spawnPointsLevel4;
	List<GameObject> activeEnemies;

	// Use this for initialization
	void Start () {
		activeEnemies = new List<GameObject> ();
	}
	
	// Update is called once per frame
	void FixedUpdate () {
		if(activeEnemies.Count != 0) {
			foreach(GameObject enemy in activeEnemies) {
				if(!enemy.GetComponent<EnemyHealth>().isAlive) {
					activeEnemies.Remove (enemy);
				}
			}
		}
	}

	public void EnableSpawnPoints(int level) {
		
		switch (level) {
		case 1:
			for (int i = 0; i < spawnPointsLevel1.Count; i++) {
				activeEnemies.Add(spawnPointsLevel1 [i].SpawnEnemy (EnemyList.EnemyType.POPCORN, spawnPointsLevel1 [i].transform.position));
			}
			Debug.Log ("Spawning enemy for level 1");
			break;
		case 2:
			for (int i = 0; i < spawnPointsLevel2.Count; i++) {
				activeEnemies.Add(spawnPointsLevel2 [i].SpawnEnemy (EnemyList.EnemyType.POPCORN, spawnPointsLevel2 [i].transform.position));
			}
			Debug.Log ("Spawning enemy for level 2");
			break;
		case 3:
			for (int i = 0; i < spawnPointsLevel3.Count; i++) {
				activeEnemies.Add(spawnPointsLevel3 [i].SpawnEnemy (EnemyList.EnemyType.POPCORN, spawnPointsLevel3 [i].transform.position));
			}
			Debug.Log ("Spawning enemy for level 3");
			break;
		case 4:
			for (int i = 0; i < spawnPointsLevel4.Count; i++) {
				activeEnemies.Add(spawnPointsLevel4 [i].SpawnEnemy (EnemyList.EnemyType.POPCORN, spawnPointsLevel4 [i].transform.position));
			}
			Debug.Log ("Spawning enemy for level 4");
			break;
		
		default:
			Debug.Log ("Invalid level number. Out of bounds.");
			break;
		}
	}

	public int GetNumberOfActiveEnemies() {
		return activeEnemies.Count;
	}
}
