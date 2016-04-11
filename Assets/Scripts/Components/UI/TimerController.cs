using UnityEngine;
using UnityEngine.UI;
using System.Collections;

namespace SupHero.Components.UI {
    public class TimerController : MonoBehaviour {

        public Text timerText;

        void Start() {
            timerText.text = "Test";
        }

        public void updateTimer(string time) {
            timerText.text = time;
        }

        void Update() {

        }
    }
}
