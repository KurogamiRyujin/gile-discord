using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// UNUSED CLASS
/// 
/// Bullet Hell class that creates a bullet hell feature in whatever scene it is activated.
/// Bullet spread is determined by the Hell Style property.
/// </summary>
public class BulletHell : MonoBehaviour {

    /// <summary>
    /// Determines how the Bullet objects spread.
    /// </summary>
	public enum HellStyle
	{
		STRAIGHT_SHOTS,
		SPIRALING_SHOTS
	}

    /// <summary>
    /// Component that shoots Bullet objects.
    /// </summary>
	[SerializeField] private BulletShooter bulletShooter;
    /// <summary>
    /// Flag to check if the shooter should rotate clockwise or counter-clockwise.
    /// </summary>
	[SerializeField] private bool isCounterClockwise = false;

    /// <summary>
    /// Shooter's rotation speed.
    /// </summary>
	[SerializeField] private float rotationSpeed = 2.5f;
    /// <summary>
    /// Shooter's firing interval in seconds.
    /// </summary>
	[SerializeField] private float firingInterval = 1.0f;
    /// <summary>
    /// Damage value given for all Bullet objects.
    /// </summary>
	private int damage = 0;
    /// <summary>
    /// Transform of the target the shooter should aim at. Used only when Hell Style is set to Straight Shots.
    /// </summary>
	[SerializeField] private Transform targetTransform = null;
    /// <summary>
    /// Target's direction relative to the Bullet Hell object's position.
    /// </summary>
	private Vector2 targetDirection;
    /// <summary>
    /// Counter to how many Bullets can be fired in rapid succession before cooldown.
    /// </summary>
	private int rapidfireCounter = 0;
    /// <summary>
    /// Flag to check if shooter is on cooldown.
    /// </summary>
	private bool isCooldown = false;
    /// <summary>
    /// Determines the Bullet spread.
    /// </summary>
	private HellStyle shootingStyle;
    /// <summary>
    /// Flag if the Bullet Hell object is instantiated.
    /// </summary>
	private bool isInitiated = false;
    /// <summary>
    /// Flag to check if the shooter is peforming a rapid fire burst.
    /// </summary>
	private bool isRapidFiring = false;
    
	/// <summary>
    /// Standard Unity function. Called once every frame.
    /// </summary>
	void Update () {
		if (isInitiated) {
			if (targetTransform != null) {
				targetDirection = new Vector2 (targetTransform.position.x, targetTransform.position.y);
			}

			Debug.Log ("Direction: " + targetDirection);

			if (!isCooldown) {
				FireShooter ();
			}

			if (shootingStyle == HellStyle.SPIRALING_SHOTS) {
				RotateSpiralingShooter ();
				Debug.Log ("Rotating");
			} else if (shootingStyle == HellStyle.STRAIGHT_SHOTS) {
				StraightShooter ();
			} else
				Debug.Log ("Not Rotating");
		}
	}

    /// <summary>
    /// If the Hell Style is set to Spiraling Shots, this function is called to make the shooter fire while rotating.
    /// </summary>
	private void RotateSpiralingShooter() {
		float rotationDiff = 1.0f;
		if (isCounterClockwise)
			rotationDiff = -1.0f;
		else
			rotationDiff = 1.0f;

        bulletShooter.transform.Rotate (rotationDiff * Vector3.forward, rotationSpeed * Time.deltaTime);
	}

    /// <summary>
    /// If Hell Style is set to Straight Shots, shooter takes aim at a target and rapid fires in that direction.
    /// </summary>
	private void StraightShooter () {
		if (targetTransform != null) {
			Vector3 dir = targetTransform.position - bulletShooter.transform.position;
			float angle = Mathf.Atan2(dir.y, dir.x) * Mathf.Rad2Deg;
			Quaternion q = Quaternion.AngleAxis(angle, Vector3.forward);
			bulletShooter.transform.rotation = q;
		}

		if (!isRapidFiring)
			StartCoroutine (RapidFiring ());
	}

    /// <summary>
    /// Coroutine function that controls rapid fire through manipulation of firing interval.
    /// </summary>
    /// <returns></returns>
	private IEnumerator RapidFiring() {
		isRapidFiring = true;
		float originalFiringInterval = firingInterval;

		firingInterval = originalFiringInterval / 2;
		while (isInitiated) {
			if (this.rapidfireCounter < 4)
				firingInterval = originalFiringInterval / 2;
			else
				firingInterval = originalFiringInterval * 8;
			yield return null;
		}
		firingInterval = originalFiringInterval;
		isRapidFiring = false;
	}

    /// <summary>
    /// Shooter fires either a Bullet or a Charm (formerly called Yarnball).
    /// </summary>
	private void FireShooter() {
		isCooldown = true;
		this.rapidfireCounter++;
		if (this.rapidfireCounter == 5) {
			this.rapidfireCounter = 0;
		}
		bulletShooter.ShootBullet (this.damage);
		StartCoroutine (Cooldown ());
	}

    /// <summary>
    /// Cooldown coroutine which prevents shooter from excessive, uncontrolled shooting.
    /// </summary>
    /// <returns></returns>
	private IEnumerator Cooldown() {
		yield return new WaitForSeconds (firingInterval);
		isCooldown = false;
	}

    /// <summary>
    /// Damage setter
    /// </summary>
    /// <param name="damage"></param>
	public void SetDamage(int damage) {
		this.damage = damage;
	}

    /// <summary>
    /// Target setter
    /// </summary>
    /// <param name="target"></param>
	public void SetTarget(Transform target) {
		this.targetTransform = target;
	}

    /// <summary>
    /// Hell Style setter
    /// </summary>
    /// <param name="hellStyle"></param>
	public void SetStyle(HellStyle hellStyle) {
		this.shootingStyle = hellStyle;
	}

    /// <summary>
    /// Counterclockwise flag setter
    /// </summary>
    /// <param name="flag"></param>
	public void IsCounterClockwise(bool flag) {
		this.isCounterClockwise = flag;
	}

    /// <summary>
    /// Call to start the Bullet Hell.
    /// </summary>
	public void BeginHell() {
		this.isInitiated = true;
	}

    /// <summary>
    /// Call to terminate the Bullet Hell.
    /// </summary>
	public void EndHell() {
		this.isInitiated = false;
	}
}
