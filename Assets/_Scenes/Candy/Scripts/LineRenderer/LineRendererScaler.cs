using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LineRendererScaler : MonoBehaviour {
//	[SerializeField] private LineRenderer[] lineRenderers;
//	[SerializeField] private float[] originalWidths;
//	void Awake () {
//		this.lineRenderers = GetComponentsInChildren<LineRenderer> ();
//		this.RecordWidth ();
//	}
//	void RecordWidth() {
//		this.originalWidths = new float[lineRenderers.Length];
//		int i = 0;
//		foreach (LineRenderer renderer in this.lineRenderers) {
//			originalWidths [i] = renderer.startWidth;
//			i++;
//		}
//	}
//	void Update () {
//		this.AdjustLineRendererWidth ();
//	}
//	public void AdjustLineRendererWidth() {
//		int i = 0;
//		foreach (LineRenderer renderer in this.lineRenderers) {
//			renderer.startWidth = originalWidths[i] * CameraZoom.MAX_ZOOM_IN / CameraZoom.MAX_ZOOM_OUT;
//			i++;
//		}
//	}
}
