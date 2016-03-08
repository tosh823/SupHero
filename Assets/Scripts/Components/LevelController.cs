using UnityEngine;
using UnityEngine.UI;
using System.Collections;
using System.Collections.Generic;
using SupHero.Model;

namespace SupHero.Components {
    public class LevelController : MonoBehaviour {

        public static LevelController instance = null;

        public HUDController HUD;
        public CameraController view;
        public GameObject zonePrefab;

        private GameObject zoneObject;
        private Level level;

        private float time = Constants.turnTime;

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
            level = new Level();
            HUD = GameObject.FindGameObjectWithTag("MainUI").GetComponent<HUDController>();
            view = Camera.main.GetComponent<CameraController>();
            zoneObject = Instantiate(zonePrefab);
            zoneObject.transform.SetParent(transform);
            level.createPlayers();
            zoneObject.GetComponent<ZoneController>().spawnPlayers(level.players);
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
                    HUD.GetComponent<HUDController>().clearPlayerUIs();
                    if (level.changeRoles()) {
                        zoneObject.GetComponent<ZoneController>().spawnPlayers(level.players);
                    }
                }
            }
        }
    }
}
