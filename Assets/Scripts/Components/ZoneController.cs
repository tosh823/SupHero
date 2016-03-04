using UnityEngine;
using System.Collections;
using System.Collections.Generic;

using SupHero.Model;

namespace SupHero.Controllers {
    public class ZoneController : MonoBehaviour {

        public GameObject playerPrefab;
        private List<GameObject> players;
        private HUDController HUD;

        void Awake() {
            HUD = GameObject.FindGameObjectWithTag("MainUI").GetComponent<HUDController>();
        }

        // Use this for initialization
        void Start() {

        }

        // Update is called once per frame
        void Update() {
            
        }

        public void spawnPlayers(List<Player> toSpawn) {
            players = new List<GameObject>();
            foreach (Player player in toSpawn) {
                // Creating new player object
                GameObject pawn = Instantiate(playerPrefab) as GameObject;
                float chaosX = Random.value * 10;
                float chaosZ = Random.value * 10;
                pawn.transform.SetParent(transform);
                pawn.transform.Translate(chaosX, 0f, chaosZ);
                GameObject ui = HUD.createUIforPlayer(player);
                PlayerController pc = pawn.GetComponent<PlayerController>();
                pc.setPlayer(player);
                pc.setUI(ui);
                pc.OnDie += spawnPlayer;
                
                players.Add(pawn);
            }
        }
        
        public void spawnPlayer(Player player) {
            // First, delete existing instance of player
            GameObject existing = findPlayer(player);
            existing.GetComponent<PlayerController>().OnDie -= spawnPlayer;
            Destroy(existing.gameObject);
            players.Remove(existing);
            // Secondly, create new
            GameObject pawn = Instantiate(playerPrefab) as GameObject;
            float chaosX = Random.value * 10;
            float chaosZ = Random.value * 10;
            pawn.transform.SetParent(transform);
            pawn.transform.Translate(chaosX, 0f, chaosZ);
            GameObject ui = HUD.findUIforPlayer(player);
            pawn.GetComponent<PlayerController>().setPlayer(player);
            pawn.GetComponent<PlayerController>().setUI(ui);
            players.Add(pawn);
            player.resurrect();
        }

        public void destroyPlayers() {
            foreach (GameObject player in players) {
                player.GetComponent<PlayerController>().OnDie -= spawnPlayer;
                Destroy(player.gameObject);
            }
            players.Clear();
        }

        public void logInfoAboutPlayers() {
            foreach (GameObject player in players) {
                Player objectPlayer = player.GetComponent<PlayerController>().player;
                Debug.Log(objectPlayer.GetType().ToString() + " " + objectPlayer.playerName + " with number " + objectPlayer.number);
            }
        }

        private GameObject findPlayer(Player player) {
            foreach (GameObject playerInScene in players) {
                if (playerInScene.GetComponent<PlayerController>().player.number == player.number) {
                    return playerInScene;
                }
            }
            return null;
        }
    }
}
