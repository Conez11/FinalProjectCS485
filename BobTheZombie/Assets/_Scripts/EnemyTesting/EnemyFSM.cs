using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class EnemyFSM : MonoBehaviour {

	public enum EnemyState {
		None,
		Patrol,
		Chase,
		Attack,
		Backoff,
		Dead,
	}

	public Transform pathHolder;
	public EnemyVision vision;

	EnemyState curState;
	EnemyState prevState;
	Transform player;
	Vector3[] waypoints;
	Vector3 lastWaypoint;
	NavMeshAgent pathfinder;


	public float moveSpeed;
	public float turnSpeed;
	public float turnSpeedAttacking;
	public float attackSpeed;
	public float attackRange;
	public float attackRangeMax;
	public float patrolWait;

	float elapsedTime;
	int health;	//eventually own script?
	bool isDead;


	void Start () {
		curState = EnemyState.Patrol;
		prevState = EnemyState.None;
		elapsedTime = 0.0f;
		health = 100;		
		isDead = false;

		vision = gameObject.GetComponent<EnemyVision> ();
		pathfinder = gameObject.GetComponent<NavMeshAgent> ();

		waypoints = new Vector3[pathHolder.childCount];
		for (int i = 0; i < waypoints.Length; i++) {
			waypoints [i] = pathHolder.GetChild (i).position;
			waypoints [i] = new Vector3 (waypoints [i].x, transform.position.y, waypoints [i].z);
		}

		player = GameObject.FindGameObjectWithTag ("Player").transform;
	}

	void Update () {
		switch (curState) {
			case EnemyState.Patrol:
				UpdatePatrol ();
				break;
			case EnemyState.Chase:
				UpdateChase ();
				break;
			case EnemyState.Attack:
				UpdateAttack ();
				break;
			case EnemyState.Backoff:
				UpdateBackoff ();
				break;
			case EnemyState.Dead:
				UpdateDead ();
				break;
		}

		if (health <= 0) {
			curState = EnemyState.Dead;
		}

		elapsedTime += Time.deltaTime;
	}

	void UpdatePatrol () {
		if (curState != prevState) {
			StartCoroutine (FollowPath (waypoints));
		}
		prevState = curState;

		if (vision.playerAcquired) {
			curState = EnemyState.Chase;
		}
	}

	void UpdateChase () {
		if (curState != prevState) {
			StartCoroutine (ChasePath ());
		}
		prevState = curState;
	}

	void UpdateAttack () {

	}

	void UpdateBackoff () {

	}

	void UpdateDead () {

	}

	IEnumerator FollowPath (Vector3[] waypoints) {
		transform.position = waypoints [0];			//Enemy starts at first waypoint
		int waypointIndex = 1;
		Vector3 targetWaypoint = waypoints [waypointIndex];		// next waypoint
		lastWaypoint = targetWaypoint;
		transform.LookAt (targetWaypoint);						// initialize look at very next waypoint

		//************** Needs to be updated to allow state change to attack player
		// Move to next waypoint
		while (true) {	// true needs new logic?
			transform.position = Vector3.MoveTowards (transform.position, targetWaypoint, moveSpeed * Time.deltaTime);
			if (transform.position == targetWaypoint) {
				waypointIndex = (waypointIndex + 1) % waypoints.Length;
				targetWaypoint = waypoints [waypointIndex];
				yield return new WaitForSeconds (patrolWait);					//Wait for waittime then continue coroutine
				yield return StartCoroutine (TurnToFace (targetWaypoint));		//Start Coroutine and turn to face next waypoint
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

	IEnumerator ChasePath () {
		float refreshRate = .25f;

		while (player != null) {
			Vector3 playerPosition = new Vector3 (player.position.x, 0, player.position.z);
			if (!isDead) {
				pathfinder.SetDestination (playerPosition);
				//TESTING
			}
			yield return new WaitForSeconds (refreshRate);
		}
		yield return null;
	}

/*	IEnumerator Attacking () { // NEEDS REVIEW
		Quaternion rotToPlayer = Quaternion.LookRotation (player.position - transform.position);
		transform.rotation = Quaternion.Slerp (transform.rotation, rotToPlayer, Time.deltaTime * turnSpeedAttacking);

		if (elapsedTime > attackSpeed) {
			Debug.Log ("Attack");
			elapsedTime = 0;
		}
		yield return null;
	}*/

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
