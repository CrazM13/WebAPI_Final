using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerMovementNetwork : MonoBehaviour {

	Rigidbody2D rb2d;

	public float v;
	public float h;

	void Start() {
		rb2d = GetComponent<Rigidbody2D>();
	}

	public void FixedUpdate() {
		//transform.Rotate(0, 0, -h * 100f * Time.deltaTime);

		//Network.Move(v, h);

		if (v != 0) {
			rb2d.AddForce(transform.up * 3f * v);
		} else {
			rb2d.velocity = new Vector2(rb2d.velocity.x, 0);
		}

		if (h != 0) {
			rb2d.AddForce(transform.right * 3f * h);
		} else {
			rb2d.velocity = new Vector2(0, rb2d.velocity.y);
		}
	}
}
