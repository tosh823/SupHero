using UnityEngine;
using UnityEngine.UI;
using System.Collections;
using System.Collections.Generic;
using SupHero.Model;
using SupHero.Components.UI;

namespace SupHero.Components.Level {
    public class LevelController : MonoBehaviour {

        public static LevelController Instance = null;

        public Model.Level level { get; private set; }
        public HUDController HUD { get; private set; }
        public CameraController view { get; private set; }
        public GameObject zonePrefab;
        public int turn { get; private set; }

        private ZoneController zone;

        private Timer timer;

        // Singleton realization
        void Awake() {
            Debug.Log("Awake of the new game");
            if (Instance == null) {
                Instance = this;
            }
            else if (Instance != this) {
                Destroy(gameObject);
            }
            DontDestroyOnLoad(gameObject);
        }


        void Start() {
            Debug.Log("Alright, starting a new game");
            level = new Model.Level();
            level.createPlayers();

            HUD = GameObject.FindGameObjectWithTag(Tags.MainUI).GetComponent<HUDController>();
            HUD.createTimer();
            view = Camera.main.GetComponent<CameraController>();

            createZone();
            turn = 1;

            timer = gameObject.AddComponent<Timer>();
            timer.time = Data.Instance.mainSettings.turnTime;
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
                turn++;
                transferToZone();
                timer = gameObject.AddComponent<Timer>();
                timer.time = Data.Instance.mainSettings.turnTime;
                timer.OnTick += updateTimer;
                timer.OnEnd += newTurn;
                timer.launch();
                HUD.showMessage("New Turn!", 1f);
            }
            else {
                // Go to reward section
                // DEBUG display results info
                foreach (Player player in level.players) {
                    Debug.Log("Player " + player.number + " : " + player.points);
                }
            }
        }

        public void transferToZone() {
            // Destroying current zone
            zone.destroyPlayers();
            HUD.clearPlayerUIs();
            Destroy(zone.gameObject);
            // Creating new one
            createZone();
        }

        private void createZone() {
            GameObject zoneInstance = Instantiate(zonePrefab) as GameObject;
            zoneInstance.transform.SetParent(transform);
            zone = zoneInstance.GetComponent<ZoneController>();
        }
    }
}
