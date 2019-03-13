using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerMovement : MonoBehaviour {

	Rigidbody2D rb2d;

	void Start() {
		rb2d = GetComponent<Rigidbody2D>();
	}

	void FixedUpdate() {
		//transform.Rotate(0, 0, -Input.GetAxisRaw("Horizontal") * 100f * Time.deltaTime);

		Network.Move(Input.GetAxisRaw("Vertical"), Input.GetAxisRaw("Horizontal"));

		if (Input.GetAxisRaw("Vertical") != 0) {
			rb2d.AddForce(transform.up * 3f * Input.GetAxisRaw("Vertical"));
		} else {
			rb2d.velocity = new Vector2(rb2d.velocity.x, 0);
		}

		if (Input.GetAxisRaw("Horizontal") != 0) {
			rb2d.AddForce(transform.right * 3f * Input.GetAxisRaw("Horizontal"));
		} else {
			rb2d.velocity = new Vector2(0, rb2d.velocity.y);
		}
	}

	private void OnTriggerEnter2D(Collider2D collision) {
		transform.position = Vector2.zero;
		Network.SendDeath();
	}
}
