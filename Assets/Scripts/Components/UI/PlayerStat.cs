using UnityEngine;
using UnityEngine.UI;
using System.Collections;
using SupHero.Components.Character;
using SupHero.Model;

namespace SupHero.Components.UI {
    public class PlayerStat : MonoBehaviour {

        public Text playerName;
        public Text score;
        public Image pose;
        public Image place;

        public Sprite place1;
        public Sprite place2;
        public Sprite place3;
        public Sprite place4;

        void Start() {

        }
        
        public void createWithPlayer(Player player, int place) {
            playerName.text = player.playerName;
            score.text = player.points.ToString();
            pose.sprite = player.character.pose;
            switch (place) {
                case 1:
                    this.place.sprite = place1;
                    break;
                case 2:
                    this.place.sprite = place2;
                    break;
                case 3:
                    this.place.sprite = place3;
                    break;
                case 4:
                    this.place.sprite = place4;
                    break;
                default:
                    this.place.gameObject.SetActive(false);
                    break;
            }
        }

        void Update() {
            if (Input.anyKeyDown) {
                Game.Instance.loadMainMenu();
            }
        }
    }
}
