using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class MenuScript : MonoBehaviour {


	public void GameStartEvent()
	{
		SceneManager.LoadScene ("MapTest2",LoadSceneMode.Single);	
	}



	public void QuitEvent(){
		Application.Quit ();

	}


}
