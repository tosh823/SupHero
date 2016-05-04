using UnityEngine;
using System.Collections;

public class Fading : MonoBehaviour {

	public Texture2D fadeOutTexture; //texture that overlays the screen
	public float fadeSpeed = 0.8f;

	private int drawDepth = -1000; //texture's order in draw hierarchy
	private float alpha = 1.0f; //alpha value between 0 and 1
	private int fadeDir = -1; // direction to fade

	void OnGUI () {
		// fade out/in the alpha value using a direction, a speed and time.deltatime to convert operation to seconds
		alpha += fadeDir * fadeSpeed * Time.deltaTime;
		// force the number between 0 and 1 because GUI.color uses alpha values between 0 and 1
		alpha = Mathf.Clamp01(alpha);

		// set color of our GUI (texture). All color values remain the same and the alpha in set to alpha variable
		GUI.color = new Color (GUI.color.r, GUI.color.g, GUI.color.b, alpha); //set the alpha value
		GUI.depth = drawDepth; // make the black texture render on top
		GUI.DrawTexture (new Rect (0, 0, Screen.width, Screen.height), fadeOutTexture); // draw the teture to fit the entire screen
	}

	//sets fadeDir to the direction parameter making the scene fade in if -1 and out if 1
	public float BeginFade (int direction) {
		fadeDir = direction;
		return (fadeSpeed); // return the fadeSpeed variable so it's easy to time the application-LoadLevel();
	}

	// OnLevelWasLoaded is called when a level is loaded. It takes loaded level index (int) as a parameter so you can limit the fade in to certain scenes
	void Start (){
		//alpha = 1; // use this if the alpha is not set to 1 by default
		BeginFade(-1); // call the fade in function
	}

}
