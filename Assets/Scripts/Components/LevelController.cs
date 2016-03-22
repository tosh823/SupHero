using UnityEngine;
using UnityEngine.UI;
using System.Collections;
using System.Collections.Generic;
using SupHero.Model;

namespace SupHero.Components {
    public class LevelController : MonoBehaviour {

        public static LevelController instance = null;

        public Level level { get; private set; }
        public HUDController HUD { get; private set; }
        public CameraController view { get; private set; }
        public Data data;
        public GameObject zonePrefab;

        private GameObject zoneObject;

        private Timer timer;

        // Singleton realization
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

        // Update is called once per frame
        void Update() {
            if (level.isPlaying) {
                // Timer ticking
                /*if (time > 0) {
                    HUD.GetComponent<HUDController>().updateTimer(time);
                    time -= Time.deltaTime;
                }
                // Time is over, change roles
                else {
                    time = Constants.turnTime;
                    zoneObject.GetComponent<ZoneController>().destroyPlayers();
                    HUD.clearPlayerUIs();
                    if (level.changeRoles()) {
                        zoneObject.GetComponent<ZoneController>().spawnPlayers(level.players);
                    }
                }*/
            }
        }

        public void updateTimer() {
            HUD.updateTimer(timer.time);
        }

        public void newTurn() {
            // Destroy timer
            timer.OnTick -= updateTimer;
            timer.OnEnd -= newTurn;
            Destroy(timer);

            if (level.changeRoles()) {
                Debug.Log("Change roles!");
                transferToZone();
                timer = gameObject.AddComponent<Timer>();
                timer.time = data.mainSettings.turnTime;
                timer.OnTick += updateTimer;
                timer.OnEnd += newTurn;
                timer.launch();
            }
            else {
                // Go to reward section
                //timer.OnTick -= updateTimer;
                //timer.OnEnd -= newTurn;
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
