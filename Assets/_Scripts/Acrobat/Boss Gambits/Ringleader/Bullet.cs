using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// Bullet behaviour where the Bullet objects continuously moves forward until it expires.
/// </summary>
public class Bullet : MonoBehaviour {

    /// <summary>
    /// Bullet flight speed
    /// </summary>
	[SerializeField] private float speed = 10.0f;
    /// <summary>
    /// Distance before expiration.
    /// </summary>
	[SerializeField] private float maxDistance = 100.0f;

    /// <summary>
    /// Bullet's origin
    /// </summary>
	private Vector3 originalPos;

    /// <summary>
    /// Unity function. Called once the game object the script is attached to is instantiated.
    /// </summary>
	void Awake() {
		originalPos = transform.position;
	}

	/// <summary>
    /// Standard Unity function. Called every frame and serves as the MonoBehaviour's core thread.
    /// </summary>
	void Update () {
		transform.position = Vector3.MoveTowards (transform.position, transform.position + transform.right, speed * Time.deltaTime);

		if (Vector3.Distance (originalPos, transform.position) >= maxDistance)
			Expire ();
	}

    /// <summary>
    /// Bullet expiration. Called when the Bullet object needs to be destroyed.
    /// </summary>
	private void Expire() {
		Destroy (this.gameObject);
	}
}
