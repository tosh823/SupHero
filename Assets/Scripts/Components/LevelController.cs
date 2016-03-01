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

        private float time = Constants.turnTime;

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
                    time = Constants.turnTime;
                    zoneObject.GetComponent<ZoneController>().destroyPlayers();
                    if (level.changeRoles()) {
                        zoneObject.GetComponent<ZoneController>().spawnPlayers(level.hero, level.guards);
                        zoneObject.GetComponent<ZoneController>().logInfoAboutPlayers();
                    }
                }
            }
        }
    }
}
