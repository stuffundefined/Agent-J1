﻿using System;
using UnityEngine;
using UnityEngine.SceneManagement;
public class Rocket : MonoBehaviour {
	[SerializeField] float RCSThrust = 100f;
	[SerializeField] float mainThrust = 100f;
	[SerializeField] float levelLoadDelay = 2f;
	[SerializeField] AudioClip thrustS;
	[SerializeField] AudioClip deathS;
	[SerializeField] AudioClip finishS;
	[SerializeField] ParticleSystem thrustP;
	[SerializeField] ParticleSystem deathP;
	[SerializeField] ParticleSystem finishP;
	Rigidbody rigidBody;
	AudioSource audioSource;
	enum State { Alive, Failed, Teleporting };
	State state = State.Alive;
	bool collisionsDisabled = false;
	void Start () {
		rigidBody = GetComponent<Rigidbody>();
		audioSource = GetComponent<AudioSource>();
	}
	void Update () {
		if (state == State.Alive) {
			RespondToRotateKeys();
			RespondToSpace();
		}
		if (Debug.isDebugBuild) {
			RespondToDebugKeys();
		}
	}
	private void RespondToDebugKeys() {
		if (Input.GetKeyDown(KeyCode.L)) {
			LoadNext();
		} else if (Input.GetKeyDown(KeyCode.C)) {
			collisionsDisabled = !collisionsDisabled;
		}
	}
	private void OnCollisionEnter(Collision collision) {
		if (state != State.Alive || collisionsDisabled) { return; }
		switch (collision.gameObject.tag) {
			case "Friendly":
				break;
			case "Finish":
				Succeed();
				break;
			default:
				Fail();
				break;
		}
	}
	private void Fail() {
		state = State.Failed;
		audioSource.Stop();
		audioSource.PlayOneShot(deathS);
		deathP.Play();
		Invoke("GoBack", levelLoadDelay);
	}
	private void Succeed() {
		state = State.Teleporting;
		audioSource.Stop();
		audioSource.PlayOneShot(finishS);
		finishP.Play();
		Invoke("LoadNext", levelLoadDelay);
	}
	private void GoBack() {
		SceneManager.LoadScene(0);
	}
	private void LoadNext() {
		int currentScene = SceneManager.GetActiveScene().buildIndex;
		int nextSceneIndex = currentScene + 1;
		if (nextSceneIndex == SceneManager.sceneCountInBuildSettings) {
			nextSceneIndex = 0;
		}
		SceneManager.LoadScene(nextSceneIndex);
	}
	private void RespondToRotateKeys() {
		rigidBody.freezeRotation = true;
		float rotationThisFrame = RCSThrust * Time.deltaTime;
		if (Input.GetKey(KeyCode.A)) {
			transform.Rotate(Vector3.forward * rotationThisFrame);
		} else if (Input.GetKey(KeyCode.D)) {
			transform.Rotate(-Vector3.forward * rotationThisFrame);
		}
		rigidBody.freezeRotation = false;
	}
	private void RespondToSpace() {
		if (Input.GetKey(KeyCode.Space)) {
			ApplyThrust();
		} else {
			audioSource.Stop();
			thrustP.Stop();
		}
	}
	private void ApplyThrust() {
		rigidBody.AddRelativeForce(Vector3.up * mainThrust);
		if (!audioSource.isPlaying) {
			audioSource.PlayOneShot(thrustS);
		}
		thrustP.Play();
	}
}