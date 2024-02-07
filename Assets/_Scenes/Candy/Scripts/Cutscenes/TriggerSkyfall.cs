using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// Implementation of a single trigger observer. If triggered, destabilizes the room.
/// </summary>
public class TriggerSkyfall : SingleTriggerObserver {
	/// <summary>
	/// Sky block where the sky fragments used to fill the specified hollow block comes from.
	/// </summary>
	[SerializeField] private SkyBlock skyBlock;
	/// <summary>
	/// Hollow block manipulated in the cutscene.
	/// </summary>
	[SerializeField] private HollowBlock hollowBlock;

	/// <summary>
	/// Standard Unity Function. Called once in the lifetime of the component once it is enabled.
	/// </summary>
	void Start() {
		this.GetSkyBlock ().Hide ();
//		this.GetSkyBlock ().gameObject.SetActive (false);
	}

	/// <summary>
	/// Action to do when this has been triggered.
	/// </summary>
	public override void Action() {
		//		this.GetSkyBlock ().gameObject.SetActive (true);
//		this.GetSkyBlock ().Show ();
		this.GetSkyBlock ().Drop ();
		SoundManager.Instance.Play (AudibleNames.Results.BREAK, false);
	}

	// Safety measure check. Must not be called. Ideally assigned in Inspector.
	/// <summary>
	/// Gets the sky block.
	/// </summary>
	/// <returns>The sky block.</returns>
	public SkyBlock GetSkyBlock() {
		if(this.skyBlock == null) {
			GameObject.FindObjectsOfType<SkyBlock> ();
		}
		return this.skyBlock;
	}

	/// <summary>
	/// Gets the hollow block.
	/// </summary>
	/// <returns>The hollow block.</returns>
	public HollowBlock GetHollowBlock() {
		if(this.hollowBlock == null) {
			GameObject.FindObjectsOfType<HollowBlock> ();
		}
		return this.hollowBlock;
	}
}
