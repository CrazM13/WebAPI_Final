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

			float time = ply["besttime"].n;
			string outTime = "";

			int minutes = (int)time / 60;
			int seconds = (int)time % 60;
			outTime = string.Format("{0:00}:{1:00}", minutes, seconds);

			if (ply != null) highScoreText[i].text = ply["name"].str + " - " + outTime + '\n';
		}
	}
}
