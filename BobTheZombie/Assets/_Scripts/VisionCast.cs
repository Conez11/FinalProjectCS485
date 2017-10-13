using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class VisionCast : MonoBehaviour {

	public Light vision;
	public float visionDistance;
	public LayerMask visionMask;	// LayerMask used for obsticle preventing vision of target
	public Color newVisionColor;
	public string targetTag;		// Tag of target, this variable is not necessary but was used for testing
	public float timeToSpotTarget;	// Time required of constant view to fully see target player

	Transform target;			// Player transform
	float targetVisibleTimer;	// Hold current time that player is in sight
	float visionAngle;
	Color originalVisionColor;

	void Start () {
		target = GameObject.FindGameObjectWithTag (targetTag).transform;	// Find target in this case "player" will be targettag
		visionAngle = vision.spotAngle;										// Vision angle from Spotlight GameObject
		originalVisionColor = vision.color;									// Light Color set in Spotlight GameObject
	}

	void Update () {
		// If target is visiable increase time that target is visiable, else reduce time
		if (CanSeeTarget ()) {
			targetVisibleTimer += Time.deltaTime;
		} else {
			targetVisibleTimer -= Time.deltaTime;
		}

		// Interpolate vision color from no vision of player to the required time to spot player
		targetVisibleTimer = Mathf.Clamp (targetVisibleTimer, 0, timeToSpotTarget);
		vision.color = Color.Lerp (originalVisionColor, newVisionColor, targetVisibleTimer / timeToSpotTarget); 

		if (targetVisibleTimer >= timeToSpotTarget) {

		}
	}

	//  Returns true if 3 checks confirm vision of player is met
	bool CanSeeTarget () {
		// Check that player is within enemy vision distance
		if (Vector3.Distance (transform.position, target.position) < visionDistance) {	
			Vector3 dirTotarget = (target.position - transform.position).normalized;	// Get direction
			float angleTotarget = Vector3.Angle (transform.forward, dirTotarget);		// Get angle
			// Check if player is within enemy vision angle
			if (angleTotarget < visionAngle / 2f) {
				// Check if no objects (such as obsticles) are between enemy and player
				if (!Physics.Linecast (transform.position, target.position, visionMask)) {
					return true;
				}
			}
		}
		return false;
	}

	// Gizmo ray to represent the length of vision distance
	// Important b/c light source fades at the distance increases, this does not
	void OnDrawGizmos () {
		Gizmos.color = Color.magenta;
		Gizmos.DrawRay (transform.position, transform.forward * visionDistance);
	}

}
