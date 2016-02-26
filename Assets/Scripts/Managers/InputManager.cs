using UnityEngine;
using System.Collections;

namespace SupHero {

    public enum InputType {
        KEYBOARD,
        GAMEPAD
    }

    public static class InputManager {

        static InputManager() {

        }

        public static Vector2 getMovementVector(Player player) {
            Vector2 movement = new Vector2(0f, 0f);
            float h, v;
            switch (player.inputType) {
                case InputType.KEYBOARD:
                    h = Input.GetAxis("Horizontal");
                    v = Input.GetAxis("Vertical");
                    movement = new Vector2(h, v);
                    break;
                case InputType.GAMEPAD:
                    h = Input.GetAxis(getControlForPlayer("LeftStickX", player.number));
                    v = Input.GetAxis(getControlForPlayer("LeftStickY", player.number));
                    movement = new Vector2(h, v);
                    break;
                default:
                    break;
            }
            return movement;
        }

        private static string getControlForPlayer(string control, int number) {
            return string.Format("{0}_P{1}", control, number);
        }
    }
}
