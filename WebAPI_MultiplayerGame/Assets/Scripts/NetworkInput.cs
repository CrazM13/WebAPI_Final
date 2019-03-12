using System.Collections;
using System.Collections.Generic;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using UnityEngine;
using SocketIO;
using System;

public class NetworkInput : MonoBehaviour {

	public static SocketIOComponent socket;

	string userName;
	public InputField nameInput;

	public GameObject inputForm;
	public GameObject userList;

	public Text listText;

	// Use this for initialization
	void Start () {
		socket = GetComponent<SocketIOComponent>();

		socket.On("connected", OnConnect);
		socket.On("hideForm", OnHideForm);
		socket.On("registrationFailed", OnRegistrationFailed);

		userList.SetActive(false);
	}

	private void OnRegistrationFailed(SocketIOEvent obj) {
		Debug.Log("NAME TAKEN");
	}

	private void OnHideForm(SocketIOEvent obj) {
		inputForm.SetActive(false);
		userList.SetActive(true);

		foreach (JSONObject user in obj.data["users"].list) {
			listText.text += user["name"].str + '\n';
		}
	}

	private void OnConnect(SocketIOEvent obj) {
		Debug.Log("We Are Connected");
	}

	public void GrabFormData() {
		userName = nameInput.text;
		Debug.Log(userName);
		JSONObject data = new JSONObject(JSONObject.Type.OBJECT);
		data.AddField("name", userName);

		socket.Emit("sendData", data);
	}

	public void StartGame() {
		SceneManager.LoadScene(1);
	}

}
