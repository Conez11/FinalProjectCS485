using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class PlayerAttack : MonoBehaviour {

	public GameObject Enemy;
	public GameObject map;
	public string targetTag;
	public UnityEngine.UI.Text t;
	public int score=0;

	public int killCount;

	void Start() {
		
		killCount = 0;
		score=PlayerPrefs.GetInt ("Score");
		t.text = "Score: " + score.ToString ();
	}

	void Update()
	{
		/*
		if (Input.GetKeyDown (KeyCode.Space) && GetComponent<BoxCollider> ()) {
			Destroy (Enemy);
			GetComponent<PlayerHealth> ().curHealth += 20;//adds 20 to playerHealth bar and its health bar when kills enemy
	
		}
	*/

		t.text = "Score: " + score.ToString ();
	}

	void OnTriggerStay(Collider other)
	{
		
		if (Input.GetKeyDown (KeyCode.E)&&other.CompareTag(targetTag)) {
			other.gameObject.SetActive (false);
			killCount++;
			score += 100;
			GetComponent<PlayerHealth> ().curHealth += 20;

		}
		if (Input.GetKeyDown (KeyCode.E)&&other.CompareTag ("Exit")) {
			score += 300;
			PlayerPrefs.SetInt ("Score", score);

			SceneManager.LoadSceneAsync( SceneManager.GetActiveScene ().buildIndex);


		}
	}
}
