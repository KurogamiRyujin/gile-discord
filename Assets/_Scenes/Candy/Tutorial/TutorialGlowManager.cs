using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TutorialGlowManager {
	private GlowManager glowManager;
	public void SubscribeGlowManager(GlowManager manager) {
		this.glowManager = manager;
	}

	public void UnubscribeGlowManager(GlowManager manager) {
		//only the interface that subscribed can unsubscribe
		if (manager == this.glowManager)
			glowManager = null;
	}
	public void StopGlow() {
		#if UNITY_ANDROID
		this.GetGlowManager ().TurnOff ();
		#endif
	}

	public void StopArrowGlow() {
		#if UNITY_ANDROID
		this.GetGlowManager ().StopArrow ();
		#endif
	}
	public void StopStabilityGlow() {
		#if UNITY_ANDROID
		this.GetGlowManager ().StopStability ();
		#endif
	}
	public void StopJumpGlow() {
		#if UNITY_ANDROID
		this.GetGlowManager ().StopJump ();
		#endif
	}
	public void StopGlowCarry() {
		#if UNITY_ANDROID
		this.GetGlowManager ().StopCarry ();
		#endif
	}
	public void StopGlowItem() {
		#if UNITY_ANDROID
		this.GetGlowManager ().StopItem ();
		#endif
	}
	public void GlowArrows() {
		#if UNITY_ANDROID
		this.GetGlowManager ().GlowArrow ();
		Debug.Log ("<color=red>CALLED GLOW 2</color>");
		#endif
	}
	public void GlowSwitch() {
		#if UNITY_ANDROID
		this.GetGlowManager ().GlowSwitch ();
		#endif
	}
	public void GlowJump() {
		#if UNITY_ANDROID
		this.GetGlowManager ().GlowJump ();
		Debug.Log ("<color=red>CALLED GLOW JUMP 2</color>");
		#endif
	}

	public void GlowCarry() {
		#if UNITY_ANDROID
		this.GetGlowManager ().GlowCarry ();
		#endif
	}

	public void GlowItem() {
		#if UNITY_ANDROID
		this.GetGlowManager ().GlowItem ();
		#endif
	}

	public void GlowYarn() {
		#if UNITY_ANDROID
		this.GetGlowManager ().GlowYarn ();
		#endif
	}
	public void GlowHealth() {
		#if UNITY_ANDROID
		this.GetGlowManager ().GlowHealth ();
		#endif
	}
	public void GlowStability() {
		#if UNITY_ANDROID
		this.GetGlowManager ().GlowStability ();
		#endif
	}
	public GlowManager GetGlowManager() {
		return this.glowManager;
	}
}
