using UnityEngine;
using System.Collections;
using SupHero;
using SupHero.Model;

namespace SupHero.Components {
    public class Spawner : MonoBehaviour {

        // Prefabs for instantiating
        public GameObject playerPrefab;

        private ZoneController zoneController; // Ref to GameObject ZoneController

        void Awake() {
            zoneController = GetComponent<ZoneController>();
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
                // And respawn
                GameObject pawn = Instantiate(playerPrefab) as GameObject;
                float chaosX = Random.value * 10;
                float chaosZ = Random.value * 10;
                pawn.transform.SetParent(transform);
                pawn.transform.Translate(chaosX, 0f, chaosZ);
                GameObject ui = LevelController.instance.HUD.findUIforPlayer(player);
                PlayerController pc = pawn.GetComponent<PlayerController>();
                pc.setPlayer(player);
                pc.setUI(ui);
               
                return pawn;
            }
            else {
                // Creating new player object
                GameObject pawn = Instantiate(playerPrefab) as GameObject;
                float chaosX = Random.value * 10;
                float chaosZ = Random.value * 10;
                pawn.transform.SetParent(transform);
                pawn.transform.Translate(chaosX, 0f, chaosZ);
                GameObject ui = LevelController.instance.HUD.createUIforPlayer(player);
                PlayerController pc = pawn.GetComponent<PlayerController>();
                pc.setPlayer(player);
                pc.setUI(ui);

                return pawn;
            }
            
        }
    }
}
