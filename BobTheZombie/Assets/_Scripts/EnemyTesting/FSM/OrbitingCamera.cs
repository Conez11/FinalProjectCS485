using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class OrbitingCamera : MonoBehaviour {

	public Transform player;
	//Transform parent;

	Vector3 offset;
	Quaternion currentRotation;

	//public float smoothPosition = 100f;
	//public float smoothRotation = 100f;
	public float orbitSpeed = 5f;
	public float angleHelp = 15f;
	float angleX;
	float angleY;
	float angleZ;



	
	void Start () {

		//parent = GetComponentInParent<Transform> ();

		offset = player.position - transform.position;

		Vector3 angles = transform.eulerAngles;
		angleX = angles.x;
		angleY = angles.x;
		angleZ = angles.z;



		currentRotation = Quaternion.Euler (angleX, angleY, angleZ);
		transform.rotation = currentRotation;
		angleY = angles.x;

	}

	void LateUpdate () {

		angleY = Input.GetAxis ("Mouse X") * orbitSpeed;
		player.transform.Rotate (0, angleY, 0);

		float desiredAngle = player.transform.eulerAngles.y;
		Quaternion rotation = Quaternion.Euler (angleX - angleHelp, desiredAngle, 0);
		//Quaternion smoothedRotation = Quaternion.Slerp (transform.rotation, rotation, smoothRotation * Time.deltaTime);
		transform.position = player.transform.position - (rotation * offset);
		transform.LookAt (player.transform);

	}
}
