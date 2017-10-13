using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class VisionCast : MonoBehaviour {

	public Light vision;
	public float visionDistance;
	public LayerMask visionMask;
	public Color newVisionColor;
	public string targetTag;
	public float timeToSpotTarget;

	Transform target;
	float targetVisibleTimer;
	float visionAngle;
	Color originalVisionColor;

	void Start () {
		target = GameObject.FindGameObjectWithTag (targetTag).transform;
		visionAngle = vision.spotAngle;
		originalVisionColor = vision.color;
	}

	void Update () {
		if (CanSeeTarget ()) {
			targetVisibleTimer += Time.deltaTime;
		} else {
			targetVisibleTimer -= Time.deltaTime;
		}

		targetVisibleTimer = Mathf.Clamp (targetVisibleTimer, 0, timeToSpotTarget);
		vision.color = Color.Lerp (originalVisionColor, newVisionColor, targetVisibleTimer / timeToSpotTarget);

		if (targetVisibleTimer >= timeToSpotTarget) {

		}
	}

	bool CanSeeTarget () {
		if (Vector3.Distance (transform.position, target.position) < visionDistance) {
			
			Vector3 dirTotarget = (target.position - transform.position).normalized;
			float angleTotarget = Vector3.Angle (transform.forward, dirTotarget);

			if (angleTotarget < visionAngle / 2f) {
				
				if (!Physics.Linecast (transform.position, target.position, visionMask)) {
					
					return true;
				}
			}
		}
		return false;
	}

	void OnDrawGizmos () {
		Gizmos.color = Color.magenta;
		Gizmos.DrawRay (transform.position, transform.forward * visionDistance);
	}

}
