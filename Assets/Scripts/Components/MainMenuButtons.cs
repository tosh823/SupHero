using UnityEngine;
using System.Collections;

public class MainMenuButtons : MonoBehaviour {

	public void ChangeScene (string sceneName)
	{
		Application.LoadLevel(sceneName);
	}
		
}
