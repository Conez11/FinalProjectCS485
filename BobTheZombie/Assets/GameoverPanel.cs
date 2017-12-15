using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GameoverPanel : MonoBehaviour {

	public void MenuEvent()
	{
		SceneManager.LoadSceneAsync ("Menu",LoadSceneMode.Single);	
	}

	public void RestartEvent()
	{
		SceneManager.LoadSceneAsync ("Unlimited",LoadSceneMode.Single);	
	}
}
