using UnityEngine;
using UnityEngine.UI;
using System.Collections;

namespace SupHero.Components {
    public class TimerController : MonoBehaviour {

        public Text timerText;

        // Use this for initialization
        void Start() {
            timerText.text = "Test";
        }

        public void updateTimer(string time) {
            timerText.text = time;
        }

        // Update is called once per frame
        void Update() {

        }
    }
}
