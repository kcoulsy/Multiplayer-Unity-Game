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

	// Sync vars are variables that change on clients if changed on the server
	[SyncVar(hook="OnPlayerNameChanged")]
	public string PlayerName = "Anonymous";

	// Update is called once per frame
	void Update () {
		// Runs on everyones componuter regardless of who's object it is
		if (Input.GetKeyDown(KeyCode.S)) {
			CmdSpawnMyUnit();
		}

		if (Input.GetKeyDown(KeyCode.Q)) {
			string n = "user_" + Random.Range(1, 1000);
			Debug.Log("Sending the server a request to change our name to " + n);
			CmdChangePlayerName(n);
		}
	}

	// IF using a hook on a SyncVar, local var is not updated.
	void OnPlayerNameChanged(string newName) {
		Debug.Log("OnPlayerNameChanged: " + PlayerName + " -> " + newName);
		gameObject.name = "PlayerConnectionObject [" + newName + "]";
		PlayerName = newName;
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

	[Command]
	void CmdChangePlayerName(string n) {
		PlayerName = n;
		Debug.Log("CmdChangePlayerName: " + n);

		// Maybe check there are no blacklisted words
		// if yes, do we ignore the request and do nothing
		// or still call the rpc with the original etc

		// Tell other clients what the players name is
		// RpcChangePlayerName(n);
	}


	/**  		#### RPC ####
	*
	* 	Special functions that only get executed on the clients
	* 	Must have [ClientRPC] before and start with Rpc
	*/

	// [ClientRpc]
	// void RpcChangePlayerName(string n) {
	// 	 Debug.Log("RpcChangePlayerName: " + n);
	// 	 PlayerName = n;
	// }
}
