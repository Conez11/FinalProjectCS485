using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;


public class PlayerBrains : MonoBehaviour {

	//public int playerHealth = 100;
	public int currentHealth;
	public int damagePerTime;
	public int timeToDamage;

	//private int maxHealth;
	private int brainsAmount = 5;
	private int healthPerBrain = 20;
	private float elapsedTime;

	public Image[] brainImages;
	public Sprite[] brainSprites;

	public PlayerAttack attack;
	public GameObject gameover;
	private int kills;
	private int killsIndex;


	void Start () {
		attack = gameObject.GetComponent<PlayerAttack> ();
		gameover.SetActive(false);
		currentHealth = brainsAmount * healthPerBrain;
		//maxHealth = brainsAmount * healthPerBrain;
		elapsedTime = 0f;
		kills = 0;
		killsIndex = 0;
		checkHealth ();
	}

	void Update () {
		elapsedTime += Time.deltaTime;
		if (Mathf.Floor (elapsedTime) >= timeToDamage) {
			ModifyHealth (-damagePerTime);
			elapsedTime = 0f;
		}

		kills = attack.killCount;
		if (kills > killsIndex) {
			ModifyHealth (20);
			killsIndex = kills;
		}

	}

	void checkHealth () {
		for (int i = 0; i < brainsAmount; i++) {
			if (brainsAmount <= i) {
				brainImages [i].enabled = false;
			} else {
				brainImages [i].enabled = true;
			}
		}
		UpdateBrains ();
	}

	void UpdateBrains() {
		bool empty = false;
		int i = 0;
		foreach (Image image in brainImages) {
			if (empty) {
				image.sprite = brainSprites [0];
			} else {
				i++;
				if (currentHealth >= i * healthPerBrain) {
					image.sprite = brainSprites [brainSprites.Length - 1];
				} else {
					int currentBrainHealth = (int)(healthPerBrain - (healthPerBrain * i - currentHealth));
					int healthPerImage = healthPerBrain / (brainSprites.Length - 1);
					int imageIndex = currentBrainHealth / healthPerImage;
					image.sprite = brainSprites [imageIndex];
					empty = true;
				}
			}
		}
	}

	public void ModifyHealth(int amount) {
		currentHealth += amount;

		currentHealth = Mathf.Clamp (currentHealth, 0, brainsAmount * healthPerBrain);

		UpdateBrains ();
		if (currentHealth <= 0) {
			gameover.SetActive(true);
			//SceneManager.LoadSceneAsync ("Menu", LoadSceneMode.Single);
		}
	}
		
}
