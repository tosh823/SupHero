using UnityEngine;
using System.Collections;
using System.Collections.Generic;

using SupHero.Model;

namespace SupHero.Controllers {
    public class ZoneController : MonoBehaviour {

        public GameObject playerPrefab;
        //public GameObject[] players;
        public List<GameObject> players;

        // Use this for initialization
        void Start() {
            //players;
        }

        // Update is called once per frame
        void Update() {

        }

        public void spawnPlayers(Hero hero, List<Guard> guards) {
            GameObject pawn = Instantiate(playerPrefab) as GameObject;
            pawn.transform.SetParent(transform);
            pawn.GetComponent<PlayerController>().setPlayer(hero);
            players.Add(pawn);

            foreach (Guard guard in guards) {
                pawn = Instantiate(playerPrefab) as GameObject;
                float chaosX = Random.value * 10;
                float chaosZ = Random.value * 10;
                pawn.transform.SetParent(transform);
                pawn.transform.Translate(chaosX, 0f, chaosZ);
                pawn.GetComponent<PlayerController>().setPlayer(guard);
                players.Add(pawn);
            }
        }

        public void destroyPlayers() {
            foreach (GameObject player in players) {
                player.GetComponent<PlayerController>().killSelf();
            }
            players.Clear();
        }

        public void logInfoAboutPlayers() {
            foreach (GameObject player in players) {
                Player objectPlayer = player.GetComponent<PlayerController>().player;
                Debug.Log(objectPlayer.GetType().ToString() + " " + objectPlayer.playerName + " with number " + objectPlayer.number);
            }
        }
    }
}
