using UnityEngine;
using System.Collections;

namespace SupHero {
    public class ManagersLoader : MonoBehaviour {

        public GameObject gameManager;

        // Initialization
        void Awake() {
            // If not exist, create Game Manager
            if (GameManager.instance == null) {
                Instantiate(gameManager);
            }
        }
    }
}
