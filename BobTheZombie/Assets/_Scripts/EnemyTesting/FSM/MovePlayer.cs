using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MovePlayer : MonoBehaviour {

	public Transform camera;
	Rigidbody rigidbody;
	Vector3 velocity;
	public float moveSpeed;

	void Start () {
		
	}

	void Update () {
		Vector3 direction = Vector3.zero;

		if (Input.GetKey (KeyCode.W)) {
			direction += camera.forward * Time.deltaTime;
		}
		if (Input.GetKey (KeyCode.S)) {
			direction -= camera.forward * Time.deltaTime;
		}
		if (Input.GetKey (KeyCode.D)) {
			direction += camera.right * Time.deltaTime;
		}
		if (Input.GetKey (KeyCode.A)) {
			direction -= camera.right * Time.deltaTime;
		}
			
		direction.y = 0f;
		transform.position += direction.normalized * moveSpeed * Time.deltaTime;

	}

	void FixedUpdate () {

	}

}
