using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PlayerAttack : MonoBehaviour {
	public GameObject Enemy;
	
	void Update()
	{
		if (Input.GetKeyDown(KeyCode.Space) && GetComponent<BoxCollider>())
		{
			Destroy (Enemy);
		}
	}
}
