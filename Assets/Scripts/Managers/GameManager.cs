using UnityEngine;
using System;
using System.Collections;
using System.Collections.Generic;

namespace SupHero {
    public class GameManager : MonoBehaviour {

        public static GameManager instance = null;
        public static List<Player> players;

        // Called before Start
        void Awake() {
            if (instance == null) {
                instance = this;
            }
            else if (instance != this) {
                Destroy(gameObject);
            }
            DontDestroyOnLoad(gameObject);
        }

        // Use this for initialization
        void Start() {
            players = new List<Player>();
        }

        // Update is called once per frame
        void Update() {

        }

        public void setupPlayers() {
            string[] joysticks = Input.GetJoystickNames();
            if (joysticks.Length > 0) {
                for (int index = 0; index < joysticks.Length; index++) {
                    Debug.Log(index + " = " + joysticks[index]);
                    Player player = new Player(index);
                    player.inputType = InputType.GAMEPAD;
                    players.Add(player);
                }
            }
            else {
                Player player = new Player(0);
                player.inputType = InputType.KEYBOARD;
                players.Add(player);
            }
        }
    }
}
