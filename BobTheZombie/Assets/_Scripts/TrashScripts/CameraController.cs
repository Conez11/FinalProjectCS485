using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraController : MonoBehaviour {

	//public float turnSpeed;

	public Transform player;

	private Vector3 offset;
	private Quaternion rotationOffset;

	void Start ()
	{
		offset = transform.position - player.transform.position;
		rotationOffset = transform.rotation;
	}

	void LateUpdate ()
	{
		transform.position = player.position + offset;
		transform.rotation = player.rotation * rotationOffset; 
		//float movement = Input.GetAxis ("Horizontal") * turnSpeed * Time.deltaTime;

		//if (!Mathf.Approximately (movement, 0f)) {
			//transform.RotateAround (player.position, Vector3.up, movement);
			//offset = transform.position - player.position;
		//}
	}

}
