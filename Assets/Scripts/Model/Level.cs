using UnityEngine;
using System.Collections;
using System.Collections.Generic;

namespace SupHero.Model {
    public class Level {

        public List<Player> players;
        public Hero currentHero;

        public Level() {
            players = new List<Player>();
        }

        public void addPlayers(List<Player> players) {
            this.players.AddRange(players);
        }

        public void addPlayer(Player player) {
            players.Add(player);
        }
    }
}
