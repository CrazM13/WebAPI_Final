using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DartPool : MonoBehaviour {

	public GameObject popperDartPrefab;

	public int maxNumberOfDarts = 1;

	private Queue<GameObject> darts;

	// Use this for initialization
	void Start () {
		darts = new Queue<GameObject>();

		while (darts.Count < maxNumberOfDarts) {
			GameObject popperDart = Instantiate(popperDartPrefab);
			popperDart.SetActive(false);
			darts.Enqueue(popperDart);
		}
	}
	
	// Update is called once per frame
	void Update () {}

	public void ShootDart(Vector2 position) {
		GameObject dart = darts.Dequeue();
		dart.SetActive(true);
		dart.transform.position = position;
		darts.Enqueue(dart);
	}
}
