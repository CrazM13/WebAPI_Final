using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PopperDart : MonoBehaviour {

	public float speed = 1;

	// Use this for initialization
	void Start () { /*MT*/ }
	
	// Update is called once per frame
	void Update () {
		transform.position += transform.up * speed * Time.deltaTime;
	}
}
