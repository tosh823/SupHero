using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using SupHero.Model;
using SupHero.Components.Character;

namespace SupHero.Components.Level {
    public class ZoneController : MonoBehaviour {
        
        // Store
        public List<GameObject> players;

        public Spawner spawner;
        public Map constructor;

        void Awake() {
            
        }

        void Start() {
            constructor = GetComponent<Map>();
            spawner = GetComponent<Spawner>();
            if (LevelController.Instance != null) {
                constructor.constructZone(5);
                spawnAll(LevelController.Instance.level.players);
            }
        }

        void Update() {
            
        }

        // Initial respawn on zone
        public void spawnAll(List<Player> toSpawn) {
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
                player.resurrect();
            }
            spawner.resetSpawnPoints();
        }
        
        // Using for respawning players after death
        public void spawnPlayer(Player player) {
            if (player is Hero) {
                destroyPlayers();
                spawnAll(LevelController.Instance.level.players);
            }
            else {
                GameObject pawn = spawner.spawnPlayer(player);
                PlayerController pc = pawn.GetComponent<PlayerController>();

                pc.setPlayer(player);
                pc.OnDie += spawnPlayer;

                players.Add(pawn);
                player.resurrect();
            }
        }

        // Kill all the players
        public void destroyPlayers() {
            foreach (GameObject player in players) {
                player.GetComponent<PlayerController>().OnDie -= spawnPlayer;
                Destroy(player.gameObject);
            }
            players.Clear();
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
