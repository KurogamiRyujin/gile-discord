using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

/// <summary>
/// Handles functionality of the mobile UI
/// </summary>
public class MobileUI : MonoBehaviour {
    //	private const float MOVEMENT = 0.5f;
    /// <summary>
    /// LayerMask that refers to the 'Ground' layer
    /// </summary>
    [SerializeField] LayerMask groundLayer;
    /// <summary>
    /// Reference to the PlayerInput script
    /// </summary>
    PlayerInput player;
    /// <summary>
    /// Determines how fast Yuni will move
    /// </summary>
    float moveSpeed;

    /// <summary>
    /// Flag that determines whether or not the right button is pressed
    /// </summary>
    public bool rightPressed;
    /// <summary>
    /// Flag that determines whether or not the left button is pressed
    /// </summary>
    public bool leftPressed;
    /// <summary>
    /// Flag that determines whether or not the jump button is pressed
    /// </summary>
    public bool jumpPressed;
    /// <summary>
    /// Flag that determines whether or not the interact button is pressed
    /// </summary>
    public bool interactPressed;
    /// <summary>
    /// Flag that determines whether or not the charge button is pressed
    /// </summary>
    public bool chargePressed;
    /// <summary>
    /// Flag that determines whether or not the switch weapon button is pressed
    /// </summary>
    public bool switchPressed;
    /// <summary>
    /// Flag that determines whether or not the menu button is pressed
    /// </summary>
    public bool menuPressed;
    /// <summary>
    /// Flag that determines whether or not the help button is pressed
    /// </summary>
    public bool helpPressed;
    /// <summary>
    /// Movement speed with effect of Time.deltaTime
    /// </summary>
    public float inputValue;

    //EventTrigger leftTrigger, rightTrigger;

    /// <summary>
    /// Reference to HintColorPressed script
    /// </summary>
    [SerializeField] HintColorPressed colorPressed;

    /// <summary>
    /// Unity Function. Called once upon creation of the object 
    /// </summary>
	void Awake() {

		#if UNITY_STANDALONE
			Destroy(gameObject);
		#elif UNITY_ANDROID
			GameController_v7.Instance.GetMobileUIManager().SubscribeMobileUI(this);
//			Debug.LogError("Mobile UI Subscribed");
		#endif

	}

    /// <summary>
    /// Get value of inputValue variable
    /// </summary>
    /// <returns>Returns inputValue</returns>
	public float GetInputValue() {
		return this.inputValue;
	}

    /// <summary>
    /// Checks if jump button is pressed
    /// </summary>
    /// <returns>Returns true if jump button is pressed</returns>
	public bool IsJumpPressed() {
		return this.jumpPressed;
	}

    /// <summary>
    /// Checks if interact button is pressed
    /// </summary>
    /// <returns>Returns true if interact button is pressed</returns>
	public bool IsInteractPressed() {
		return this.interactPressed;
	}

    /// <summary>
    /// Checks if switch weapon button is pressed
    /// </summary>
    /// <returns>Returns true if switch weapon button is pressed</returns>
	public bool IsSwitchPressed() {
		return this.switchPressed;
	}

    /// <summary>
    /// Checks if charge button is pressed
    /// </summary>
    /// <returns>Returns true if charge button is pressed</returns>
	public bool IsChargePressed() {
		return this.chargePressed;
	}

    /// <summary>
    /// Checks if menu button is pressed
    /// </summary>
    /// <returns>Returns true if menu button is pressed</returns>
	public bool IsMenuPressed() {
		return this.menuPressed;
	}

    /// <summary>
    /// Checks if help button is pressed
    /// </summary>
    /// <returns>Returns true if help button is pressed</returns>
	public bool IsHelpPressed() {
		return this.helpPressed;
	}

    /// <summary>
    /// Sets jumpPressed to value of isJumping
    /// </summary>
    /// <param name="isJumping">Value is true if jump button is pressed</param>
	public void SetJumping(bool isJumping) {
		this.jumpPressed = isJumping;
	}

    /// <summary>
    /// Sets interactPressed to value of isInteracting
    /// </summary>
    /// <param name="isInteracting">Value is true if interact button is pressed</param>
	public void SetInteracting(bool isInteracting) {
		this.interactPressed = isInteracting;
	}

    /// <summary>
    /// Sets rightPressed to value of isPressed
    /// </summary>
    /// <param name="isPressed">Value is true if right button is pressed</param>
	public void SetRightPressed(bool isPressed) {
		this.rightPressed = isPressed;
	}

    /// <summary>
    /// Sets leftPressed to value of isPressed
    /// </summary>
    /// <param name="isPressed">Value is true if left button is pressed</param>
	public void SetLeftPressed(bool isPressed) {
		this.leftPressed = isPressed;
	}

    /// <summary>
    /// Unity Function. Called once the gameobject is disabled 
    /// </summary>
    void OnDisable() {
		leftPressed = false;
		rightPressed = false;
		jumpPressed = false;
	}


    /// <summary>
    /// Unity Function. Called once in the game object's lifetime to initiate the script once it has been enabled 
    /// </summary>
    // Use this for initialization
    void Start () {
		gameObject.tag = "Mobile UI";
		moveSpeed = 10f;
		rightPressed = false;
		leftPressed = false;
		interactPressed = false;
		jumpPressed = false;
		chargePressed = false;
		switchPressed = false;
		player = GameObject.FindObjectOfType<PlayerYuni> ().GetPlayerInput ();
		inputValue = 0f;

		// TODO: Change this
		//leftTrigger = GameObject.Find ("Left").GetComponent<EventTrigger> ();
		//rightTrigger = GameObject.Find ("Right").GetComponent<EventTrigger> ();

	}

    /// <summary>
    /// Unity Function. Called every fixed framerate frame 
    /// </summary>
    void FixedUpdate() { // Changed from Update to FixedUpdate
		if (leftPressed) {
			inputValue += Time.deltaTime * moveSpeed * -1;
			inputValue = Mathf.Lerp(-1, 0, inputValue);
		} else if (rightPressed) {
			inputValue += Time.deltaTime * moveSpeed;
			inputValue = Mathf.Lerp(0, 1, inputValue);
//			inputValue = Mathf.Clamp(inputValue, -1, 1);
		}
		else {
			inputValue = 0;
		}
	}


    /// <summary>
    /// Sets rightPressed to true
    /// </summary>
    public void RightButton_Down() {	
		rightPressed = true;
	}

    /// <summary>
    /// Sets rightPressed to false
    /// </summary>
	public void RightButton_Up() {
		rightPressed = false;
//		Debug.Log ("RU");
	}

    /// <summary>
    /// Sets leftPressed to true
    /// </summary>
	public void LeftButton_Down(){
		leftPressed = true;
//		Debug.Log ("LD");
	}

    /// <summary>
    /// Sets leftPressed to false
    /// </summary>
	public void LeftButton_Up() {
		leftPressed = false;
//		Debug.Log ("LU");
	}


    /// <summary>
    /// Sets jumpPressed to true
    /// </summary>
	public void JumpButton_Down() {
		jumpPressed = true;
	}

    /// <summary>
    /// Sets jumpPressed to false
    /// </summary>
	public void JumpButton_Up() {
		jumpPressed = false;
	}

    /// <summary>
    /// Sets interactPressed to true
    /// </summary>
	public void InteractButton_Down() {
		interactPressed = true;
	}

    /// <summary>
    /// Sets interactPressed to false
    /// </summary>
	public void InteractButton_Up() {
		interactPressed = false;
	}

    /// <summary>
    /// Sets switchPressed to true
    /// </summary>
	public void SwitchWeapon_Down() {
		switchPressed = true;
	}

    /// <summary>
    /// Sets switchPressed to false
    /// </summary>
	public void SwitchWeapon_Up() {
		switchPressed = false;
	}

    /// <summary>
    /// Sets helpPressed to true
    /// </summary>
	public void Help_Down() {
//		Debug.Log ("help down");
		helpPressed = true;
	}

    /// <summary>
    /// Sets helpPressed to false
    /// </summary>
	public void Help_Up() {
//		Debug.Log ("help up");
		helpPressed = false;
	}

    /// <summary>
    /// Sets menuPressed to true
    /// </summary>
	public void Menu_Down() {
//		Debug.Log ("menu down");
		menuPressed = true;
	}

    /// <summary>
    /// Sets menuPressed to false
    /// </summary>
    public void Menu_Up() {
//		Debug.Log ("menu up");
		menuPressed = false;
	}

    /// <summary>
    /// Sets chargePressed to true
    /// </summary>
	public void Charge_Down() {
//		Debug.Log ("charge down");
		chargePressed = true;
		if (this.GetHintColorPressed () != null) {
			this.GetHintColorPressed ().Open ();
		}
	}

    /// <summary>
    /// Sets chargePressed to false
    /// </summary>
	public void Charge_Up() {
//		Debug.Log ("charge up");
		chargePressed = false;
		if (this.GetHintColorPressed () != null) {
			this.GetHintColorPressed ().Close ();
		}
	}

    /// <summary>
    /// Checks if right button is pressed
    /// </summary>
    /// <returns>Returns true if right button is pressed</returns>
	public bool IsRightPressed() {
		return this.rightPressed;
	}

    /// <summary>
    /// Checks if left button is pressed
    /// </summary>
    /// <returns>Returns true if left button is pressed</returns>
	public bool IsLeftPressed() {
		return this.leftPressed;
	}

    /// <summary>
    /// Gets reference of HintColorPressed script
    /// </summary>
    /// <returns>Returns reference to HintColorPressed script</returns>
	public HintColorPressed GetHintColorPressed() {
		if (this.colorPressed == null) {
			this.colorPressed = GetComponentInChildren<HintColorPressed> ();
		}
		return this.colorPressed;
	}
}
