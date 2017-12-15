using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class MenuScript : MonoBehaviour {


	public void GameStartEvent()
	{
		SceneManager.LoadSceneAsync ("Unlimited",LoadSceneMode.Single);	
	}



	public void QuitEvent(){
		Application.Quit ();

	}


}
