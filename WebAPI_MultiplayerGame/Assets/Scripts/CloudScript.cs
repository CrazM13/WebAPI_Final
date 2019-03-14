using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CloudScript : MonoBehaviour {

	private Vector2 startPos;
	private Vector2 targetPos;

	public float speed = 1;

	// Use this for initialization
	void Start () {
		startPos = transform.position;
		targetPos = startPos * new Vector2(-1, 1);
	}
	
	// Update is called once per frame
	void Update () {
		transform.Translate(-transform.right * Time.deltaTime * speed);

		if (transform.position.x < targetPos.x) {
			transform.position = startPos;
		}
	}
}
