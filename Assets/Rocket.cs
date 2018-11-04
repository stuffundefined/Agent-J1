using UnityEngine;
using UnityEngine.SceneManagement;
public class Rocket : MonoBehaviour {
	[SerializeField] float RCSThrust = 100f;
	[SerializeField] float mainThrust = 100f;
	Rigidbody rigidBody;
	AudioSource audioSource;
	enum State { Alive, Failed, Teleporting };
	State state = State.Alive;
	void Start () {
		rigidBody = GetComponent<Rigidbody>();
		audioSource = GetComponent<AudioSource>();
	}
	void Update () {
		if (state != State.Failed) {
			Rotate();
			Thrust();
		}
	}
	private void OnCollisionEnter(Collision collision) {
		if (state != State.Alive) {
			return;
		}
		switch (collision.gameObject.tag) {
			case "Friendly":
				break;
			case "Finish":
				state = State.Teleporting;
				Invoke("LoadNext", 1f);
				break;
			default:
				state = State.Failed;
				Invoke("Fail", 1f);
				break;
		}
	}
	private void Fail() {
		SceneManager.LoadScene(0);
	}
	private void LoadNext() {
		SceneManager.LoadScene(1);
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