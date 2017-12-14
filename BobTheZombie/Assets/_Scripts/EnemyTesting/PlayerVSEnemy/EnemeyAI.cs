using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemeyAI : MonoBehaviour {
	public Transform player;
	public float playerDistance;
	public float rotationDamping;
	public float moveSpeed;
	public static bool isPlayerAlive = true;


	

	void Update ()
	{
		if (isPlayerAlive) {


			playerDistance = Vector3.Distance (player.position, transform.position);
			//if player is less than 20f looks
			if (playerDistance < 20f) {
				lookAtPlayer ();
			}
			//if distance is less than 17 it chases
			if (playerDistance < 17f) {

				//chases player
				if (playerDistance > 5f) {
					chase ();
				} 
				//attacks it
				else if (playerDistance < 5f) {
					attack ();
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
		transform.Translate (Vector3.forward * moveSpeed * Time.deltaTime);

	}
	void attack()
	{
		RaycastHit hit;

		if (Physics.Raycast (transform.position, transform.forward, out hit)) {
			if (hit.collider.gameObject.tag == "Player") {
				//lowers players health by 1 
				hit.collider.gameObject.GetComponent<PlayerBrains> ().ModifyHealth(-1);//gets component from player health script	
			}

		}
	}
}
