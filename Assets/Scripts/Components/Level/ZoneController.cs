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
                GameObject pawn = spawner.spawnPlayer(player);
                PlayerController pc = pawn.GetComponent<PlayerController>();

                // Then, give it soul
                pc.setPlayer(player);
                pc.OnDie += spawnPlayer;

                players.Add(pawn);
                player.born();
            }
            spawner.resetSpawnPoints();
        }
        
        // Using for respawning players after death
        public void spawnPlayer(Player player) {
            Debug.Log(player + " died, respawning");
            // Depending of whom to spawn, choose
            if (player is Guard) {
                respawnGuard(player);
            }
            else {
                respawnHero(player);
            }
        }

        public void respawnHero(Player player) {
            // Remove gameObject from zome
            GameObject existing = findPlayer(player);
            if (existing != null) {
                // If player still exist, remove
                removePlayer(existing);
                players.Remove(existing);
            }
            // Immediate spawn in the beginning of the level
            // with teleporting guards there too
            GameObject pawn = spawner.spawnPlayer(player);
            PlayerController pc = pawn.GetComponent<PlayerController>();
            pc.setPlayer(player);
            pc.OnDie += spawnPlayer;

            players.Add(pawn);
            player.born();
            massTeleportToStart();
        }

        public void respawnGuard(Player player) {
            // Remove gameObject from zome
            GameObject existing = findPlayer(player);
            if (existing != null) {
                // If player still exist, remove
                removePlayer(existing);
                players.Remove(existing);
            }
            // Create a SpawnPoint with duration
            SpawnPoint spawn = spawner.createSpawnPoint();
            spawn.pilot = player;
            Timer spawnLifetime = spawn.gameObject.AddComponent<Timer>();
            spawnLifetime.time = Data.Instance.mainSettings.guard.spawnDuration;
            spawnLifetime.OnEnd += delegate () {
                GameObject pawn = spawner.spawnPlayer(player, spawn.transform.position);
                PlayerController pc = pawn.GetComponent<PlayerController>();
                pc.setPlayer(player);
                pc.OnDie += spawnPlayer;

                players.Add(pawn);
                player.born();
                spawn.Spawn();
            };
            spawnLifetime.Launch();
        }

        // Just pure function to spawn a generic player anywhere
        public void spawnPlayerInPosition(Player player, Vector3 place) {
            GameObject pawn = spawner.spawnPlayer(player, place);
            PlayerController pc = pawn.GetComponent<PlayerController>();
            pc.setPlayer(player);
            pc.OnDie += spawnPlayer;

            players.Add(pawn);
            player.born();
        }

        public void massTeleportToStart() {
            foreach (GameObject player in players) {
                Vector3 origin = player.GetComponent<PlayerController>().originPosition;
                player.transform.position = origin;
            }
        }

        public void teleportObjectToHero(GameObject obj) {
            GameObject hero = getHero();
            if (hero != null) {
                Vector3 place = spawner.getRandomPositionNear(hero.transform.position, Data.Instance.mainSettings.hero.spawnDistance * 2f);
                obj.transform.position = place;
            }
        }

        // Kill all the players
        public void killPlayers() {
            foreach (GameObject player in players) {
                removePlayer(player);
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
        }
    }
}
