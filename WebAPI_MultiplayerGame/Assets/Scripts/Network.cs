using SocketIO;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public class Network : MonoBehaviour {

	static SocketIOComponent socket;

	public Spawner spawner;
	

	void Start () {
		socket = GetComponent<SocketIOComponent>();

		socket.On("open", OnConnected);
		socket.On("talkback", OnTalkBack);
		socket.On("spawn", OnSpawn);
		socket.On("move", OnMove);
		socket.On("disconnected", OnDisconnect);
		socket.On("register", OnRegister);
		socket.On("updatePosition", OnUpdatePosition);
		socket.On("requestPosition", OnRequestPosition);
	}

	private void OnRequestPosition(SocketIOEvent obj) {
		socket.Emit("updatePosition", PosToJson(spawner.localPlayer.transform.position, spawner.localPlayer.transform.rotation.eulerAngles.z));
	}

	private void OnUpdatePosition(SocketIOEvent obj) {
		Debug.Log("Updating Positons " + obj.data);

		//float v = float.Parse(obj.data["v"].ToString().Replace("\"", ""));
		//float h = float.Parse(obj.data["h"].ToString().Replace("\"", ""));
		Vector3 position = MakePositionFromJson(obj);
		float rotation = obj.data["rotZ"].n;

		GameObject player = spawner.FindPlayer(obj.data["id"].str);

		player.transform.position = position;
		player.transform.eulerAngles = new Vector3(0, 0, rotation);
	}

	private void OnRegister(SocketIOEvent obj) {
		Debug.Log("Registered Player " + obj.data);
		spawner.AddPlayer(obj.data["id"].ToString(), spawner.localPlayer);
	}

	private void OnDisconnect(SocketIOEvent obj) {
		Debug.Log("Player Disconnected " + obj.data);

		string id = obj.data["id"].ToString();

		spawner.RemovePlayer(id);
	}

	private void OnMove(SocketIOEvent obj) {
		//Debug.Log("Player Moving " + obj.data);

		string id = obj.data["id"].ToString();

		float v = float.Parse(obj.data["v"].ToString().Replace("\"", ""));
		float h = float.Parse(obj.data["h"].ToString().Replace("\"", ""));

		GameObject player = spawner.FindPlayer(id);
		PlayerMovementNetwork playerMovement = player.GetComponent<PlayerMovementNetwork>();

		playerMovement.v = v;
		playerMovement.h = h;



	}

	private void OnSpawn(SocketIOEvent obj) {
		Debug.Log("Player Spawned With ID " + obj.data);

		GameObject player = spawner.SpawnPlayer(obj.data["id"].ToString());

		// Spawn Existing Players
	}

	private void OnTalkBack(SocketIOEvent obj) {
		Debug.Log("The Server Says \"Hello\" Back");
	}

	private void OnConnected(SocketIOEvent obj) {
		Debug.Log("Connected To Server");

		socket.Emit("sayhello");
	}

	public static void Move(float currentPosV, float currentPosH) {
		//Debug.Log("Send Position To Server " + VectorToJson(currentPos));
		socket.Emit("move", new JSONObject(VectorToJson(currentPosV, currentPosH)));


	}

	public static string VectorToJson(float dirV, float dirH) {
		return string.Format(@"{{""v"":""{0}"",""h"":""{1}""}}", dirV, dirH);
	}

	public static JSONObject PosToJson(Vector3 pos, float rotZ) {
		JSONObject jPos = new JSONObject(JSONObject.Type.OBJECT);
		jPos.AddField("x", pos.x);
		jPos.AddField("y", pos.y);
		jPos.AddField("z", pos.z);
		jPos.AddField("rotZ", rotZ);
		return jPos;
	}

	public static Vector3 MakePositionFromJson(SocketIOEvent e) {
		return new Vector3(e.data["x"].n, e.data["y"].n, e.data["z"].n);
	}

}
