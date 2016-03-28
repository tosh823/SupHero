using UnityEngine;
using UnityEngine.UI;
using System.Collections;
using System.Collections.Generic;
using SupHero.Model;

namespace SupHero.Components {
    public class LevelController : MonoBehaviour {

        public static LevelController Instance = null;

        public Level level { get; private set; }
        public HUDController HUD { get; private set; }
        public CameraController view { get; private set; }
        public Data data;
        public GameObject zonePrefab;

        private GameObject zoneObject;

        private Timer timer;

        // Singleton realization
        void Awake() {
            if (Instance == null) {
                Instance = this;
            }
            else if (Instance != this) {
                Destroy(gameObject);
            }
            DontDestroyOnLoad(gameObject);
        }

        void Start() {
            level = new Level();
            level.createPlayers();

            HUD = GameObject.FindGameObjectWithTag(Tags.MainUI).GetComponent<HUDController>();
            view = Camera.main.GetComponent<CameraController>();
            data = GetComponent<Data>();

            createZone();

            timer = gameObject.AddComponent<Timer>();
            timer.time = data.mainSettings.turnTime;
            timer.OnTick += updateTimer;
            timer.OnEnd += newTurn;
            timer.launch();
        }

        void Update() {
            if (level.isPlaying) {
                
            }
        }

        public void updateTimer() {
            HUD.updateTimer(timer.time);
        }

        public void newTurn() {
            if (level.changeRoles()) {
                transferToZone();
                timer = gameObject.AddComponent<Timer>();
                timer.time = data.mainSettings.turnTime;
                timer.OnTick += updateTimer;
                timer.OnEnd += newTurn;
                timer.launch();
                HUD.showMessage("New Turn!", 1f);
            }
            else {
                // Go to reward section
            }
        }

        public void transferToZone() {
            // Destroying current zone
            zoneObject.GetComponent<ZoneController>().destroyPlayers();
            HUD.clearPlayerUIs();
            Destroy(zoneObject.gameObject);
            // Creating new one
            createZone();
        }

        private void createZone() {
            zoneObject = Instantiate(zonePrefab);
            zoneObject.transform.SetParent(transform);
        }
    }
}
