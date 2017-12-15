using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyController : MonoBehaviour {

	public float moveSpeed = 5;
	public float turnSpeed = 45;
	public float waitTime = .5f;

	public Transform pathHolder;

	public Vector3[] waypoints;

	void Start () {
		// Initialize waypoint in pathHolder for enemy to follow
		waypoints = new Vector3[pathHolder.childCount];
		for (int i = 0; i < waypoints.Length; i++) {
			waypoints [i] = pathHolder.GetChild (i).position;
			waypoints [i] = new Vector3 (waypoints [i].x, transform.position.y, waypoints [i].z);
		}

		StartCoroutine (FollowPath (waypoints));	// Begin following path
	}
	

	void Update () {
			
	} 


	public IEnumerator FollowPath (Vector3[] waypoints) {
		transform.position = waypoints [0];			//Enemy starts at first waypoint
		int waypointIndex = 1;
		Vector3 targetWaypoint = waypoints [waypointIndex];		// next waypoint
		transform.LookAt (targetWaypoint);						// look at waypoint		(Not sure if this is necessary with the turn to face method below)

		//************** Needs to be updated to allow state change to attack player
		// Move to next waypoint
		while (true) {
			transform.position = Vector3.MoveTowards (transform.position, targetWaypoint, moveSpeed * Time.deltaTime);
			if (transform.position == targetWaypoint) {
				waypointIndex = (waypointIndex + 1) % waypoints.Length;
				targetWaypoint = waypoints [waypointIndex];
				yield return new WaitForSeconds (waitTime);					//Wait for waittime then continue coroutine
				yield return StartCoroutine (TurnToFace (targetWaypoint));	//Start Coroutine and turn to face next waypoint
			}
			yield return null;
		}
	}

	//
	public IEnumerator TurnToFace (Vector3 lookTarget) {
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
