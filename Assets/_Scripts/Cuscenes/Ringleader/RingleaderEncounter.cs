using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// Cutscene behaviour for the Final Boss, Ringleader, Encounter.
/// </summary>
public class RingleaderEncounter : Cutscene {
	[SerializeField] private Dialogue dialogue;
	[SerializeField] private List<GameObject> gambits;
	[SerializeField] private List<EnemyHealth> ringleaders;
	[SerializeField] private Transform spawnPoint;
	[SerializeField] private ParticleEffect particleEffect;

//	private PlayerAttack playerAttack;
//	private PlayerMovement playerMovement;

	private RingleaderAttack currentGambit;
	private RingleaderMovement currentRingleaderMovement;
	private EnemyHealth currentRingleaderHealth;

	private bool isInDialogue = false;
	private PlayerYuni player;
	// Use this for initialization
	void Awake () {
		Init ();
	}

	protected override void Init () {
		currentGambit = null;
		currentRingleaderMovement = null;
		currentRingleaderHealth = null;
		player = null;
//		playerAttack = null;
//		playerMovement = null;
		foreach (EnemyHealth ringleader in ringleaders) {
			ringleader.gameObject.GetComponent<RingleaderAttack> ().enabled = false;
			ringleader.gameObject.GetComponent<RingleaderMovement> ().canMove = false;
			ringleader.gameObject.GetComponent<EnemyHealth> ().enabled = false;
		}

		base.Init ();
	}

	void Start() {

		if (particleEffect == null) {
			particleEffect = GameObject.FindObjectOfType<ParticleEffect> ();
		}
	}

	void OnTriggerEnter2D(Collider2D other) {
//		if (other.gameObject.CompareTag ("Player")) {
//			playerAttack = other.gameObject.GetComponent<PlayerAttack> ();
//			playerMovement = other.gameObject.GetComponent<PlayerMovement> ();
//
//			PlayScenes ();
//		}
		if (other.gameObject.GetComponent<PlayerYuni> () != null) {
			this.player = other.gameObject.GetComponent<PlayerYuni> ();
			PlayScenes ();
		}
	}

	protected override void disablePlayerControls () {
		ToggleMobileUI (false);
//		playerMovement.canMove (false);
//		playerAttack.canAttack (false);
		player.GetPlayerMovement().canMove(false);
		player.GetPlayerAttack ().canAttack (false);
	}

	private void EnableDialogue(string id) {
		Debug.Log ("Passed id "+id);
		player.GetPlayerMovement ().isInDialogue ();
//		playerMovement.isInDialogue ();
//		dialogueTrigger.TriggerDialogue ();
		GameController_v7.Instance.GetDialogueManager ().DisplayMessage (DialogueManager_v2.DialogueType.DIALOGUE, dialogue, 
			GameController_v7.Instance.GetImageManager().GetDialogueImage(id));
	}

	void ToggleMobileUI(bool flag) {
		#if UNITY_ANDROID
//		EventManager.Instance.ToggleMobileControls(flag);
//		GameController_v7.Instance.GetEventManager().ToggleMobileControls(flag); // TODO check
//		GameController_v7.Instance.GetDialogueManager ().DisplayMessage (DialogueManager_v2.DialogueType.DIALOGUE, dialogue, 
//			null);
		Parameters parameters = new Parameters ();
		parameters.PutExtra ("FLAG", flag);
		EventBroadcaster.Instance.PostEvent (EventNames.TOGGLE_MOBILE_UI, parameters);
		#endif
	}

	public override void PlayScenes () {
		if (!isPlaying) {
			base.PlayScenes ();

			StartCoroutine (Gambit (1));
		}
	}


	public PlayerYuni GetPlayer() {
		if (this.player == null) {
			this.player = GameObject.FindObjectOfType<PlayerYuni> ();
		}
		return this.player;
	}
	private IEnumerator Gambit(int i) {
		//Insert dialogue here or whatever

		//1st dialogue: Why do you resist me when we're the same?
		//2nd dialogue: You fool! This is our way of life!
		//3rd dialogue: Very well! Its either win or lose!

//		switch (i) {
//		case 1:
//			StartCoroutine (InitialEncounter ());
//
//			break;
//		case 2:
//			StartCoroutine (MidfightDialogue ());
//
//			break;
//		case 3:
//			StartCoroutine (ConclusiveDialogue ());
//
//			break;
//		}
		if (i > 1) {
			gambits [i - 2].SetActive (false);
		}

		gambits [i-1].SetActive (true);

		if (i == 2 || i == 3) {
//			playerMovement.GetPlayer().gameObject.transform.position = spawnPoint.position;
			GetPlayer().gameObject.transform.position = spawnPoint.position;
		}


//		playerAttack.canAttack (true);
		//		playerMovement.canMove (true);


//		if (i > 1) {
//			gambits [i - 2].SetActive (false);
//		}
//
//		gambits [i-1].SetActive (true);

		switch (i) {
		case 1:
			StartCoroutine (InitialEncounter ());
			break;
		case 2:
//			if (ringleaders [1] != null) {
//				ringleaders [1].Death ();
//			}
			StartCoroutine (MidfightDialogue ());
			break;
		case 3:
//			if (ringleaders [2] != null) {
//				ringleaders [2].Death ();
//			}
			StartCoroutine (ConclusiveDialogue ());
			break;
		}
		while (isInDialogue) {
			yield return null;
		}
		GetPlayer().GetPlayerAttack().canAttack(true);
		GetPlayer().GetPlayerMovement().canMove(true);
		ToggleMobileUI (true);



		ringleaders [i-1].gameObject.SetActive (true);

		currentGambit = ringleaders [i-1].GetComponent<RingleaderAttack> ();
		currentRingleaderMovement = ringleaders [i-1].GetComponent<RingleaderMovement> ();
		currentRingleaderHealth = ringleaders [i-1].GetComponent<EnemyHealth> ();

		currentRingleaderMovement.canMove = true;
		currentRingleaderHealth.enabled = true;
		currentGambit.enabled = true;


		while (ringleaders [i-1].isAlive) {
			yield return null;
		}
		if (ringleaders [i - 1] != null) {
			ringleaders [i - 1].CallDeath (0);
		}

//		while (ringleaders [i-1] != null) {
//			yield return null;
//		}

		if (i < gambits.Count)
			StartCoroutine (Gambit (++i));
		else
			StartCoroutine (RingleaderDeath ());
	}

	private IEnumerator RingleaderDeath() {
		ringleaders [ringleaders.Count - 1].gameObject.SetActive (true);
		isInDialogue = true;
		EnableDialogue ("RINGLEADER");//Ringleader
		//You...
		//You could have...
		//Lead us... Instead...
		while (GameController_v7.Instance.GetDialogueManager().IsDialogueBoxPlaying()) {
			disablePlayerControls ();
			yield return null;
		}

		ringleaders [ringleaders.Count - 1].GetComponent<EnemyHealth> ().Death ();

		yield return new WaitForSeconds (2.0f);

		EnableDialogue ("YUNI");//Yuni
		//...
		//Well, I could end up like him.
		//Actually, special Phantoms like me and him are the only ones with any intelligence.
		//But still...
		//Some things must be done...
		while (GameController_v7.Instance.GetDialogueManager().IsDialogueBoxPlaying()) {
			disablePlayerControls ();
			yield return null;
		}

		player.GetPlayerAttack().canAttack(true);
		player.GetPlayerMovement().canMove(true);

//		playerAttack.canAttack (true);
//		playerMovement.canMove (true);
		ToggleMobileUI (true);

		//TODO: Candy, you can add a call function here to something you want to happen after their scene.
	}

	private IEnumerator InitialEncounter() {
		isInDialogue = true;
		EnableDialogue ("RINGLEADER");//Ringleader
		//So you've come...
		//How curious.
		//Why do you resist?
		while (GameController_v7.Instance.GetDialogueManager().IsDialogueBoxPlaying()) {
			disablePlayerControls ();
			yield return null;
		}

		EnableDialogue ("YUNI");//Yuni
		//To fix this WORLD!
		while (GameController_v7.Instance.GetDialogueManager().IsDialogueBoxPlaying()) {
			disablePlayerControls ();
			yield return null;
		}

		isInDialogue = false;
		this.particleEffect.Play ();
	}

	private IEnumerator MidfightDialogue() {
		isInDialogue = true;
		EnableDialogue ("RINGLEADER");//Ringleader
		//Fix the world... Yes, that's obvious.
		//I'm asking why you do so even if we're similar.
		while (GameController_v7.Instance.GetDialogueManager().IsDialogueBoxPlaying()) {
			disablePlayerControls ();
			yield return null;
		}

		EnableDialogue ("YUNI");//Yuni
		//Because this world isn't ours to destroy!
		while (GameController_v7.Instance.GetDialogueManager().IsDialogueBoxPlaying()) {
			disablePlayerControls ();
			yield return null;
		}

		EnableDialogue ("RINGLEADER");//Ringleader
		//Our race needs to possess to thrive.
		//We have no choice but to conquer worlds!
		//Their lives be damned!
		//We need to sustain our lives!
		while (GameController_v7.Instance.GetDialogueManager().IsDialogueBoxPlaying()) {
			disablePlayerControls ();
			yield return null;
		}

		EnableDialogue ("YUNI");//Yuni
		//So do THEY!
		while (GameController_v7.Instance.GetDialogueManager().IsDialogueBoxPlaying()) {
			disablePlayerControls ();
			Debug.Log ("STUCK");
			yield return null;
		}

		GameController_v7.Instance.GetPauseController ().Continue ();
		isInDialogue = false;
	}

	private IEnumerator ConclusiveDialogue() {
		isInDialogue = true;
		EnableDialogue ("RINGLEADER");//Ringleader
		//You fool!
		//You would cause the destruction of your own race?!
		while (GameController_v7.Instance.GetDialogueManager().IsDialogueBoxPlaying()) {
			disablePlayerControls ();
			yield return null;
		}

		EnableDialogue ("YUNI");//Yuni
		//No. I'm saving both theirs and ours!
		//I'll stop this pitiful cycle of our parasitic ways.
		//If our continued existence will only harm others, then we shouldn't continue existing.
		//Our path of destruction ends here!
		//You, who has helped me throughout this world, our chance meeting was likely for this moment.
		//So, please, help me defeat him! I'll stand back up no matter how many times needed!
		while (GameController_v7.Instance.GetDialogueManager().IsDialogueBoxPlaying()) {
			disablePlayerControls ();
			yield return null;
		}

		isInDialogue = false;
	}
}
