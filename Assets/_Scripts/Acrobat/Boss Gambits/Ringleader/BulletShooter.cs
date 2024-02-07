using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// Class which instantiates objects with Bullet behaviour.
/// </summary>
public class BulletShooter : MonoBehaviour {

    /// <summary>
    /// Game object prefab for a Bullet Object
    /// </summary>
	[SerializeField] private Bullet bulletPrefab;
	
    /// <summary>
    /// Shoots a normal bullet given a damage value.
    /// </summary>
    /// <param name="damage"></param>
	public void ShootBullet(int damage) {
		Bullet bullet = Instantiate<Bullet> (bulletPrefab, transform.position, transform.localRotation);
		bullet.gameObject.GetComponent<BulletDamage> ().SetDamage (damage);
	}
}
