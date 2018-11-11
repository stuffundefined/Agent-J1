﻿using UnityEngine;
using UnityEngine.SceneManagement;
public class Rocket : MonoBehaviour {
	[SerializeField] float RCSThrust = 100f;
	[SerializeField] float mainThrust = 100f;
	[SerializeField] AudioClip thrust;
	[SerializeField] AudioClip death;
	[SerializeField] AudioClip finish;
	Rigidbody rigidBody;
	AudioSource audioSource;
	enum State { Alive, Failed, Teleporting };
	State state = State.Alive;
	void Start () {
		rigidBody = GetComponent<Rigidbody>();
		audioSource = GetComponent<AudioSource>();
	}
	void Update () {
		if (state == State.Alive) {
			RespondToRotateKeys();
			RespondToSpace();
		}
	}
	private void OnCollisionEnter(Collision collision) {
		if (state != State.Alive) { return; }
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
		audioSource.PlayOneShot(death);
		Invoke("GoBack", 1f);
	}
	private void Succeed() {
		state = State.Teleporting;
		audioSource.Stop();
		audioSource.PlayOneShot(finish);
		Invoke("LoadNext", 1f);
	}
	private void GoBack() {
		SceneManager.LoadScene(0);
	}
	private void LoadNext() {
		SceneManager.LoadScene(1);
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
		}
	}
	private void ApplyThrust() {
		rigidBody.AddRelativeForce(Vector3.up * mainThrust);
		if (!audioSource.isPlaying) {
			audioSource.PlayOneShot(thrust);
		}
	}
}