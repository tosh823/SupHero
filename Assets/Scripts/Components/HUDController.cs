using UnityEngine;
using UnityEngine.UI;
using System.Collections;

namespace SupHero.Controllers {
    public class HUDController : MonoBehaviour {

        public GameObject timerPanel;
        private GameObject timerInstance;

        // Use this for initialization
        void Start() {
            timerInstance = Instantiate(timerPanel);
            timerInstance.transform.SetParent(transform, false);
        }

        // Update is called once per frame
        void Update() {
            
        }

        public void updateTimer(float time) {
            timerInstance.GetComponent<TimerController>().updateTimer(getTime(time));
        }

        private string getTime(float time) {
            int rounded = Mathf.FloorToInt(time);
            int minutes = rounded / 60;
            int seconds = rounded % 60;
            return string.Format("{0}:{1:D2}", minutes, seconds);
        }
    }
}
