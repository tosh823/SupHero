using UnityEngine;
using System.Collections;

namespace SupHero {
    public class MainMenuButtons : MonoBehaviour {

        public void ChangeScene(string sceneName) {
            Game.Instance.loadLevel();
        }
    }
}
