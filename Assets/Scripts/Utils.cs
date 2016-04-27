using UnityEngine;
using System;
using System.Collections.Generic;

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

        public static bool feelLucky(float probability) {
            float luck = UnityEngine.Random.value;
            if (luck < probability) return true;
            else return false;
        }

        public static int getRandomRange(int min, int max) {
            float minFloat = min;
            float maxFloat = max;
            float value = UnityEngine.Random.Range(minFloat, maxFloat);
            int result = Mathf.FloorToInt(value);
            return result;
        }

        public static T getRandomElement<T>(List<T> list) {
            float rand = UnityEngine.Random.Range(0f, list.Count);
            int item = Mathf.FloorToInt(rand);
            if (item == list.Count) item--;
            return list[item];
        }

        public static T getRandomElement<T>(T[] array) {
            float rand = UnityEngine.Random.Range(0f, array.Length);
            int item = Mathf.FloorToInt(rand);
            if (item == array.Length) item--;
            return array[item];
        }

    }
}
