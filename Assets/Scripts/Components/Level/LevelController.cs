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
            if (Instance == null) {
                Instance = this;
            }
            else if (Instance != this) {
                Destroy(gameObject);
            }
        }


        void Start() {
            startLevel();
        }

        public void startLevel() {
            level = new Model.Level();
            level.createPlayers();

            HUD = GameObject.FindWithTag(Tags.MainUI).GetComponent<HUDController>();
            HUD.createTimer();
            view = Camera.main.GetComponent<CameraController>();

            createZone();

            turn = 1;

            timer = gameObject.AddComponent<Timer>();
            timer.time = Data.Instance.mainSettings.turnTime;
            timer.OnTick += updateTimer;
            timer.OnEnd += newTurn;
            timer.Launch();
        }

        void Update() {
            
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
                timer.Launch();
                HUD.showMessage("New Turn!", 1f);
            }
            else {
                // Go to reward section
                // DEBUG display results info
                zone.destroyPlayers();
                HUD.clearPlayerUIs();
                Destroy(zone.gameObject);
                // Saving data
                Data.Instance.session.players = level.players;
                Game.Instance.loadResults();
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

        public Dictionary<string, int> getStatistics() {
            Dictionary<string, int> stats = new Dictionary<string, int>();
            foreach (Player player in level.players) {
                stats.Add(player.playerName, player.points);
            }
            return stats;
        }
    }
}
