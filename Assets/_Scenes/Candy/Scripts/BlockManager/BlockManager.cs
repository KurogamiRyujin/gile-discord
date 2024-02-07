using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// General Block manager class. Contains any block manager that is and will be implemented.
/// </summary>
public class BlockManager {
	/// <summary>
	/// Hollow Block Manager instance.
	/// </summary>
	private HollowBlockManager hollowBlockManager;

	/// <summary>
	/// Subscribes the hollow block manager.
	/// </summary>
	/// <param name="manager">Manager.</param>
	public void SubscribeHollowBlockManager (HollowBlockManager manager) {
		this.hollowBlockManager = manager;
	}

	/// <summary>
	/// Unsubscribes the hollow block manager.
	/// </summary>
	/// <param name="manager">Manager.</param>
	public void UnsubscribeHollowBlockManager(HollowBlockManager manager) {
		//only the interface that subscribed can unsubscribe
		if (manager == this.hollowBlockManager)
			hollowBlockManager = null;
	}

	/// <summary>
	/// Takes the hollow block skin.
	/// </summary>
	/// <returns>The hollow block skin.</returns>
	public SkinPackage TakeHollowBlockSkin() {
		return this.hollowBlockManager.TakeSkin ();
	}
}
