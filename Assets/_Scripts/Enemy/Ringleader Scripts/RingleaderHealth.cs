using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// Inherits enemy health. Imlpements the Ringleader's specific health functionalities.
/// </summary>
public class RingleaderHealth : EnemyHealth {

    /// <summary>
    /// Reference to the Ringleader's animatable class.
    /// </summary>
	private RingleaderAnimatable ringleaderAnimatable;
    /// <summary>
    /// Reference to the Ringleader's controller.
    /// </summary>
    private RingleaderController ringleaderController;

    /// <summary>
	/// Unity Function. Called once upon creation of the object.
	/// </summary>
    void Awake() {
        ringleaderController = GetComponent<RingleaderController>();
        ringleaderAnimatable = GetComponent<RingleaderAnimatable> ();
		EventBroadcaster.Instance.AddObserver (EventNames.INVULNERABLE_ENEMIES, this.Invulnerable);
		EventBroadcaster.Instance.AddObserver (EventNames.VULNERABLE_ENEMIES, this.Vulnerable);

		Parameters data = new Parameters ();
		data.PutExtra ("enemyHealth", this);
		EventBroadcaster.Instance.PostEvent (EventNames.REGISTER_ENEMY, data);
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
    /// Performs actions when the character is not alive. Plays the death animation.
    /// </summary>
	public override void Death () {
        StartCoroutine(LastWords());
		//isAlive = false;
		//ringleaderAnimatable.Death ();
	}
    
    /// <summary>
    /// Special to the Ringleader.
    /// 
    /// Upon death, triggers the Ringleader's last dialogue.
    /// </summary>
    /// <returns>None</returns>
    public IEnumerator LastWords() {
        yield return StartCoroutine(this.ringleaderController.HitRoutine());

        SoundManager.Instance.Play(AudibleNames.BreakableBox.GLASS, false);
        SoundManager.Instance.Play(AudibleNames.Phantom.DEATH, false);
        EventBroadcaster.Instance.PostEvent(EventNames.RINGLEADER_DEATH);

        isAlive = false;
        ringleaderAnimatable.Death();

        //yield return StartCoroutine(this.ringleaderController.RetryRoutine());
        yield return null;
    }
}
