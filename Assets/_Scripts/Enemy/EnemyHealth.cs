using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

/// <summary>
/// Handles an enemy character's health status.
/// </summary>
public class EnemyHealth : MonoBehaviour {
	// NOTE: Animator should call death

    /// <summary>
    /// String constant name for the "hit" animation.
    /// </summary>
	public const string ANIMATOR_HIT = "hit";
    /// <summary>
    /// String constant name for the "death" animation.
    /// </summary>
	public const string ANIMATOR_DEATH = "death";

    /// <summary>
    /// UNUSED
    /// 
    /// Offset between the enemy's sprite and its health bar.
    /// </summary>
	public const float HEALTH_HEIGHT_OFFSET = 1.0f;
    /// <summary>
    /// UNUSED
    /// 
    /// Offset between the enemy's width and its health bar.
    /// </summary>
	public const float HEALTH_LENGTH_OFFSET = 0.5f;
    /// <summary>
    /// Flag if the enemy is still alive based on its health.
    /// </summary>
	public bool isAlive = true;

    /// <summary>
    /// Reference to the enemy's rigidbody.
    /// </summary>
	private Rigidbody2D rigidBody2d;
    /// <summary>
    /// Reference to the enemy's animator.
    /// </summary>
	[SerializeField] protected Animator enemyAnimator;
    /// <summary>
    /// Reference to the enemy's phantom's animator.
    /// </summary>
	[SerializeField] protected Animator phantomAnimator;
    /// <summary>
    /// Enemy's health.
    /// </summary>
	[SerializeField] protected int hp = 1;
    /// <summary>
    /// Flag if the enemy is vulnerable. Changes based on the room's stability.
    /// </summary>
	[SerializeField] protected bool isVulnerable = false;

    /// <summary>
    /// Key assigned for an instance of this enemy.
    /// Used for recording data.
    /// </summary>
    Tuple<Entry.Type, int> enemyKey;
    Tuple<Entry.Type, int> lcdKey;

    /// <summary>
    /// Reference to the sprite's root transform.
    /// </summary>
	Transform top;

    /// <summary>
	/// Unity Function. Called once upon creation of the object.
	/// </summary>
    void Awake() {
		this.enemyAnimator = GetComponent<Animator> ();
		this.rigidBody2d = GetComponent<Rigidbody2D> ();

		EventBroadcaster.Instance.AddObserver (EventNames.INVULNERABLE_ENEMIES, this.Invulnerable);
		EventBroadcaster.Instance.AddObserver (EventNames.VULNERABLE_ENEMIES, this.Vulnerable);

		Parameters data = new Parameters ();
		data.PutExtra ("enemyHealth", this);

		EventBroadcaster.Instance.PostEvent (EventNames.REGISTER_ENEMY, data);

		top = gameObject.transform.GetChild (0);
		enemyKey = null;
		lcdKey = null;
	}

    /// <summary>
    /// Raises the destroy event.
    /// </summary>
	void OnDestroy() {
		Parameters data = new Parameters ();
		data.PutExtra ("enemyHealth", this);
		EventBroadcaster.Instance.PostEvent (EventNames.UNREGISTER_ENEMY, data);
		EventBroadcaster.Instance.RemoveActionAtObserver (EventNames.INVULNERABLE_ENEMIES, this.Invulnerable);
		EventBroadcaster.Instance.RemoveActionAtObserver (EventNames.VULNERABLE_ENEMIES, this.Vulnerable);
	}

    /// <summary>
    /// Checks if the reference to the animator is not null.
    /// </summary>
    /// <param name="animator">Animator reference</param>
    /// <returns>If the animator is not null. Otherwise, false.</returns>
	bool isAnimatorNotNull(Animator animator) {
		if (animator != null) {
			return true;
		}
		return false;
	}

    /// <summary>
    /// Sets the animator to perform an animation based on a keyword.
    /// </summary>
    /// <param name="animator">Animator Reference</param>
    /// <param name="keyword">Keyword associated with an animation.</param>
	public void setAnimator(Animator animator, string keyword) {
		if (isAnimatorNotNull (animator)) {
			switch (keyword) {
			case ANIMATOR_HIT:
				animator.SetTrigger (ANIMATOR_HIT);	
				break;
			case ANIMATOR_DEATH:
				animator.SetTrigger (ANIMATOR_DEATH);	
				break;
			}
		}
	}

    /// <summary>
    /// Hit the enemy. Decrease this character's health.
    /// 
    /// If the health reaches 0, kill this character.
    /// </summary>
	public void Smash() {
		this.setAnimator (this.enemyAnimator, ANIMATOR_HIT);

		if (this.isVulnerable)
			this.hp--;

		if (this.hp <= 0)
			this.hp = 0;

		if (this.hp == 0)
			Death ();
	}

    /// <summary>
    /// Performs actions when the character is not alive. Plays the death animation.
    /// </summary>
	public virtual void Death() {
		isAlive = false;

		this.enemyAnimator.updateMode = AnimatorUpdateMode.UnscaledTime;
		if (this.rigidBody2d != null) {
			this.rigidBody2d.velocity = new Vector2 (0f, 0f);
		}
		this.setAnimator (this.enemyAnimator, ANIMATOR_DEATH);
		SoundManager.Instance.Play (AudibleNames.Phantom.DEATH, false);
	}

    /// <summary>
    /// Aesthetic function.
    /// Plays this character's phantom animation.
    /// </summary>
	public void CallPhantomDeath() {
		this.phantomAnimator.gameObject.SetActive (true);
		this.setAnimator (this.phantomAnimator, ANIMATOR_DEATH);
	}

    /// <summary>
    /// Called at the end of the death animation. Destroys this game object.
    /// </summary>
    /// <param name="value"></param>
	public void CallDeath(float value) {
		Debug.Log ("DESTROYED");
		Destroy (this.gameObject);
	}

    /// <summary>
    /// Assign a key to this enemy.
    /// </summary>
    /// <param name="key">Key</param>
    public void SetEnemyKey(Tuple<Entry.Type, int> key) {
        this.enemyKey = key;
        Debug.LogWarning("Key set to enemy " + String.Join(" ", key));
    }
    
    /// <summary>
    /// Returns the enemy's key.
    /// </summary>
    /// <returns>Key</returns>
    public Tuple<Entry.Type, int> GetEnemyKey() {
        return enemyKey;
    }
    
    public void SetLCDKey(Tuple<Entry.Type, int> key) {
        this.lcdKey = key;
        Debug.LogWarning("Key set to lcd " + String.Join(" ", key));
    }

    public Tuple<Entry.Type, int> GetLCDKey() {
        return lcdKey;
    }

    /// <summary>
    /// Called when the room is unstable, sets the enemy invulnerable.
    /// </summary>
	protected void Invulnerable() {
		this.isVulnerable = false;
	}

    /// <summary>
    /// Called when the room is stable, sets the enemy vulnerable.
    /// </summary>
	protected void Vulnerable() {
		this.isVulnerable = true;
	}

    /// <summary>
    /// Returns the enemy's current health.
    /// </summary>
    /// <returns>Current Health.</returns>
	public int GetHealth() {
		return this.hp;
	}
}
