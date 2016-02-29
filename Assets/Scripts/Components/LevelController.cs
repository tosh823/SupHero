using UnityEngine;
using UnityEngine.UI;
using System.Collections;
using System.Collections.Generic;
using SupHero.Model;

namespace SupHero.Controllers {
    public class LevelController : MonoBehaviour {

        public GameObject zonePrefab;
        public GameObject HUD;

        private GameObject zoneObject;
        private Level level;

        private float time = 120f;

        // Use this for initialization
        void Start() {
            level = new Level();
            zoneObject = Instantiate(zonePrefab);
            zoneObject.transform.SetParent(transform);
            createPlayers(4);
            level.setupRoles();
            zoneObject.GetComponent<ZoneController>().spawnPlayers(level.players);
            displayInfo();
        }

        // Update is called once per frame
        void Update() {
            // Timer ticking
            if (time > 0) {
                HUD.GetComponent<HUDController>().updateTimer(time);
                time -= Time.deltaTime;
            }
            // Time is over, change roles
            else {
                
            }
        }

        public void createPlayers(int number) {
            for (int index = 1; index <= number; index++) {
                Player player = new Player(index);
                player.inputType = InputType.GAMEPAD;
                level.addPlayer(player);
            }
        }

        public void createPlayers() {
            string[] joysticks = Input.GetJoystickNames();
            if (joysticks.Length > 0) {
                for (int index = 0; index < joysticks.Length; index++) {
                    Debug.Log(index + " = " + joysticks[index]);
                    Player player = new Player(index);
                    player.inputType = InputType.GAMEPAD;
                    level.addPlayer(player);
                }
            }
            else {
                Player player = new Player(0);
                player.inputType = InputType.KEYBOARD;
                level.addPlayer(player);
            }
        }

        public void displayInfo() {
            foreach (Player player in level.players) {
                Debug.Log(player.GetType().ToString() + " with number " + player.number + " on the battlefield");
            }
        }
    }
}
