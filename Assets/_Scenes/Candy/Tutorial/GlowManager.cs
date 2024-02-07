using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// Handles the glow on the UI elements during the tutorial phase. The glows emphasize a particular UI element when the dialogue tutorial talks about it.
/// </summary>
public class GlowManager : MonoBehaviour {
    /// <summary>
    /// Emphasis glow for the left/right direction buttons.
    /// </summary>
	[SerializeField] private ArrowGlow arrowGlow;
    /// <summary>
    /// Emphasis glow for the switch weapon button.
    /// </summary>
	[SerializeField] private SwitchGlow switchGlow;
    /// <summary>
    /// Emphasis glow for the carry button.
    /// </summary>
	[SerializeField] private CarryGlow carryGlow;
    /// <summary>
    /// Emphasis glow for the jump glow.
    /// </summary>
	[SerializeField] private JumpGlow jumpGlow;

    /// <summary>
    /// Emphasis glow for the item UI element.
    /// </summary>
	[SerializeField] private ItemGlow itemGlow;
    /// <summary>
    /// Emphasis glow for the charm UI element.
    /// </summary>
	[SerializeField] private YarnGlow yarnGlow;
    /// <summary>
    /// Emphasis glow for the stability number line.
    /// </summary>
	[SerializeField] private StabilityGlow stabilityGlow;
    /// <summary>
    /// Emphasis glow for the health meter.
    /// </summary>
	[SerializeField] private HealthGlow healthGlow;

    /// <summary>
	/// Unity Function. Called once upon creation of the object.
	/// </summary>
	void Awake() {
		GameController_v7.Instance.GetTutorialGlowManager ().SubscribeGlowManager (this);

	}

    /// <summary>
    /// Activate the directional buttons glow.
    /// </summary>
	public void GlowArrow() {
		
//		this.TurnOff ();
		#if UNITY_ANDROID
		this.GetArrowGlow ().Glow ();
		Debug.Log ("<color=red>CALLED GLOW 3</color>");
		#endif
	}

    /// <summary>
    /// Stop the glow for the stability number line.
    /// </summary>
	public void StopStability() {
		//		this.TurnOff ();
		#if UNITY_ANDROID
		this.GetStabilityGlow ().Close ();
		Debug.Log ("<color=red>CALLED GLOW 3</color>");
		#endif
	}

    /// <summary>
    /// Stop the glow for the directional buttons.
    /// </summary>
	public void StopArrow() {
		//		this.TurnOff ();
		#if UNITY_ANDROID
		this.GetArrowGlow ().Close ();
		Debug.Log ("<color=red>CALLED GLOW 3</color>");
		#endif
	}

    /// <summary>
    /// Activate the glow for the carry button.
    /// </summary>
	public void GlowCarry() {
		//		this.TurnOff ();
		#if UNITY_ANDROID
		this.GetCarryGlow ().Glow ();
		#endif
	}

    /// <summary>
    /// Activate the glow for the jump button.
    /// </summary>
	public void GlowJump() {
		//		this.TurnOff ();
		#if UNITY_ANDROID
		this.GetJumpGlow ().Glow ();
		Debug.Log ("<color=red>CALLED GLOW JUMP 3</color>");
		#endif
	}

    /// <summary>
    /// Stop the glow for the jump button.
    /// </summary>
	public void StopJump() {
		#if UNITY_ANDROID
		this.GetJumpGlow ().Close ();
		#endif
	}

    /// <summary>
    /// Stop the glow for the carry button.
    /// </summary>
	public void StopCarry() {
		#if UNITY_ANDROID
		this.GetCarryGlow ().Close ();
		#endif
	}

    /// <summary>
    /// Stop the glow for the item UI element.
    /// </summary>
	public void StopItem() {
		#if UNITY_ANDROID
		this.GetItemGlow ().Close ();
		#endif
	}

    /// <summary>
    /// Activate the glow for the switch weapons button.
    /// </summary>
	public void GlowSwitch() {
		//		this.TurnOff ();
		#if UNITY_ANDROID
		this.GetSwitchGlow ().Glow ();
		#endif
	}

    /// <summary>
    /// Activate the glow for the item UI element.
    /// </summary>
	public void GlowItem() {
		//		this.TurnOff ();
		#if UNITY_ANDROID
		this.GetItemGlow ().Glow ();
		#endif
	}

    /// <summary>
    /// Activate the glow for the charm UI element.
    /// </summary>
	public void GlowYarn() {
		//		this.TurnOff ();
		#if UNITY_ANDROID
		this.GetYarnGlow ().Glow ();
		#endif
	}

    /// <summary>
    /// Activate the glow for the stability number line.
    /// </summary>
	public void GlowStability() {
		//		this.TurnOff ();
		#if UNITY_ANDROID
		this.GetStabilityGlow ().Glow ();
		#endif
	}
    
    /// <summary>
    /// Activate the glow for the health meter.
    /// </summary>
	public void GlowHealth() {
		//		this.TurnOff ();
		#if UNITY_ANDROID
		this.GetHealthGlow ().Glow ();
		#endif
	}

    /// <summary>
    /// Stop all glows.
    /// </summary>
	public void TurnOff() {
		#if UNITY_ANDROID
		this.GetArrowGlow ().Close ();
		this.GetCarryGlow ().Close ();
		this.GetJumpGlow ().Close ();

		this.GetSwitchGlow ().Close ();
		this.GetItemGlow ().Close ();
		this.GetYarnGlow ().Close ();
		this.GetStabilityGlow ().Close ();
		this.GetHealthGlow ().Close ();
		#endif
	}

    /// <summary>
    /// Returns the arrow glow behaviour.
    /// </summary>
    /// <returns>Arrow Glow Behaviour</returns>
	public ArrowGlow GetArrowGlow() {
		if (this.arrowGlow == null) {
			this.arrowGlow = GetComponentInChildren<ArrowGlow> ();
		}
		return this.arrowGlow;
	}

    /// <summary>
    /// Returns the switch weapons button glow behaviour.
    /// </summary>
    /// <returns>Switch Weapons Buton Glow Behaviour</returns>
	public SwitchGlow GetSwitchGlow() {
		if (this.switchGlow == null) {
			this.switchGlow = GetComponentInChildren<SwitchGlow> ();
		}
		return this.switchGlow;
	}

    /// <summary>
    /// Returns the carry button glow behaviour.
    /// </summary>
    /// <returns>Carry Button Glow Behaviour</returns>
	public CarryGlow GetCarryGlow() {
		if (this.carryGlow == null) {
			this.carryGlow = GetComponentInChildren<CarryGlow> ();
		}
		return this.carryGlow;
	}

    /// <summary>
    /// Returns the jump button glow behaviour.
    /// </summary>
    /// <returns>Jump Button Glow Behaviour</returns>
	public JumpGlow GetJumpGlow() {
		if (this.jumpGlow == null) {
			this.jumpGlow = GetComponentInChildren<JumpGlow> ();
		}
		return this.jumpGlow;
	}

    /// <summary>
    /// Returns the item glow behaviour.
    /// </summary>
    /// <returns>Item Glow Behaviour</returns>
	public ItemGlow GetItemGlow() {
		if (this.itemGlow == null) {
			this.itemGlow = GetComponentInChildren<ItemGlow> ();
		}
		return this.itemGlow;
	}

    /// <summary>
    /// Returns the charm UI element glow behaviour.
    /// </summary>
    /// <returns>Charm UI Element Glow Behaviour</returns>
	public YarnGlow GetYarnGlow() {
		if (this.yarnGlow == null) {
			this.yarnGlow = GetComponentInChildren<YarnGlow> ();
		}
		return this.yarnGlow;
	}

    /// <summary>
    /// Returns the stability number line glow behaviour.
    /// </summary>
    /// <returns>Stability Number Line Glow Behaviour</returns>
	public StabilityGlow GetStabilityGlow() {
		if (this.stabilityGlow == null) {
			this.stabilityGlow = GetComponentInChildren<StabilityGlow> ();
		}
		return this.stabilityGlow;
	}

    /// <summary>
    /// Returns the health meter glow behaviour.
    /// </summary>
    /// <returns>Health Meter Glow Behaviour</returns>
	public HealthGlow GetHealthGlow() {
		if (this.healthGlow == null) {
			this.healthGlow = GetComponentInChildren<HealthGlow> ();
		}
		return this.healthGlow;
	}
}
