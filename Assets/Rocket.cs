using UnityEngine;
using UnityEngine.SceneManagement;

public class Rocket : MonoBehaviour {
	[SerializeField] float RCSThrust = 100f;
	[SerializeField] float mainThrust = 100f;
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
	private void OnCollisionEnter(Collision collision) {
		switch(collision.gameObject.tag) {
			case "Friendly":
				break;
			case "Finish":
				print("yay");
				break;
			default:
				print("die");
				break;
		}
	}
	private void Rotate() {
		rigidBody.freezeRotation = true;
		float rotationThisFrame = RCSThrust * Time.deltaTime;
		if (Input.GetKey(KeyCode.A)) {
			transform.Rotate(Vector3.forward * rotationThisFrame);
		} else if (Input.GetKey(KeyCode.D)) {
			transform.Rotate(-Vector3.forward * rotationThisFrame);
		}
		rigidBody.freezeRotation = false;
	}
	void Thrust() {
		if (Input.GetKey(KeyCode.Space)) {
			rigidBody.AddRelativeForce(Vector3.up * mainThrust);
			if (!audioSource.isPlaying) {
				audioSource.Play();
			}
		} else {
			audioSource.Stop();
		}
	}
}