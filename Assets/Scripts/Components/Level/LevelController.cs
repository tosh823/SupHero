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
            if (Data.Instance.session.players == null) level.createPlayers();
            else level.players = Data.Instance.session.players;

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
            // Destroying current zone
            zone.killPlayers();
            HUD.clearPlayerUIs();
            Destroy(zone.gameObject);
            if (level.changeRoles()) {
                turn++;
                createZone();
                timer = gameObject.AddComponent<Timer>();
                timer.time = Data.Instance.mainSettings.turnTime;
                timer.OnTick += updateTimer;
                timer.OnEnd += newTurn;
                timer.Launch();
                HUD.showMessage("New Turn!", 1f);
            }
            else {
                // Saving data
                Data.Instance.session.players = level.players;
                Game.Instance.loadResults();
            }
        }

        public void generateZone() {
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
