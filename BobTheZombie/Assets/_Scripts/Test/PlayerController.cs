using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController : MonoBehaviour {

	public float moveSpeed;
	public float rotateSpeed;

	public Transform camera;

	void Start () 
	{
		//rb = GetComponent<Rigidbody> ();
	}

	void Update ()
	{

		Vector3 moveDirection = Vector3.zero;

		if (Input.GetKey (KeyCode.W)) {
			moveDirection += camera.forward;
		}
		if (Input.GetKey (KeyCode.S)) {
			moveDirection += -camera.forward;
		}
		if (Input.GetKey (KeyCode.A)) {
			transform.rotation *= Quaternion.RotateTowards (Quaternion.Euler(0,1,0), Quaternion.Euler(0,-1,0), rotateSpeed * Time.deltaTime * 45);
		}
		if (Input.GetKey (KeyCode.D)) {
			transform.rotation *= Quaternion.RotateTowards (Quaternion.Euler(0,-1,0), Quaternion.Euler(0,1,0), rotateSpeed * Time.deltaTime * 45);
		}
		moveDirection.y = 0.0f;
		transform.position += moveDirection.normalized * moveSpeed * Time.deltaTime;
	}

	void FixedUpdate ()
	{
		
	}

}
