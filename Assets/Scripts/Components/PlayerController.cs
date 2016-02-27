using UnityEngine;
using System.Collections;
using SupHero;
using SupHero.Model;

namespace SupHero.Controllers {
    public class PlayerController : MonoBehaviour {

        private Player player;

        // Use this for initialization
        void Start() {
            
        }

        // Update is called once per frame
        void Update() {

        }

        public void setPlayer(Player player) {
            this.player = player;
        }

        public Vector2 getMovementVector() {
            Vector2 movement = new Vector2(0f, 0f);
            float h, v;
            switch (player.inputType) {
                case InputType.KEYBOARD:
                    h = Input.GetAxis("Horizontal");
                    v = Input.GetAxis("Vertical");
                    movement = new Vector2(h, v);
                    break;
                case InputType.GAMEPAD:
                    h = Input.GetAxis(Utils.getControlForPlayer("LeftStickX", player.number));
                    v = Input.GetAxis(Utils.getControlForPlayer("LeftStickY", player.number));
                    movement = new Vector2(h, v);
                    break;
                default:
                    break;
            }
            return movement;
        }
    }
}
