using System.Collections;
using System.Collections.Generic;
using UnityEngine.SceneManagement;
using UnityEngine;

public class SceneChanger : MonoBehaviour {

	public void LoadHighScoreScene() {
		SceneManager.LoadScene(2);
	}

	public void LoadUserInputScene() {
		SceneManager.LoadScene(0);
	}

}
