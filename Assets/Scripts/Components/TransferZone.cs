﻿using UnityEngine;
using System.Collections;
using SupHero.Model;

namespace SupHero.Components {
    public class TransferZone : MonoBehaviour {

        void OnTriggerEnter(Collider other) {
            GameObject entered = other.gameObject;
            if (entered.CompareTag(Tags.Player)) {
                // If hero reached the end of zone, go to new one
                if (entered.GetComponent<PlayerController>().player is Hero) {
                    LevelController.Instance.transferToZone();
                }
            }
        }
    }
}
