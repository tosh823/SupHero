﻿using UnityEngine;
using System.Collections;
using SupHero;
using SupHero.Model;

namespace SupHero.Components.Level {
    public class Spawner : MonoBehaviour {

        public GameObject spawnPrefab;
        public Transform heroSpawnPoint;
        public Transform[] guardsSpawnPoints;

        private ZoneController zoneController; // Ref to GameObject ZoneController

        void Awake() {
            zoneController = GetComponent<ZoneController>();
        }

        void Start() {
            
        }

        void Update() {

        }

        // Spawn a player
        public GameObject spawnPlayer(Player player, bool isInitial = false) {
            // First, search for player
            GameObject existing = zoneController.findPlayer(player);
            if (existing != null) {
                // If player already existed, remove
                zoneController.removePlayer(existing);
            }
            // Creating new player object
            GameObject pawn = createPlayerGameObject(player);
            Vector3 spawnPosition = Vector3.zero;
            if (player is Hero) {
                // Hero always spawns in the beginning
                spawnPosition = heroSpawnPoint.position;
            }
            else {
                if (isInitial) spawnPosition = getInitialSpawnPosition();
                else spawnPosition = getSpawnPosition();
            }
            spawnPosition.y = Data.Instance.mainSettings.hero.prefab.transform.position.y;
            pawn.transform.position = spawnPosition;

            return pawn;  
        }

        private GameObject createPlayerGameObject(Player player) {
            GameObject prefab;
            if (player is Hero) prefab = Data.Instance.mainSettings.hero.prefab;
            else prefab = Data.Instance.mainSettings.guard.prefab;
            GameObject pawn = Instantiate(prefab) as GameObject;
            pawn.transform.SetParent(transform);
            return pawn;
        }

        public Vector3 getPosInsideBounds() {
            Vector3 pos = Vector3.zero;
            Area visibleArea = Camera.main.GetComponent<CameraController>().getVisibleArea();
            Bounds surfaceBounds = zoneController.constructor.battleField.GetComponent<Collider>().bounds;
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
            Vector3 position = zoneController.getHero().transform.position;
            Vector3 rng = Random.onUnitSphere * Data.Instance.mainSettings.hero.spawnDistance;
            Bounds surfaceBounds = zoneController.constructor.battleField.GetComponent<Collider>().bounds;
            // Checking availability of point
            bool beyoundMaxX = ((position.x + rng.x) >= surfaceBounds.max.x);
            bool belowMinX = ((position.x + rng.x) <= surfaceBounds.min.x);
            if (beyoundMaxX || belowMinX) {
                position.x -= rng.x;
            }
            else position.x += rng.x;

            bool beyoundMaxZ = ((position.z + rng.z) >= surfaceBounds.max.z);
            bool belowMinZ = ((position.z + rng.z) <= surfaceBounds.min.z);
            if (beyoundMaxZ || belowMinZ) {
                position.z -= rng.z;
            }
            else position.z += rng.z;

            return position;
        }

        private Vector3 getInitialSpawnPosition() {
            Vector3 position = Vector3.zero;
            for (int index = 0; index < guardsSpawnPoints.Length; index++) {
                if (guardsSpawnPoints[index].gameObject.activeInHierarchy) {
                    guardsSpawnPoints[index].gameObject.SetActive(false);
                    return guardsSpawnPoints[index].position;
                }
            }
            return position;
        }

        public void resetSpawnPoints() {
            for (int index = 0; index < guardsSpawnPoints.Length; index++) {
                guardsSpawnPoints[index].gameObject.SetActive(true);
            }
        }
    }
}
