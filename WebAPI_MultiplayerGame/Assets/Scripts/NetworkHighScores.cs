using SocketIO;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using System;

public class NetworkHighScores : MonoBehaviour {

	public static SocketIOComponent socket;

	public Text[] highScoreText;

	// Use this for initialization
	void Start() {
		socket = SocketScript.socket;

		socket.On("recieveHighScores", OnRecieveHighScores);

		socket.Emit("requestHighScores");
	}

	private void OnRecieveHighScores(SocketIOEvent obj) {

		Debug.Log(obj.data);

		for (int i = 0; i < 10; i++) {
			JSONObject ply = obj.data["users"][i];
			if (ply != null) highScoreText[i].text = ply["name"].str + " - " + ply["deaths"].n + '\n';
		}
	}
}
