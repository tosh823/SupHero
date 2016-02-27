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
            setupPlayers();
            zoneObject.GetComponent<ZoneController>().spawnPlayers(level.players);
        }

        // Update is called once per frame
        void Update() {
            if (time > 0) {
                HUD.GetComponent<HUDController>().updateTimer(time);
                time -= Time.deltaTime;
            }
        }

        public void setupPlayers() {
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
    }
}
