using SocketIO;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SocketScript : MonoBehaviour {

	public static SocketIOComponent socket;

	// Use this for initialization
	void Awake () {
		socket = GetComponent<SocketIOComponent>();

		DontDestroyOnLoad(gameObject);
	}
}
