using UnityEngine;
using System.Collections;
using UnityEngine.UI;


public class FadeInOut : MonoBehaviour {

	// Use this for initialization
	public void LoadNextLevel (string name) {
		StartCoroutine (LevelLoad(name));
	}
	
	// Update is called once per frame
	IEnumerator LevelLoad (string name) {
		yield return new WaitForSeconds (1f);
		Application.LoadLevel (name);
	}
}
