using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class DummyPedagogicalComponent : AbstractPedagogicalComponent {

//	[SerializeField] private bool updateDenominators = false;

	void Awake () {
		UpdateValidDenominators ();
		SceneManager.activeSceneChanged += ActiveSceneChanged;
		SceneManager.sceneUnloaded += OnSceneUnload;
	}

	void OnDestroy () {
		SceneManager.activeSceneChanged -= ActiveSceneChanged;
		SceneManager.sceneUnloaded -= OnSceneUnload;
	}

	// Use this for initialization
	void Start () {
//		Random.InitState (123156);
//		UpdateValidDenominators ();
//		updateDenominators = false;
	}

//	void Update() {
//		if (updateDenominators)
//			UpdateValidDenominators ();
//	}

	private void ActiveSceneChanged (Scene scene1, Scene scene2) {
		UpdateValidDenominators ();
	}

	private void OnSceneUnload(Scene scene) {
		FractionsReference.Instance ().ClearEnemyDenominators ();
	}

	protected override void UpdateValidDenominators() {
		Debug.Log ("Updated Denominators");
		FractionsReference.Instance ().UpdateValidDenominators (this.validDenominators);
		FractionsReference.Instance ().UpdateFractionsRange ();
	}
}
