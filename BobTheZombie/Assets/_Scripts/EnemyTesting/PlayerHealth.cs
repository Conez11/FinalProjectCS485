using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerHealth : MonoBehaviour {
	public int maxHealth = 100;
	public int curHealth = 100;
	//public float health;
	public float healthBarLength;
	public GameObject Player;

	// Use this for initialization
	void Start () {
		healthBarLength = Screen.width / 2;
	}
	
	// Update is called once per frame
	void Update ()
	{
		AddjustCurrentHealth(0);

		if (curHealth < 0) {
			Destroy (Player);
		}

	}
	
	void OnGUI() {
		GUI.Box(new Rect(10, 40, healthBarLength,20), curHealth + "/" + maxHealth);
	}
	
	public void AddjustCurrentHealth(int adj) {
		curHealth += adj;
	
		//players is destroyed when health bar is 0 
		if (curHealth <= 0) {
			EnemeyAI.isPlayerAlive = false;
			curHealth = 0;

			Destroy (Player);

		}

		if(curHealth > maxHealth)
			curHealth = maxHealth;
		
		if(maxHealth < 1)
			maxHealth = 1;

		healthBarLength = (Screen.width / 4);
		//	* (curHealth / (float)maxHealth);
	}
}