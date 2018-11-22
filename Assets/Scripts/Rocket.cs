using System;
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
	[SerializeField]bool alive = true;
	bool collisionsDisabled = false;
	void Start () {
		rigidBody = GetComponent<Rigidbody>();
		audioSource = GetComponent<AudioSource>();
		if (SceneManager.GetActiveScene().buildIndex == SceneManager.sceneCountInBuildSettings) {
			alive = false;
		}
	}
	void Update () {
		if (alive) {
			RespondToRotateKeys();
			RespondToSpace();
		}
		if (Debug.isDebugBuild) {
			RespondToDebugKeys();
		}
		RespondToQuitKey();
	}
	private void RespondToQuitKey() {
		if (!alive && SceneManager.GetActiveScene().buildIndex == SceneManager.sceneCountInBuildSettings - 1 && Input.GetKey(KeyCode.Space)) {
			Application.Quit();
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
		if (!alive || collisionsDisabled) { return; }
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
		alive = false;
		audioSource.Stop();
		audioSource.PlayOneShot(deathS);
		deathP.Play();
		Invoke("Restart", levelLoadDelay);
	}
	private void Succeed() {
		alive = false;
		audioSource.Stop();
		audioSource.PlayOneShot(finishS);
		finishP.Play();
		Invoke("LoadNext", levelLoadDelay);
	}
	private void Restart() {
		SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
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
		if (Input.GetKey(KeyCode.A)) {
			Rotate(RCSThrust * Time.deltaTime);
		} else if (Input.GetKey(KeyCode.D)) {
			Rotate(-RCSThrust * Time.deltaTime);
		}
	}
	private void Rotate(float amount) {
		rigidBody.freezeRotation = true;
		transform.Rotate(Vector3.forward * amount);
		rigidBody.freezeRotation = false;
	}
	private void RespondToSpace() {
		if (Input.GetKey(KeyCode.Space)) {
			Thrust();
		} else {
			StopThrust();
		}
	}
	private void StopThrust() {
		audioSource.Stop();
		thrustP.Stop();
	}
	private void Thrust() {
		rigidBody.AddRelativeForce(Vector3.up * mainThrust);
		if (!audioSource.isPlaying) {
			audioSource.PlayOneShot(thrustS);
		}
		thrustP.Play();
	}
}