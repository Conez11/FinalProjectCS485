using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerCamMovement : MonoBehaviour {

	public float moveSpeed;

	Rigidbody rigidbody;
	Camera cam;
	Vector3 velocity;

	void Start () {
		
		rigidbody = GetComponent<Rigidbody> ();
		cam = Camera.main;
	}


	void Update () {
		
		Vector3 mousePos = cam.ScreenToWorldPoint (new Vector3 (Input.mousePosition.x, Input.mousePosition.y, cam.transform.position.y));

		transform.LookAt (mousePos + Vector3.up * transform.position.y);

		velocity = new Vector3 (Input.GetAxis ("Horizontal"), 0.0f, Input.GetAxis ("Vertical")).normalized * moveSpeed;
		
	}

	void FixedUpdate () {

		rigidbody.MovePosition (rigidbody.position + velocity * Time.fixedDeltaTime);
	}
}
