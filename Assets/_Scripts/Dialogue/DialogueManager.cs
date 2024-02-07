using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class DialogueManager : MonoBehaviour {

	private const float WAIT_TIME = 3f;

	public Animator animator;
	public bool isPlaying = true;

	private Queue<string> sentences;

	public Text nameText;
	public Text dialogueText;

	public Image spriteImage;
	public Image ringleaderImage;
	public Image yuniImage;
	float counter;

	// Use this for initialization
	void Start () {
		sentences = new Queue<string> ();
	}


	public void StartDialogue(Dialogue dialogue) {
		animator.SetBool ("IsOpen", true);

		isPlaying = true;
		Debug.Log ("Starting conversation with " + dialogue.name);
		nameText.text = dialogue.name.ToUpper();
		if (nameText.text.Contains ("RINGLEADER")) {
			if (yuniImage != null)
				yuniImage.gameObject.SetActive (false);
		}
		else {
			if(yuniImage != null)
				yuniImage.gameObject.SetActive (true);
		}
		sentences.Clear ();

		foreach (string sentence in dialogue.sentences) {
			sentences.Enqueue (sentence);
		}

		DisplayNextSentence ();
	}

	public void DisplayNextSentence() {
		if (sentences.Count == 0) {
			EndDialogue ();
			return;
		}

		string sentence = sentences.Dequeue ();
		Debug.Log (sentence);
		StopAllCoroutines ();
		StartCoroutine (TypeSentence (sentence));
	}

	IEnumerator TypeSentence (string sentence) {
		
		dialogueText.text = "";
		foreach (char letter in sentence.ToCharArray()) {
			dialogueText.text += letter;
			yield return new WaitForSecondsRealtime (0.0f);
		}

		counter = 0f;

		while (counter < WAIT_TIME) {
//			print ("waiting " + counter);
			counter += Time.deltaTime;
			yield return null;
		}

		DisplayNextSentence ();
	}

	public void EndDialogue() {
		animator.SetBool ("IsOpen", false);
		isPlaying = false;
		Debug.Log ("End of Conversation");
	}


}
