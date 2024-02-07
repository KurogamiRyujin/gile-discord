using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AIController : MonoBehaviour {

	private List<Puppet> puppets;

	// Use this for initialization
	void Awake () {
		puppets = new List<Puppet> ();
	}
	
	// Update is called once per frame
	void Update () {
		foreach (Puppet puppet in puppets) {
			puppet.Obey ();
		}
	}

	public void ReceiveNewPuppet(Puppet puppet) {
		puppets.Add (puppet);
	}

	public void RemovePuppet(Puppet puppet) {
		puppets.Remove (puppet);
	}
}
