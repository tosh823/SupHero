using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using SupHero.Model;
using SupHero.Components.Character;

namespace SupHero.Components.Level {
    public class ZoneController : MonoBehaviour {
        
        // Store
        public List<GameObject> players;

        private Spawner spawner;
        private Map constructor;

        void Awake() {
            
        }

        void Start() {
            spawner = GetComponent<Spawner>();
            constructor = GetComponent<Map>();

            if (LevelController.Instance != null) {
                constructor.constructZone(5);
                spawnPlayers(LevelController.Instance.level.players);
            }
        }

        void Update() {
            
        }

        // Initial respawn on zone
        public void spawnPlayers(List<Player> toSpawn) {
            players = new List<GameObject>();
            foreach (Player player in toSpawn) {
                // First, give it body
                // Creating new player gameObject
                GameObject pawn = spawner.spawnPlayer(player, true);
                PlayerController pc = pawn.GetComponent<PlayerController>();

                // Then, give it soul
                pc.setPlayer(player);
                pc.OnDie += spawnPlayer;

                players.Add(pawn);
            }
            spawner.resetSpawnPoints();
        }
        
        // Using for respawning players after death
        public void spawnPlayer(Player player) {
            GameObject pawn = spawner.spawnPlayer(player);
            PlayerController pc = pawn.GetComponent<PlayerController>();

            pc.setPlayer(player);
            pc.OnDie += spawnPlayer;

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

        public GameObject getHero() {
            foreach (GameObject playerInScene in players) {
                if (playerInScene.GetComponent<PlayerController>().player is Hero) {
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
