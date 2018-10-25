﻿using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Rocket : MonoBehaviour {
	Rigidbody rigidBody;
	AudioSource audioSource;
	void Start () {
		rigidBody = GetComponent<Rigidbody>();
		audioSource = GetComponent<AudioSource>();
	}
	void Update () {
		ProcessInput();
	}
	private void ProcessInput() {
		if (Input.GetKey(KeyCode.A)) {
			transform.Rotate(Vector3.forward);
		} else if (Input.GetKey(KeyCode.D)) {
			transform.Rotate(-Vector3.forward);
		}
		if (Input.GetKey(KeyCode.Space)) {
			rigidBody.AddRelativeForce(Vector3.up);
		}
	}
}