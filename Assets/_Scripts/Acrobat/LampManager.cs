using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// UNUSED CLASS
/// 
/// Handles the lamp that is associated with a detonating platform piece. Toggles its platform piece to either be visible or invisible.
/// </summary>
public class LampManager : MonoBehaviour {
    /// <summary>
    /// Determines how long the platform piece will be illuminated.
    /// </summary>
	[SerializeField] private float LIGHT_DURATION = 2.0f;

    /// <summary>
    /// Reference to the detonating platform piece.
    /// </summary>
	[SerializeField] private DetonatingTrampolinePlatformPiece detonatingPlatform;
    /// <summary>
    /// Reference to the particle effects.
    /// </summary>
	[SerializeField] private ParticleEffect lampLights;
    /// <summary>
    /// Reference to the trampoline.
    /// </summary>
	[SerializeField] private TrampolinePlatform trampoline;
    /// <summary>
    /// Flag if the lamp is on and is illuminating the platform.
    /// </summary>
	[SerializeField] private bool isOn;

    /// <summary>
    /// Standard Unity Function. Starts up the MonoBehaviour. Called only once throughout its life.
    /// </summary>
	void Start () {
		CheckIfNull ();
		this.isOn = false;
		Refresh ();
	}

    /// <summary>
    /// Reinitializes the platform to its unsolved, invisible state.
    /// </summary>
	public void Refresh() {
		detonatingPlatform.Detonate ();
		HidePlatform ();
	}

    /// <summary>
    /// Checker to ensure the platform components are not null.
    /// </summary>
	void CheckIfNull() {
		if (detonatingPlatform == null) {
			detonatingPlatform = gameObject.GetComponentInChildren<DetonatingTrampolinePlatformPiece> ();
		}
		if (lampLights == null) {
			ParticleEffect[] particleEffects = gameObject.GetComponentsInChildren<ParticleEffect> ();
			lampLights = particleEffects [0]; // To ensure it won't be null
			foreach (ParticleEffect effect in particleEffects) {
				if (effect.GetEffectType () == ParticleEffect.EffectType.LAMP_LIGHTS)
					lampLights = effect;
			}


		}
		if(trampoline == null) {
			trampoline = gameObject.GetComponentInChildren<TrampolinePlatform> ();
			trampoline.SetLampManager (this);
		}
	}

    /// <summary>
    /// Illuminates the lamp lights.
    /// </summary>
	void PlayLights() {
		lampLights.Play ();
	}

    /// <summary>
    /// Command to turn the lights on.
    /// </summary>
	void TurnOn() {
		this.isOn = true;
		PlayLights ();
	}

    /// <summary>
    /// Command to turn off the lights.
    /// </summary>
	void TurnOff() {
		this.isOn = false;
	}

    /// <summary>
    /// Makes the platform invisible.
    /// </summary>
	void HidePlatform() {
		this.detonatingPlatform.Hide ();
		HideTrampoline (); // TODO: Temporarily removed
	}

    /// <summary>
    /// Makes the platform visible.
    /// </summary>
	void ShowPlatform() {
		this.detonatingPlatform.Show ();
		ShowTrampoline (); // TODO: Temporarily removed
	}

    /// <summary>
    /// Coroutine to switch the lamp on for a certain duration before turning it on. This also causes the platform to be visible for the same duration.
    /// </summary>
    /// <returns></returns>
	IEnumerator SwitchOnRoutine() {
		this.TurnOn ();
		ShowPlatform ();

		yield return new WaitForSeconds (LIGHT_DURATION);
		if(detonatingPlatform.IsFilling()) {
			Debug.Log ("Det. Platform is filling (LampManager.cs)");
			while (detonatingPlatform.IsFilling()) {
				yield return null;
			}
			Debug.Log ("Det. Platform DONE filling (LampManager.cs)");
		}

		if (detonatingPlatform.IsTangible ()) {
			trampoline.Activate ();
		}
		else {
			Refresh ();
		}
		this.TurnOff ();
	}

    /// <summary>
    /// Illuminates the trampoline to be visible.
    /// </summary>
	void ShowTrampoline() {
		this.trampoline.gameObject.SetActive (true);
		this.trampoline.Show ();
	}
    
    /// <summary>
    /// Makes the trampoline invisible.
    /// </summary>
	void HideTrampoline() {
		this.trampoline.Deactivate ();
		this.trampoline.gameObject.SetActive (false);
	}
    /// <summary>
    /// Unity Function. Trigger collision that checks if the player has approached the platform.
    /// </summary>
    /// <param name="other"></param>
	void OnTriggerEnter2D(Collider2D other) {
		if (other.gameObject.GetComponent<PlayerYuni>() != null) {
			if (!this.isOn) {
				StartCoroutine (SwitchOnRoutine());
			}
		}
	}
}
