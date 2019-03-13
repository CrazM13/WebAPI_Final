using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HighScorePlate : MonoBehaviour {

	private float targetY;
	private float randomTime;

	// Use this for initialization
	void Start () {
		targetY = transform.position.y;
		transform.position = new Vector2(transform.position.x, 500);

		randomTime = Random.Range(0.1f, 2f);

	}
	
	// Update is called once per frame
	void Update () {
		if (randomTime <= 0) transform.position = Vector2.Lerp(transform.position, new Vector2(transform.position.x, targetY), Time.deltaTime);
		else randomTime -= Time.deltaTime;
	}
}
