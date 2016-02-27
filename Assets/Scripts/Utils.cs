using UnityEngine;
using System.Collections;

namespace SupHero {

    public enum InputType {
        KEYBOARD,
        GAMEPAD
    }

    public static class Utils {

        public static string getControlForPlayer(string control, int number) {
            return string.Format("{0}_P{1}", control, number);
        }

    }
}
