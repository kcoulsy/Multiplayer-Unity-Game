using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;

public class PlayerConnectionObject : NetworkBehaviour {

	// Use this for initialization
	void Start () {

		if (!isLocalPlayer) {
			return;
		}

		Debug.Log("PlayerObject::Start -- Spawning my own personal unit");

		CmdSpawnMyUnit();

	}
	
	public GameObject PlayerUnitPrefab;

	// Update is called once per frame
	void Update () {
		// Runs on everyones componuter regardless of who's object it is
		if (Input.GetKeyDown(KeyCode.S)) {
			CmdSpawnMyUnit();
		}
	}

	/**  		#### COMMANDS ####
	*
	* 	Commands are functions that ONLY get called by the server
	* 	Must have [Command] before and start with Cmd
	*/

	[Command]
	void CmdSpawnMyUnit() {

		GameObject go = Instantiate(PlayerUnitPrefab);

		NetworkServer.SpawnWithClientAuthority(go, connectionToClient);
	}

}
