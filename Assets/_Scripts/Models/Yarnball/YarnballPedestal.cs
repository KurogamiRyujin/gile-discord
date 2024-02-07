using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// Behaviour for the charm pedestals. They hold the charms which are picked up by the player avatar to allow it to slice sky blocks with the needle.
/// </summary>
public class YarnballPedestal : MonoBehaviour {

    /// <summary>
    /// Reference to the charm prefab.
    /// </summary>
	public Yarnball yarnballPrefab;
    /// <summary>
    /// Current charm the pedestal is holding.
    /// </summary>
	[HideInInspector] public Yarnball yarnball;
	[SerializeField] float offsetX = 0.7f;
	[SerializeField] float offsetY = 15.7f;
	[SerializeField] Transform pickupPos;
	private GameObject particleParent;
	private ParticleEffect particleEffect;

	void Awake() {
		this.particleEffect = GetComponentInChildren<ParticleEffect> ();
	}

    /// <summary>
    /// Spawns a charm based on the given denominator.
    /// </summary>
    /// <param name="denominator">Denominator</param>
	public void Open(int denominator) {
		yarnball = Instantiate<Yarnball> (yarnballPrefab, pickupPos.position, Quaternion.identity);
		yarnball.SetPedestal (this);
		yarnball.SetDenominator (denominator);

		yarnball.transform.SetParent (this.gameObject.transform);
		yarnball.gameObject.AddComponent<Levitation> ();
		this.EnableEffects ();
//		this.particleParent = GetComponentInChildren<ParticleEffect> ().gameObject;
//		this.particleEffect = particleParent.GetComponent<ParticleEffect> ();
	}

	public void DisableEffects() {
		if (this.particleEffect == null)
			this.particleEffect = GetComponentInChildren<ParticleEffect> ();
		this.particleEffect.Stop ();
//		this.particleParent.SetActive (false);
	}

	public void EnableEffects() {
		if (this.particleEffect == null)
			this.particleEffect = GetComponentInChildren<ParticleEffect> ();
//		this.particleParent.SetActive (true);
		this.particleEffect.Play ();
	}

    /// <summary>
    /// Destroys the charm this pedestal is holding.
    /// </summary>
	public void Close() {
		if (yarnball != null) {
			Destroy (yarnball.gameObject);
		}
		this.DisableEffects ();
	}
}
