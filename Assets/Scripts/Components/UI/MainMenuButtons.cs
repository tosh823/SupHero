using UnityEngine;
using System.Collections;

namespace SupHero.Components.UI {
    public class MainMenuButtons : MonoBehaviour {

		/*void Start(){
			StartCoroutine(ChangeScene());
		}*/

		public void ChangeScene(string sceneName) {
			
			Game.Instance.loadLevel();
        }



    }
}
