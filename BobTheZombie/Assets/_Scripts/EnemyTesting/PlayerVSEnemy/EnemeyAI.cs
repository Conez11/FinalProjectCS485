using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemeyAI : MonoBehaviour {
	public Transform player;
	public float playerDistance;
	public float rotationDamping;
	public float moveSpeed;
	public static bool isPlayerAlive = true;

	public float attackSpeed = 2;
	public float nextAttack;
	public int damage;

	EnemyController controller;
	EnemyVision vision;

	void Start () {

		vision = gameObject.GetComponent<EnemyVision> ();
		controller = gameObject.GetComponent<EnemyController> ();

	}


	void Update ()
	{
		nextAttack += Time.deltaTime;
		if (isPlayerAlive) {


			playerDistance = Vector3.Distance (player.position, transform.position);
			//if player is less than 20f looks
//			if ((playerDistance < 20f) && vision.playerAcquired) {
//				lookAtPlayer ();
//			}
			//if distance is less than 17 it chases
			if (vision.playerAcquired) {

				//chases player
				if (playerDistance > 5f) {
					chase ();
				} 
				//attacks it
				else if (playerDistance < 5f) {
					attack ();
				}

				 if (vision.playerLost) {
					StartCoroutine (controller.FollowPath (controller.waypoints));
				}
		
			}
		}
	
	
	}
	void lookAtPlayer ()
	{

		Quaternion rotation=Quaternion.LookRotation(player.position-transform.position);
		transform.rotation = Quaternion.Slerp (transform.rotation, rotation,Time.deltaTime*rotationDamping);

	}

	void chase()
	{
		lookAtPlayer ();
		transform.Translate (Vector3.forward * moveSpeed * Time.deltaTime);

	}
	void attack()
	{
		if ( nextAttack >= attackSpeed) {
			GameObject.FindGameObjectWithTag("Player").GetComponent<PlayerBrains> ().ModifyHealth(-damage);//gets component from player health script
			nextAttack = 0;
		}



	}
}
