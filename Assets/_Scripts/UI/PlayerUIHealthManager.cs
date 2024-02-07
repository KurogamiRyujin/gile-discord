using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

/// <summary>
/// Displays the player avatar's health.
/// </summary>
public class PlayerUIHealthManager : MonoBehaviour {
//	[SerializeField] private GameObject healthBarObject;
	//	[SerializeField] private RectTransform healthBarTransform;
    /// <summary>
    /// Reference to the health circle which visualizes the health.
    /// </summary>
	[SerializeField] private HealthCircle healthCircle;
	[SerializeField] private Image healthCircleTransform;

    /// <summary>
    /// Player avatar's current HP.
    /// </summary>
	[SerializeField] private float currentHp;
    /// <summary>
    /// Player avatar's max HP.
    /// </summary>
	[SerializeField] private float maxHp;
    /// <summary>
    /// Health meter's current scale.
    /// </summary>
	[SerializeField] private float currentScaleX;
    /// <summary>
    /// Health meter's original scale.
    /// </summary>
	[SerializeField] private Vector3 originalScale;
	[SerializeField] private float originalScaleX;

    /// <summary>
    /// Standard Unity Function. Called once in the game object's lifetime to initiate the script once it has been enabled.
    /// </summary>
    void Start () {
		EventBroadcaster.Instance.AddObserver (EventNames.ON_PLAYER_DAMAGE, this.OnPlayerDamage);
		EventBroadcaster.Instance.AddObserver (EventNames.ON_PLAYER_ENABLE, this.OnPlayerEnable);

//		healthBarTransform = (RectTransform)healthBarObject.transform;
//		this.originalScale = healthBarObject.transform.localScale;
		this.originalScaleX = this.originalScale.x;

	}

    /// <summary>
    /// Returns the health circle UI element behaviour.
    /// </summary>
    /// <returns>Health Circle</returns>
	public HealthCircle GetHealthCircle() {
		if (this.healthCircle == null) {
			this.healthCircle = GameObject.FindObjectOfType<HealthCircle> ();
		}
		return this.healthCircle;
	}

	/// <summary>
    /// Called when the player avatar is enabled in the room.
    /// 
    /// Updates the health meter according to the player avatar's HP.
    /// </summary>
    /// <param name="parameters"></param>
	void OnPlayerEnable(Parameters parameters) {
		PlayerHealth playerHealth = parameters.GetObjectExtra ("playerHealth") as PlayerHealth;
		this.maxHp = playerHealth.GetMaxHp ();
		this.currentHp = playerHealth.CurrentHealth ();
		//		this.UpdateHealthLength ();
		this.UpdateHealthCircle (this.currentHp, this.maxHp);
	}

    /// <summary>
    /// Called when the player avatar is damaged.
    /// 
    /// Decreases the health meter's scale.
    /// </summary>
    /// <param name="parameters"></param>
	void OnPlayerDamage(Parameters parameters) {
		PlayerHealth playerHealth = parameters.GetObjectExtra ("playerHealth") as PlayerHealth;
		this.maxHp = playerHealth.GetMaxHp ();
		this.currentHp = playerHealth.CurrentHealth ();
//		this.UpdateHealthLength ();
		this.UpdateHealthCircle (this.currentHp, this.maxHp);

	}

    /// <summary>
    /// Updates the health circle's scale whenever the player avatar's HP changes.
    /// </summary>
    /// <param name="currentHP">Current HP</param>
    /// <param name="maxHP">Max HP</param>
	void UpdateHealthCircle(float currentHP, float maxHP) {
		this.GetHealthCircle ().UpdateHealth(currentHP, maxHP);
	}

//	void UpdateHealthLength() {
//		float healthFraction = CurrentHealthPercent ();
//		this.currentScaleX = this.originalScaleX*healthFraction;
//		if(healthBarObject != null)
//			this.healthBarObject.transform.localScale = new Vector3 (this.currentScaleX, this.originalScale.y, this.originalScale.z);
//	}

    /// <summary>
    /// Returns the remaining percentage of the player avatar's health.
    /// </summary>
    /// <returns>Current Player Avatar's HP in percent</returns>
	float CurrentHealthPercent() {
		return this.currentHp / this.maxHp;
	}

	void Update () {
		
	}
}
