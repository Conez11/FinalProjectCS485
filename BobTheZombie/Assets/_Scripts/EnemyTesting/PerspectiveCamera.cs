using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class BoundaryP {

	public float xMin, xMax, zMin, zMax;
}

public class PerspectiveCamera : MonoBehaviour {

	public Transform player;
	public BoundaryP boundary;

	Vector3 offset;

	void Start () {
		offset = transform.position - player.position;
	}

	void LateUpdate () {

		float xTemp, yTemp, zTemp;
		//float zMinOffsetDif;

		if (player.position.x > boundary.xMax || player.position.x < boundary.xMin) {
			xTemp = transform.position.x;
		} else {
			xTemp = Mathf.Clamp (player.position.x + offset.x, boundary.xMin, boundary.xMax);
		}

		yTemp = player.position.y + offset.y;

		if (player.position.z > boundary.zMax) {
			zTemp = transform.position.z;
		}
		else if ( player.position.z < boundary.zMin) {
			//zMinOffsetDif = player.position.z - boundary.zMin;
			zTemp = transform.position.z;
		} else {
			zTemp = Mathf.Clamp (player.position.z + offset.z, boundary.zMin, boundary.zMax);
		}

		transform.position = new Vector3 (xTemp, yTemp, zTemp);
		offset = transform.position - player.position;

	}
}
