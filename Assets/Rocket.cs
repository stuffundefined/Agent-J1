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
			print("Rotates left");
		} else if (Input.GetKey(KeyCode.D)) {
			print("Rotates right");
		}
		if (Input.GetKey(KeyCode.Space)) {
			print("Thrusts");
		}
	}
}