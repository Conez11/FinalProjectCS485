using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyVision : MonoBehaviour {

	public Light vision;
	public LayerMask obstacleMask;
	public Color newVisionColor;

	public float visionDistance;
	public float timeToAcquire;
	public bool playerAcquired;
	public bool playerLost;
	public float timeToLose;

	Transform player;
	Color visionColor;

	float visionTimer;
	float visionAngle;

	void Start () {
		player = GameObject.FindGameObjectWithTag ("Player").transform;
		visionAngle = vision.spotAngle;
		visionColor = vision.color;
		playerAcquired = false;
	}

	void Update () {
		if (PlayerVisible ()) {
			visionTimer += Time.deltaTime;
		} else {
			visionTimer -= Time.deltaTime;
		}

		visionTimer = Mathf.Clamp (visionTimer, 0f, timeToAcquire);
		vision.color = Color.Lerp (visionColor, newVisionColor, visionTimer / timeToAcquire);

		if (visionTimer == timeToAcquire) {
			playerAcquired = true;
			PlayerSpotted ();
		} else {
			playerAcquired = false;
			if (visionTimer == timeToLose) {
				playerLost = true;
			}
		}

	}

	bool PlayerVisible () {
		
		if (Vector3.Distance (transform.position, player.position) < visionDistance) {
			
			Vector3 directionToPlayer = (player.position - transform.position).normalized;
			float angleToPlayer = Vector3.Angle (transform.forward, directionToPlayer);

			if (angleToPlayer <= visionAngle / 2f) {

				if (!Physics.Linecast (transform.position, player.position, obstacleMask)) {

					return true;
				}
			}
		}

		return false;
	}

	void PlayerSpotted () {
	}

	void OnDrawGizmos () {
		Gizmos.color = Color.red;
		Gizmos.DrawRay (transform.position, transform.forward * visionDistance);
	}
}
