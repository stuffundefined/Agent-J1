using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Rocket : MonoBehaviour {
	Rigidbody rigidBody;
	void Start () {
		rigidBody = GetComponent<Rigidbody>();
	}
	void Update () {
		ProcessInput();
	}
	private void ProcessInput() {
		if (Input.GetKey(KeyCode.A)) {
			transform.Rotate(Vector3.forward);
		} else if (Input.GetKey(KeyCode.D)) {
			print("Rotates right");
		}
		if (Input.GetKey(KeyCode.Space)) {
			rigidBody.AddRelativeForce(Vector3.up);
		}
	}
}