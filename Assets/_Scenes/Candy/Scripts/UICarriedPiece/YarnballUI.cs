using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class YarnballUI : MonoBehaviour {
	public const string YARNBALL_VALUE = "yarnballValue";
	[SerializeField] private TextMeshProUGUI yarnballValue;
	[SerializeField] private YarnballImage yarnballImage;
	[SerializeField] private ItemContainerAnimatable itemContainer;
	[SerializeField] private float value;

	void Awake() {
		EventBroadcaster.Instance.AddObserver (EventNames.ON_DENOMINATOR_CHANGE, UpdateYarnballValue);
		EventBroadcaster.Instance.AddObserver (EventNames.OPEN_YARNBALL, ShowYarnballImage);
	}
	void OnDestroy() {
		EventBroadcaster.Instance.RemoveObserver (EventNames.ON_DENOMINATOR_CHANGE);
		EventBroadcaster.Instance.RemoveObserver (EventNames.OPEN_YARNBALL);
	}
	void Update() {
		this.CheckValue();
	}

	public void UpdateYarnballValue(Parameters parameters) {
		float denominator = parameters.GetFloatExtra (YARNBALL_VALUE, 0f);
		this.value = denominator;
		this.GetYarnballValue ().SetText (""+denominator);

		this.CheckValue ();
	}

	public void CheckValue() {
		if (this.value == 0) {
			this.HideYarnballImage ();
		} else {
			this.ShowYarnballImage ();
		}
	}


	public ItemContainerAnimatable GetItemContainer() {
		// Must not enter this, assign ItemContainerAnimatable in inspector;
		if (this.itemContainer == null) {
			this.itemContainer = gameObject.GetComponentInParent<ItemContainerAnimatable> ();
		}
		return this.itemContainer;
	}

	public void HideYarnballImage() {
		if (this.GetYarnballImage () != null) {
			if (this.GetItemContainer () != null) {
				this.GetItemContainer ().SingleIdle ();
				//			this.GetYarnballImage ().gameObject.SetActive (false);
			}
		}
	}
	public void ShowYarnballImage() {
		if (this.GetYarnballImage () != null) {
			this.GetItemContainer ().DoubleIdle ();
//			this.GetYarnballImage ().gameObject.SetActive (true);
		}
	}
	public YarnballImage GetYarnballImage() {
		if (this.yarnballImage == null) {
			this.yarnballImage = GetComponentInChildren<YarnballImage> ();
		}
		return this.yarnballImage;
	}

	public TextMeshProUGUI GetYarnballValue() {
//		if (this.yarnballValue == null) {
//			this.yarnballValue =
//				GetComponentInChildren<YarnballValue> ().gameObject.GetComponent<TextMeshProUGUI> ();
//
//		}
		return this.yarnballValue;
	}

}
