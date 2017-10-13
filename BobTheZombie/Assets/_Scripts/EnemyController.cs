using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyController : MonoBehaviour {

	public float moveSpeed = 5;
	public float turnSpeed = 45;
	public float waitTime = .5f;

	public Transform pathHolder;
	//public Transform player;

	//VisionCast enemyVision;
	Vector3[] waypoints;



	void Start () {
		//enemyVision = GetComponentInChildren<VisionCast> ();

		waypoints = new Vector3[pathHolder.childCount];
		for (int i = 0; i < waypoints.Length; i++) {
			waypoints [i] = pathHolder.GetChild (i).position;
			waypoints [i] = new Vector3 (waypoints [i].x, transform.position.y, waypoints [i].z);
		}

		StartCoroutine (FollowPath (waypoints));
	}
	

	void Update () {
		
	} 


	IEnumerator FollowPath (Vector3[] waypoints) {
		transform.position = waypoints [0];
		int waypointIndex = 1;
		Vector3 targetWaypoint = waypoints [waypointIndex];
		transform.LookAt (targetWaypoint);

		while (true) {
			transform.position = Vector3.MoveTowards (transform.position, targetWaypoint, moveSpeed * Time.deltaTime);

			if (transform.position == targetWaypoint) {
				waypointIndex = (waypointIndex + 1) % waypoints.Length;
				targetWaypoint = waypoints [waypointIndex];
				yield return new WaitForSeconds (waitTime);
				yield return StartCoroutine (TurnToFace (targetWaypoint));
			}
			yield return null;
		}
	}

	IEnumerator TurnToFace (Vector3 lookTarget) {
		Vector3 dirToTarget = (lookTarget - transform.position).normalized;
		float targetAngle = 90 - Mathf.Atan2 (dirToTarget.z, dirToTarget.x) * Mathf.Rad2Deg;

		while (Mathf.Abs (Mathf.DeltaAngle (transform.eulerAngles.y, targetAngle)) > 0.05f) {
			float angle = Mathf.MoveTowardsAngle (transform.eulerAngles.y, targetAngle, turnSpeed * Time.deltaTime);
			transform.eulerAngles = Vector3.up * angle;
			yield return null;
		}
	}


	void OnDrawGizmos () {
		Vector3 startPosition = pathHolder.GetChild (0).position;
		Vector3 previousPosition = startPosition;

		foreach (Transform waypoint in pathHolder) {
			Gizmos.DrawSphere (waypoint.position, 1f);
			Gizmos.DrawLine (previousPosition, waypoint.position);
			previousPosition = waypoint.position;
		}

		Gizmos.DrawLine (previousPosition, startPosition);
	}
}
