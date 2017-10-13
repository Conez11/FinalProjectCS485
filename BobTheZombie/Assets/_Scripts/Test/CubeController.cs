using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CubeController : MonoBehaviour {

	public float speed;

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

		//moveDirection.right = 0.0f;
		moveDirection.y = 0.0f;

		transform.position += moveDirection.normalized * speed * Time.deltaTime;
	}
}
