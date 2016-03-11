using UnityEngine;
using System.Collections;
using SupHero;
using SupHero.Model;

namespace SupHero.Components {
    public class Spawner : MonoBehaviour {

        // Prefabs for instantiating
        public GameObject playerPrefab;

        private ZoneController zoneController; // Ref to GameObject ZoneController
        private Bounds surfaceBounds; // The dimensions of available surface

        void Awake() {
            zoneController = GetComponent<ZoneController>();
            surfaceBounds = GameObject.FindGameObjectWithTag("Surface").GetComponent<Collider>().bounds;
        }

        // Use this for initialization
        void Start() {
            
        }

        // Update is called once per frame
        void Update() {

        }

        // Spawn a player
        public GameObject spawnPlayer(Player player) {
            // First, search for player
            GameObject existing = zoneController.findPlayer(player);
            if (existing != null) {
                // If player already existed, remove
                zoneController.removePlayer(existing);
            }
            // Creating new player object
            GameObject pawn = Instantiate(playerPrefab) as GameObject;
            //float chaosX = Random.value * 10;
            //float chaosZ = Random.value * 10;
            Vector3 move = getPosInsideBounds();
            pawn.transform.SetParent(transform);
            //pawn.transform.Translate(chaosX, 0f, chaosZ);
            pawn.transform.Translate(move);
            GameObject ui = LevelController.instance.HUD.findUIforPlayer(player);
            PlayerController pc = pawn.GetComponent<PlayerController>();
            pc.setPlayer(player);
            pc.setUI(ui);

            return pawn;  
        }

        public Vector3 getPosInsideBounds() {
            Vector3 pos = Vector3.zero;
            Area visibleArea = LevelController.instance.view.getVisibleArea();
            float visibleXmin = surfaceBounds.min.x;
            float visibleXmax = surfaceBounds.max.x;
            float visibleZmin = surfaceBounds.min.z;
            float visibleZmax = surfaceBounds.max.z;

            // Checking top left corner
            Vector3 topLeft = visibleArea.topLeft;
            if (surfaceBounds.Contains(topLeft)) {
                visibleXmin = topLeft.x;
                visibleZmax = topLeft.z;
            }

            // Checking bot left corner
            Vector3 botLeft = visibleArea.botLeft;
            if (surfaceBounds.Contains(botLeft)) {
                visibleXmin = botLeft.x;
                visibleZmin = botLeft.z;
            }

            // Checking top right corner
            Vector3 topRight = visibleArea.topRight;
            if (surfaceBounds.Contains(topRight)) {
                visibleXmax = topRight.x;
                visibleZmax = topRight.z;
            }

            // Checking bot right corner
            Vector3 botRight = visibleArea.botRight;
            if (surfaceBounds.Contains(botRight)) {
                visibleXmax = botRight.x;
                visibleZmin = botRight.z;
            }

            float x = Random.Range(visibleXmin, visibleXmax);
            float z = Random.Range(visibleZmin, visibleZmax);
            pos.x = x;
            pos.y = 0f;
            pos.z = z;
            return pos;
        }

        private Vector3 getSpawnPosition() {
            Vector3 pos = Vector3.zero;
            pos.y = 1f;
            return pos;
        }

        private void createMark(Vector3 pos) {
            GameObject mark = Instantiate(LevelController.instance.view.mark, pos, Quaternion.identity) as GameObject;
            mark.transform.SetParent(transform);
        }
    }
}
