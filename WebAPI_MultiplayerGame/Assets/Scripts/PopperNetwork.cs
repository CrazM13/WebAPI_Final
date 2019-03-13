using SocketIO;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PopperNetwork : MonoBehaviour {

	private float timeUntilUpdate = 5f;

	private float targetPosition = 0;
	private bool shouldShoot = false;

	public GameObject popperDartPrefab;

	// Use this for initialization
	void Start () {
		targetPosition = transform.position.x;
	}
	
	// Update is called once per frame
	void Update () {

		timeUntilUpdate -= Time.deltaTime;

		if (timeUntilUpdate <= 0) {
			Network.RequestPopperUpdate();
			timeUntilUpdate += 5f;
		} else {
			transform.position = Vector2.Lerp(transform.position, new Vector2(targetPosition, transform.position.y), Time.deltaTime);
		}
	}

	public void SetProperties(JSONObject properties) {
		targetPosition = (properties["position"].f * 16) - 8;
		shouldShoot = properties["shouldShoot"].b;

		if (shouldShoot) Shoot();
	}

	private void Shoot() {
		GameObject popperDart = Instantiate(popperDartPrefab);
		popperDart.transform.position = transform.position;

		shouldShoot = false;
	}
}
