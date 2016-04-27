﻿using UnityEngine;
using UnityEngine.UI;
using System.Collections;

using SupHero;
using SupHero.Model;

namespace SupHero.Components.UI {
    public class PlayerUIController : MonoBehaviour {

        public Player player { get; protected set; }

        public GameObject shieldPanel;
        public Text shieldText;
        public Text armorText;
        public Text healthText;
        public Text playerName;
        public Text playerPoints;
        public Text ammo;

        void Start() {

        }

        void Update() {
            if (player != null) {
                if (player is Hero) {
                    Hero hero = player as Hero;
                    shieldText.text = string.Format("{0}%", ((int)hero.shieldPercentage).ToString());
                }
                else {
                    shieldPanel.SetActive(false);
                }
                armorText.text = player.armor.ToString();
                
                playerName.text = player.playerName;
                playerPoints.text = player.points.ToString();
            }
        }

        public void setPlayer(Player player) {
            this.player = player;
        }
    }
}
