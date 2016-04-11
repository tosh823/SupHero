using UnityEngine;
using UnityEngine.UI;

namespace SupHero.Components.UI {
    public class PopUpMessage : MonoBehaviour {

        private Text textView;
        private bool shown;
        private float speed;
        
        void Awake() {
            textView = GetComponent<Text>();
            shown = false;
        }

        void Update() {
            if (shown) {
                float current = textView.GetComponent<CanvasRenderer>().GetAlpha();
                if (current > 0f) {
                    textView.GetComponent<CanvasRenderer>().SetAlpha(current - speed * Time.deltaTime);
                }
                else {
                    shown = false;
                    Destroy(gameObject);
                }
            }
        }

        public void showWithMessage(string message, float time = 2f) {
            textView.text = message;
            speed = 1 / time;
            shown = true;
        }
    }
}
