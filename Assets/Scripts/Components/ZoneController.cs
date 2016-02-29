using UnityEngine;
using System.Collections;
using System.Collections.Generic;

using SupHero.Model;

namespace SupHero.Controllers {
    public class ZoneController : MonoBehaviour {

        public GameObject playerPrefab;

        // Use this for initialization
        void Start() {

        }

        // Update is called once per frame
        void Update() {

        }

        public void spawnPlayers(List<Player> players) {
            foreach (Player player in players) {
                GameObject pawn = Instantiate(playerPrefab);
                float chaosX = Random.value * 10;
                float chaosZ = Random.value * 10;
                pawn.transform.SetParent(transform);
                pawn.transform.Translate(chaosX, 0f, chaosZ);
                pawn.GetComponent<PlayerController>().setPlayer(player);
            }
        }
    }
}
