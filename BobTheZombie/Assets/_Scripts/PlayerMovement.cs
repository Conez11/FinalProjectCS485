using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerMovement : MonoBehaviour {

	public float moveSpeed;
	public float turnSpeed;
	public float smoothTime;

	private float angle;
	private float smoothMagnitude;
	private float smoothVelocity;

	Vector3 velocity;
	Rigidbody rigidbody;

	void Start () {
		rigidbody = GetComponent<Rigidbody> ();
	}

	void Update () {

		Vector3 inputDirection = new Vector3 (Input.GetAxis ("Horizontal"), 0, Input.GetAxis ("Vertical")).normalized;
		float inputMagnitude = inputDirection.magnitude;
		smoothMagnitude = Mathf.SmoothDamp (smoothMagnitude, inputMagnitude, ref smoothVelocity, smoothTime);

		float targetAngle = Mathf.Atan2 (inputDirection.x, inputDirection.z) * Mathf.Rad2Deg;
		angle = Mathf.LerpAngle (angle, targetAngle, Time.deltaTime * turnSpeed * inputMagnitude);

		velocity = transform.forward * moveSpeed * smoothMagnitude;
	}

	void FixedUpdate () {

		rigidbody.MoveRotation (Quaternion.Euler (Vector3.up * angle));
		rigidbody.MovePosition (rigidbody.position + velocity * Time.fixedDeltaTime); 
	}


}
