using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using SocketIO;
using System;

public class Spawner : MonoBehaviour {

	public GameObject localPlayer;
	public GameObject playerPrefab;
	public SocketIOComponent socket;

	private Dictionary<string, GameObject> players = new Dictionary<string, GameObject>();

	public GameObject SpawnPlayer(string id) {
		GameObject player = Instantiate(playerPrefab, Vector3.zero, Quaternion.identity);

		id = id.Replace("\"", "");

		player.GetComponent<NetworkEntity>().id = id;
		AddPlayer(id, player);

		return player;
	}

	public void AddPlayer(string id, GameObject player) {
		id = id.Replace("\"", "");

		players.Add(id, player);
	}

	public GameObject FindPlayer(string id) {
		id = id.Replace("\"", "");

		return players[id];
	}

	public void RemovePlayer(string id) {
		id = id.Replace("\"", "");

		GameObject player = players[id];

		Destroy(player);
		players.Remove(id);
	}

}
