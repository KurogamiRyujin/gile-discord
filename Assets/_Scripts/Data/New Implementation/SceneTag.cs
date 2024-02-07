using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SceneTag : MonoBehaviour {

	public SceneTopic topic = SceneTopic.NONE;

	public SceneTopic GetSceneTopic() {
		return topic;
	}
}
