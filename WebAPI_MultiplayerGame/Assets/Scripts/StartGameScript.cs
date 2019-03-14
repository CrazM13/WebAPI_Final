using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StartGameScript : MonoBehaviour {
	// Use this for initialization
	void Start () {
		SocketScript.socket.Emit("gameStart");
	}
}
