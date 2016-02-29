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

        private float time = 10f;

        // Use this for initialization
        void Start() {
            level = new Level();
            zoneObject = Instantiate(zonePrefab);
            zoneObject.transform.SetParent(transform);
            level.createPlayers();
            zoneObject.GetComponent<ZoneController>().spawnPlayers(level.hero, level.guards);
            zoneObject.GetComponent<ZoneController>().logInfoAboutPlayers();
        }

        // Update is called once per frame
        void Update() {
            if (level.isPlaying) {
                // Timer ticking
                if (time > 0) {
                    HUD.GetComponent<HUDController>().updateTimer(time);
                    time -= Time.deltaTime;
                }
                // Time is over, change roles
                else {
                    time = 10f;
                    zoneObject.GetComponent<ZoneController>().destroyPlayers();
                    if (level.changeRoles()) {
                        zoneObject.GetComponent<ZoneController>().spawnPlayers(level.hero, level.guards);
                        zoneObject.GetComponent<ZoneController>().logInfoAboutPlayers();
                    }
                }
            }
        }

        /*public void createPlayers() {
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
        }*/
    }
}
