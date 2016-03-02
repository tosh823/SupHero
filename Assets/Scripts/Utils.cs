using UnityEngine;
using System;
using System.Collections;


namespace SupHero {

    public enum InputType {
        KEYBOARD,
        GAMEPAD,
        NONE
    }

    public static class Utils {

        public static string getControlForPlayer(string control, int number) {
            return string.Format("{0}_{1}", control, number);
        }

        public static string getActionName(Action action) {
            return action.Method.Name;
        }

    }
}
