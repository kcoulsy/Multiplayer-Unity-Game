using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;

public class PlayerObject : NetworkBehaviour {

	// Use this for initialization
	void Start () {

		if (!isLocalPlayer) {
			return;
		}

		Debug.Log("PlayerObject::Start -- Spawning my own personal unit");

	}
	
	public GameObject PlayerUnitPrefab;

	// Update is called once per frame
	void Update () {
		
	}
}
