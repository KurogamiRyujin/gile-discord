using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DetonatingPlatform : MonoBehaviour {

	public enum Detonation_Mode
	{
		LEFT_TO_RIGHT,
		RIGHT_TO_LEFT,
		EITHER_SIDES
	}
	public const string DETONATION = "DETONATION";

	private Detonation_Mode latestDetonationMode;

	[SerializeField] private DetonatingPlatformPiece platformPrefab;
	[SerializeField] private int platformWidth;

	private List<DetonatingPlatformPiece> platforms;

	void Awake() {
		this.platforms = new List<DetonatingPlatformPiece> ();
		EventBroadcaster.Instance.AddObserver (EventNames.INITIATE_PLATFORM_DETONATION, this.DetonatePlatforms);
		EventBroadcaster.Instance.AddObserver (EventNames.REPLENISH_DETONATED_PLATFORMS, this.ReplenishPlatforms);
	}

	void OnEnable() {
		for (int i = 0; i < platformWidth; i++) {
			CreatePlatformPortion ();
		}
	}

	void OnDisable() {
		foreach (DetonatingPlatformPiece piece in platforms) {
			piece.Purge ();
		}

		this.platforms.Clear ();
	}

	void OnDestroy() {
		EventBroadcaster.Instance.RemoveObserver (EventNames.INITIATE_PLATFORM_DETONATION);
		EventBroadcaster.Instance.RemoveObserver (EventNames.REPLENISH_DETONATED_PLATFORMS);
	}

	private void CreatePlatformPortion() {
		DetonatingPlatformPiece blockTemp = Instantiate<DetonatingPlatformPiece> (platformPrefab);
		blockTemp.transform.position = this.gameObject.transform.position;
		RectTransform rect = blockTemp.GetComponent<RectTransform> ();
		Vector3 pos = new Vector3 (rect.sizeDelta.x * rect.localScale.x, 0.0f, 0.0f);
		blockTemp.transform.position += (pos * platforms.Count);

		blockTemp.Detonate ();

		blockTemp.Replenish ();

		platforms.Add (blockTemp);
		blockTemp.transform.SetParent (this.gameObject.transform);

	}

	public void DetonatePlatformNumber(int index) {
		this.platforms [index].Detonate ();
	}

	private void DetonatePlatforms(Parameters values) {
		StartCoroutine (Detonating (values.GetPlatformDetonationModeExtra (EventNames.INITIATE_PLATFORM_DETONATION, Detonation_Mode.EITHER_SIDES)));
	}

	private void ReplenishPlatforms() {
		StartCoroutine (Replenishing ());
	}

	private IEnumerator Detonating (Detonation_Mode detonationMode) {
		EventBroadcaster.Instance.PostEvent (EventNames.ROBIN_HOOD_PLATFORMS_DEACTIVATE);

		latestDetonationMode = detonationMode;

		switch (detonationMode) {
		case Detonation_Mode.LEFT_TO_RIGHT:
			for (int i = 0; i < this.platforms.Count; i++) {
				yield return new WaitForSeconds (1.0f);
				this.platforms [i].Detonate ();
			}
			break;
		case Detonation_Mode.RIGHT_TO_LEFT:
			for (int i = this.platforms.Count - 1; i >= 0; i--) {
				yield return new WaitForSeconds (1.0f);
				this.platforms [i].Detonate ();
			}
			break;
		case Detonation_Mode.EITHER_SIDES:
			for (int i = 0; i < (this.platforms.Count+1)/2; i++) {
				yield return new WaitForSeconds (1.0f);
				this.platforms [i].Detonate ();
				this.platforms [this.platforms.Count - (1 + i)].Detonate ();
				yield return new WaitForSeconds (1.0f);
			}
			break;
		}

		EventBroadcaster.Instance.PostEvent (EventNames.ROBIN_HOOD_PLATFORMS_ACTIVATE);
	}

	private IEnumerator Replenishing () {
		switch (latestDetonationMode) {
		case Detonation_Mode.LEFT_TO_RIGHT:
			for (int i = 0; i < this.platforms.Count; i++) {
				yield return new WaitForSeconds (1.0f);
				this.platforms [i].Replenish ();
			}
			break;
		case Detonation_Mode.RIGHT_TO_LEFT:
			for (int i = this.platforms.Count - 1; i >= 0; i--) {
				yield return new WaitForSeconds (1.0f);
				this.platforms [i].Replenish ();
			}
			break;
		case Detonation_Mode.EITHER_SIDES:
			for (int i = 0; i < (this.platforms.Count+1)/2; i++) {
				yield return new WaitForSeconds (1.0f);
				this.platforms [i].Replenish ();
				this.platforms [this.platforms.Count - (1 + i)].Replenish ();
				yield return new WaitForSeconds (1.0f);
			}
			break;
		}
	}
}
