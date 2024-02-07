using System.Collections;
using System.Collections.Generic;
using UnityEngine;
/// <summary>
/// Class that handles the player health.
/// </summary>
public class PlayerHealth : MonoBehaviour {

    /// <summary>
    /// Reference that indicates if the player is still alive.
    /// </summary>
	public bool isAlive;
    /// <summary>
    /// Reference to the player.
    /// </summary>
	public PlayerYuni player;
    /// <summary>
    /// Reference to the player's max health.
    /// </summary>
	[SerializeField] private float maxHp = 30f;
    /// <summary>
    /// Reference to the player's current health.
    /// </summary>
	[SerializeField] private float hp = 30f;
    /// <summary>
    /// Reference to the player's damage effect.
    /// </summary>
	[SerializeField] private ParticleEffect effectYuniDamage;


	/// <summary>
	/// Function called on enable.
	/// </summary>
	/// <returns></returns>
	void OnEnable () {
		this.isAlive = true;
		this.hp = this.maxHp;
		UpdateHealthUI();
//		Parameters parameters = new Parameters ();
//		parameters.PutObjectExtra ("playerHealth", this);
//		EventBroadcaster.Instance.PostEvent(EventNames.ON_PLAYER_ENABLE, parameters);
		GameController_v7.Instance.GetObjectStateManager ().UpdatePlayerIsAlive (true);
//		if(objectStateManager != null)
//			objectStateManager.UpdatePlayerIsAlive (true);
	}
	/// <summary>
	/// Getter function for the player.
	/// </summary>
	/// <returns></returns>
	public PlayerYuni GetPlayer() {
		if (this.player == null) {
			this.player = GetComponent<PlayerYuni> ();
		}
		return this.player;
	}

	/// <summary>
	/// Function called on awake.
	/// </summary>
	/// <returns></returns>
	void Awake() {
		hp = maxHp;
		this.isAlive = true;
	}

	/// <summary>
	/// Function called on start.
	/// </summary>
	/// <returns></returns>
	void Start () {
		hp = maxHp;
		this.isAlive = true;
		UpdateHealthUI();
	}

	/// <summary>
	/// Function called once per frame.
	/// </summary>
	/// <returns></returns>
	void Update() {
//		if (objectStateManager == null)
//			objectStateManager = GameObject.FindGameObjectWithTag ("GameController").GetComponent<ObjectStateManager> ();
		if (!this.isAlive) {
			Death ();
		}
	}

	/// <summary>
	/// Subtracts the player's health based on the value of the passed parameter.
	/// </summary>
	/// <param name="points"></param>
	/// <returns></returns>
	public void Damage(int points) {
		this.hp -= points;
		SoundManager.Instance.Play (AudibleNames.Yuni.DAMAGE, false);
		if (this.GetEffectYuniDamage () != null) {
			this.GetEffectYuniDamage ().Play ();
		}
		if (this.hp < 0)
			this.hp = 0;

//		objectStateManager.UpdatePlayerCurrentHP (this.hp);

		UpdateHealthUI ();
		if (this.hp == 0)
			this.isAlive = false;
	}

	/// <summary>
	/// Updates the health UI.
	/// </summary>
	/// <returns></returns>
	void UpdateHealthUI() {
		// Update Blue Bar
		Parameters parameters = new Parameters ();
		parameters.PutObjectExtra ("playerHealth", this);
		EventBroadcaster.Instance.PostEvent (EventNames.ON_PLAYER_DAMAGE, parameters);

		// Update Text
		parameters = new Parameters ();
		parameters.PutExtra ("currentHP", this.hp);
		parameters.PutExtra ("maxHP", this.maxHp);
		EventBroadcaster.Instance.PostEvent (EventNames.ON_HEALTH_UPDATE, parameters);

	}

	/// <summary>
	/// Getter function that returns the maximum health of the player.
	/// </summary>
	/// <returns></returns>
	public float GetMaxHp() {
		return this.maxHp;
	}

	/// <summary>
	/// Function called on player death. Drops all carried objects, deactivates the player game object, and posts a player death event.
	/// </summary>
	/// <returns></returns>
	private void Death() {
//		Parameters parameters = new Parameters ();
//		parameters.PutObjectExtra ("playerHealth", this);
//		EventBroadcaster.Instance.PostEvent(EventNames.ON_PLAYER_DEATH, parameters);
//		objectStateManager.UpdatePlayerIsAlive(false);
		this.GetPlayer().DropDead();
		this.gameObject.SetActive (false);

		//for notifying player death manager
		EventBroadcaster.Instance.PostEvent (EventNames.ON_PLAYER_DEATH);
	}

	/// <summary>
	/// Getter function that returns the player's current health.
	/// </summary>
	/// <returns></returns>
	public float CurrentHealth() {
		return this.hp;
	}

	/// <summary>
	/// Setter function that sets the player's current health.
	/// </summary>
	/// <param name="hp"></param>
	/// <returns></returns>
	public void SetCurrentHealth(float hp) {
		this.hp = hp;
	}

	/// <summary>
	/// Setter function that sets the player's maximum health.
	/// </summary>
	/// <param name="maxHp"></param>
	/// <returns></returns>
	public void SetMaxHp(float maxHp) {
		this.maxHp = maxHp;
	}

	/// <summary>
	/// Getter function that returns the player damage effect.
	/// </summary>
	/// <returns></returns>
	public ParticleEffect GetEffectYuniDamage() {
		if (this.effectYuniDamage == null) {
			ParticleEffect[] effectList = GetComponentsInChildren<ParticleEffect> ();
			for (int i = 0; i < effectList.Length; i++) {
				if (effectList [i].GetEffectType() == ParticleEffect.EffectType.YUNI_DAMAGE) {
					this.effectYuniDamage = effectList [i];
					i = effectList.Length;
				}
			}
		}
		return this.effectYuniDamage;
	}
}
