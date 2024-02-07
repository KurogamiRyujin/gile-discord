using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using System;

/// <summary>
/// Dialogue box behaviour.
/// </summary>
public class DialogueBox : MonoBehaviour, DialogueInterface {

    /// <summary>
    /// Constant time to wait before displaying the next sentence.
    /// </summary>
	private const float WAIT_TIME = 3f;
    /// <summary>
    /// Constant time to wait for the success animation to finish before dropping the dialogue box.
    /// </summary>
	public const float ANIMATION_WAIT = 0.8f;
    /// <summary>
    /// Reference to the dialogue box's animator.
    /// </summary>
	private Animator animator;

    /// <summary>
    /// Type of dialogue the dialogue box displays.
    /// </summary>
	[SerializeField] private DialogueManager_v2.DialogueType dialogueType = DialogueManager_v2.DialogueType.DIALOGUE;
    /// <summary>
    /// Set dialogue type the dialogue is supposed to execute.
    /// </summary>
	private DialogueManager_v2.DialogueType setType;//just in case dialogueType is somehow changed midway executions

    /// <summary>
    /// Flag to check if the dialogue is currently playing.
    /// </summary>
	private bool isPlaying = false;

    /// <summary>
    /// Sentences to display as a queue.
    /// </summary>
	private Queue<string> sentences;

    /// <summary>
    /// Reference to the Text component which dislplays the character's name.
    /// </summary>
	[SerializeField] private Text nameText;
    /// <summary>
    /// Reference to the Text component which displays the sentences.
    /// </summary>
	[SerializeField] private Text dialogueText;

    /// <summary>
    /// Reference to the image UI element which display's the character's image.
    /// </summary>
	[SerializeField] private Image spriteImage;
    
    /// <summary>
    /// Time counter used when waiting before the next sentence is displayed.
    /// </summary>
	private float counter;
    /// <summary>
    /// Flag if a coroutine is playing.
    /// </summary>
	bool isCoroutinePlaying;
    /// <summary>
    /// Reference to the previous sentence.
    /// </summary>
	string prevSentence;

    /// <summary>
	/// Unity Function. Called once upon creation of the object.
	/// </summary>
	void Awake () {
		animator = GetComponent<Animator> ();
		sentences = new Queue<string> ();
		//		EventBroadcaster.Instance.AddObserver (EventNames.ON_HINT_SUCCESS, HintSuccess);
	}

    /// <summary>
    /// Raises the destroy event.
    /// </summary>
	void OnDestroy() {
		//		EventBroadcaster.Instance.RemoveObserver (EventNames.ON_HINT_SUCCESS);
	}

    /// <summary>
    /// Plays the success animation when completing a mission.
    /// </summary>
	public void HintSuccess() {
		EventBroadcaster.Instance.PostEvent (EventNames.PLAY_HINT_RESULTS);
		StartCoroutine (PlayingSuccess ());
	}

    /// <summary>
    /// Closes the hint dialogue box.
    /// </summary>
	public void CloseHint() {
		this.EndDialogue ();
	}

    /// <summary>
    /// Coroutine for the success animation.
    /// </summary>
    /// <returns></returns>
	IEnumerator PlayingSuccess() {
		isPlaying = false;
		yield return new WaitForSeconds (ANIMATION_WAIT);
		this.EndDialogue ();
	}

    /// <summary>
    /// Unity Function. Called when the component becomes enabled.
    /// </summary>
	void OnEnable() {
		setType = dialogueType;
		GameController_v7.Instance.GetDialogueManager ().Register (setType, this);
	}

    /// <summary>
    /// Unity Function. Called when the component is disabled.
    /// </summary>
	void OnDisable() {
		switch (setType) {
		case DialogueManager_v2.DialogueType.DIALOGUE:
			GameController_v7.Instance.GetDialogueManager ().UnregisterDialogueBox (this);
			break;
		case DialogueManager_v2.DialogueType.HINT:
			GameController_v7.Instance.GetDialogueManager ().UnregisterHintBox (this);
			break;
		}
	}

	// For the dialogue controls
	// Any tap onscreen will next the dialogue
    /// <summary>
    /// Skips to the sentence in the dialogue.
    /// </summary>
    /// <returns></returns>
	public IEnumerator DialogueNextRoutine() {
		// TODO commented out []
//		while (this.isPlaying) {
//			if (Input.GetButton ("Fire1")) {
//				this.DisplayNextSentence ();
//			}
//			yield return null;
//		}
		yield return null;
	}

    /// <summary>
    /// Initiates the dialogue of the character.
    /// </summary>
    /// <param name="dialogue">Dialogue to be displayed.</param>
    /// <param name="characterImage">Speaking character's image.</param>
	public void StartDialogue(Dialogue dialogue, Image characterImage) {
		prevSentence = null;
		isPlaying = true;
		animator.SetBool ("IsOpen", true);

		// Check for screen taps to advance dialogue
		StartCoroutine (DialogueNextRoutine ());

		//		Debug.Log ("<color=red>sprite name is </color>"+characterImage.sprite.name);

		TextAsset textFile = dialogue.getTextFile ();
		if (textFile != null) {
			List<string> text = new List<string>(textFile.text.Split ('\n'));
			dialogue.name = text [0];
			text.RemoveAt (0);
			dialogue.sentences = text.ToArray();
		}

		Debug.LogError ("NAME TEXT IS "+dialogue.name.ToUpper ());
		if (nameText != null) {
			nameText.text = dialogue.name.ToUpper ();
            //			Debug.Log ("ENTERED NAME TEXT IS "+GameController_v7.Instance.GetImageManager ().GetDialogueImage (nameText.text.Trim ()).name);
            
            string imageName = this.LoadSkin(nameText.text.Trim());
            if (GameController_v7.Instance.GetImageManager().GetDialogueImage(imageName) != null) {
                this.spriteImage.sprite = GameController_v7.Instance.GetImageManager().GetDialogueImage(imageName).sprite;
            }

        }       
		//		else {
		//			this.spriteImage.sprite = GameController_v7.Instance.GetImageManager ().GetDialogueImage ("YUNI").sprite;
		//		}
		//		if (characterImage != null) {
		//			this.spriteImage.sprite = characterImage.sprite;
		////			spriteImage = characterImage;
		//		} else {
		//		Debug.Log("name is "+nameText.text.Trim());
		//		}
		sentences.Clear ();

		foreach (string sentence in dialogue.sentences) {
			sentences.Enqueue (sentence);
			if (prevSentence == null)
				prevSentence = sentence;
		}

		DisplayNextSentence ();
	}

    /// <summary>
    /// Loads the image of the character.
    /// </summary>
    /// <param name="name">Character name.</param>
    /// <returns>Character's skin</returns>
    public string LoadSkin(string name) {
        switch (name) {
            case "???":
                name = LoadSkinYuni();
                break;
            case "YUNI":
                name = LoadSkinYuni();
                break;
            default:
                break;
        }

        return name;
    }

    /// <summary>
    /// Loads the player avatar's skin.
    /// </summary>
    /// <returns></returns>
    public string LoadSkinYuni() {
        switch (GameController_v7.Instance.GetPlayerSkin()) {
            case GameController_v7.PlayerSkin.DEFAULT_FEMALE:
                name = "DEFAULT_FEMALE";
                break;
            default:
                name = "YUNI";
                break;
        }
        return name;
    }

    /// <summary>
    /// Checker to prevent hints from cascading when the player advances too fast for the hints.
    /// </summary>
	public void HandleHintCascade() {
		if (isPlaying) {
			EndDialogue ();
		}
	}
    
    /// <summary>
    /// Initiates a hint.
    /// </summary>
    /// <param name="dialogue">Hint to be displayed.</param>
    /// <param name="characterImage">Character image, if any.</param>
	public void StartHintDialogue(Dialogue dialogue, Image characterImage) {
		HandleHintCascade ();
		isPlaying = true;
		animator.SetBool ("IsOpen", true);

		//		Debug.Log ("<color=red>sprite name is </color>"+characterImage.sprite.name);

		TextAsset textFile = dialogue.getTextFile ();
		if (textFile != null) {
			List<string> text = new List<string>(textFile.text.Split ('\n'));
			dialogue.name = text [0];
			text.RemoveAt (0);
			dialogue.sentences = text.ToArray();
		}

		Debug.Log ("NAME TEXT IS "+dialogue.name.ToUpper ());
		if (nameText != null) {
			nameText.text = dialogue.name.ToUpper ();
			if(GameController_v7.Instance.GetImageManager ().GetDialogueImage (nameText.text.Trim ()) != null)
				this.spriteImage.sprite = GameController_v7.Instance.GetImageManager ().GetDialogueImage (nameText.text.Trim ()).sprite;

		}
		sentences.Clear ();

		foreach (string sentence in dialogue.sentences) {
			sentences.Enqueue (sentence);
		}

		DisplayHintNextSentence ();
	}

    /// <summary>
    /// Initiates a dialouge.
    /// </summary>
    /// <param name="dialogue">Dialogue to be displayed.</param>
    /// <param name="characterImage">Speaking character's image</param>
	public void StartDialogue(string[] dialogue, Image characterImage) {
		isPlaying = true;
		animator.SetBool ("IsOpen", true);

		if(nameText != null)
			nameText.text = dialogue[0];
		if(characterImage != null)
			spriteImage = characterImage;
		sentences.Clear ();

		for (int i = 1; i < dialogue.Length; i++)
			sentences.Enqueue (dialogue [i]);

		DisplayNextSentence ();
	}

    /// <summary>
    /// Initiates a hint.
    /// </summary>
    /// <param name="dialogue">Hint to be displayed.</param>
    /// <param name="characterImage">Character's image, if any.</param>
	public void StartHintDialogue(string[] dialogue, Image characterImage) {
		HandleHintCascade ();
		isPlaying = true;
		animator.SetBool ("IsOpen", true);

		if(nameText != null)
			nameText.text = dialogue[0];
		if(characterImage != null)
			spriteImage = characterImage;
		sentences.Clear ();

		for (int i = 1; i < dialogue.Length; i++)
			sentences.Enqueue (dialogue [i]);

		DisplayHintNextSentence ();
	}

    /// <summary>
    /// Function to skip the current dialogue.
    /// </summary>
	public void SkipDialogue() {
		// TODO: Implement skip
		if (isPlaying) {
			EndDialogue ();
		}
	}

    /// <summary>
    /// Skips to the next sentence in a hint.
    /// </summary>
	public void DisplayHintNextSentence() {
		//		if (sentences.Count != 0) {
		//			string sentence = sentences.Dequeue ();
		//			StopAllCoroutines ();
		//			StartCoroutine (TypeHintSentence (sentence));
		//		}


		if (sentences.Count == 0) {
			//			EndDialogue ();
			return;
		}

		string sentence = sentences.Dequeue ();
		StopAllCoroutines ();
		StartCoroutine (TypeHintSentence (sentence));
	}

    /// <summary>
    /// Skips to the next sentence in a dialogue.
    /// </summary>
	public void DisplayNextSentence() {

		if (isCoroutinePlaying) {
			StopAllCoroutines ();
			dialogueText.text = prevSentence;
			isCoroutinePlaying = false;
		} else {

			if (sentences.Count == 0) {
				EndDialogue ();
				return;
			}

			string sentence = sentences.Dequeue ();
			StopAllCoroutines ();
			StartCoroutine (TypeSentence (sentence));
		}
	}

    /// <summary>
    /// Writes the sentence onto the hint box, one letter at a time.
    /// </summary>
    /// <param name="sentence"></param>
    /// <returns></returns>
	IEnumerator TypeHintSentence (string sentence) {
		dialogueText.text = "";
		foreach (char letter in sentence.ToCharArray()) {
			dialogueText.text += letter;
			yield return null;
		}

		counter = 0f;

		while (counter < WAIT_TIME) {
			counter += Time.unscaledDeltaTime;
			yield return null;
		}

		//		DisplayNextSentence ();
		DisplayHintNextSentence ();
	}

    /// <summary>
    /// Types the sentence onto the dialogue box, one letter at a time.
    /// </summary>
    /// <param name="sentence"></param>
    /// <returns></returns>
	IEnumerator TypeSentence (string sentence) {
		prevSentence = sentence;
		dialogueText.text = "";
		isCoroutinePlaying = true;
		foreach (char letter in sentence.ToCharArray()) {
			dialogueText.text += letter;
			yield return null;
		}
		//
		//		counter = 0f;
		//
		//		while (counter < WAIT_TIME) {
		//			counter += Time.unscaledDeltaTime;
		//			yield return null;
		//		}

		isCoroutinePlaying = false;
		//		DisplayNextSentence ();
	}

    /// <summary>
    /// Terminates and closes the dialogue box.
    /// </summary>
	public void EndDialogue() {
		animator.SetBool ("IsOpen", false);
		isPlaying = false;
	}

    /// <summary>
    /// Checker if the dialogue box is playing.
    /// </summary>
    /// <returns>If the dialogue box is playing. False, otherwise.</returns>
	public bool IsPlaying() {
		return this.isPlaying;
	}
}