using UnityEngine;
using UnityEngine.SceneManagement;
using SupHero.Components.Level;
using System;

namespace SupHero {
    public class Game {

        private static Game instance;

        public static Game Instance {
            get {
                if (instance == null) {
                    instance = new Game();
                }
                return instance;
            }
        }

        private Game() {
            
        }

        public void loadLounge() {
            SceneManager.LoadScene("Lounge");
        }

        public void loadLevel() {
            SceneManager.LoadScene("Level");
        }

        public void loadResults() {
            SceneManager.LoadScene("EndResult");
        }

    }
}
