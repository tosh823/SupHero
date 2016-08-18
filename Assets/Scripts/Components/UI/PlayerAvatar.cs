using UnityEngine;
using UnityEngine.UI;
using System.Collections;

namespace SupHero.Components.UI {
    public class PlayerAvatar : MonoBehaviour {

        public Image borderImage;
        public Image shineEffect;

        public Sprite normalBorderBG;
        public Sprite normalBorderImage;

        public Sprite highlightedBorderBG;
        public Sprite highlightedBorderImage;

        void Start() {
            
        }

        public void createAvatar(bool hero = false) {
            if (hero) {
                shineEffect.gameObject.SetActive(true);
                borderImage.sprite = highlightedBorderImage;
                GetComponent<Image>().sprite = highlightedBorderBG;
            }
            else {
                shineEffect.gameObject.SetActive(false);
                borderImage.sprite = normalBorderImage;
                GetComponent<Image>().sprite = normalBorderBG;
            }
        }
    }
}
