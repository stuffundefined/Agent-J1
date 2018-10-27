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
		Rotate();
		Thrust();
	}
	private void Rotate() {
		rigidBody.freezeRotation = true;
		if (Input.GetKey(KeyCode.A)) {
			transform.Rotate(Vector3.forward);
		} else if (Input.GetKey(KeyCode.D)) {
			transform.Rotate(-Vector3.forward);
		}
		rigidBody.freezeRotation = false;
	}
	void Thrust() {
		if (Input.GetKey(KeyCode.Space)) {
			rigidBody.AddRelativeForce(Vector3.up);
			if (!audioSource.isPlaying) {
				audioSource.Play();
			}
		} else {
			audioSource.Stop();
		}
	}
}