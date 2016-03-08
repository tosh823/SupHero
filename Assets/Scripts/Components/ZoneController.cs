using UnityEngine;
using System.Collections;
using System.Collections.Generic;

using SupHero.Model;

namespace SupHero.Components {
    public class ZoneController : MonoBehaviour {
        
        // Prefabs for instantiating

        public List<GameObject> players;

        private Spawner spawner;

        void Awake() {
            spawner = GetComponent<Spawner>();
        }

        // Use this for initialization
        void Start() {
            
        }

        // Update is called once per frame
        void Update() {
            
        }

        // Initial respawn on zone
        public void spawnPlayers(List<Player> toSpawn) {
            players = new List<GameObject>();
            foreach (Player player in toSpawn) {
                // Creating new player gameObject
                GameObject pawn = spawner.spawnPlayer(player);
                pawn.GetComponent<PlayerController>().OnDie += spawnPlayer;
                players.Add(pawn);
            }
        }
        
        // Using for respawning players after death
        public void spawnPlayer(Player player) {
            GameObject pawn = spawner.spawnPlayer(player);
            pawn.GetComponent<PlayerController>().OnDie += spawnPlayer;
            players.Add(pawn);
            player.resurrect();
        }

        // Kill all the players
        public void destroyPlayers() {
            foreach (GameObject player in players) {
                player.GetComponent<PlayerController>().OnDie -= spawnPlayer;
                Destroy(player.gameObject);
            }
            players.Clear();
        }

        // Log info about players to console
        public void logInfoAboutPlayers() {
            foreach (GameObject player in players) {
                Player objectPlayer = player.GetComponent<PlayerController>().player;
                Debug.Log(objectPlayer.GetType().ToString() + " " + objectPlayer.playerName + " with number " + objectPlayer.number);
            }
        }

        // Find player gameObject
        public GameObject findPlayer(Player player) {
            foreach (GameObject playerInScene in players) {
                if (playerInScene.GetComponent<PlayerController>().player.number == player.number) {
                    return playerInScene;
                }
            }
            return null;
        }

        // Remove certaing player gameObject
        public void removePlayer(GameObject player) {
            player.GetComponent<PlayerController>().OnDie -= spawnPlayer;
            Destroy(player.gameObject);
            players.Remove(player);
        }

    }
}
