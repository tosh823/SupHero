using UnityEngine;
using System.Collections;
using System.Collections.Generic;

namespace SupHero.Model {
    public class Level {

        private List<Player> players;
        private int heroIndex;

        public Hero hero;
        public List<Guard> guards;
        public bool isPlaying;

        public Level() {
            players = new List<Player>();
            hero = new Hero();
            guards = new List<Guard>();
            heroIndex = 1;
        }

        public void addPlayers(List<Player> players) {
            this.players.AddRange(players);
        }

        public void addPlayer(Player player) {
            players.Add(player);
        }

        public void createPlayers() {
            for (int index = 1; index <= 4; index++) {
                if (index == 1) {
                    hero = new Hero(index);
                    players.Add(hero);
                }
                else {
                    Guard guard = new Guard(index);
                    guards.Add(guard);
                    players.Add(guard);
                }
            }
            isPlaying = true;
        }

        public bool changeRoles() {

            if (heroIndex == players.Count) {
                isPlaying = false;
                return false;
            }

            heroIndex++;
            guards.Clear();
            for (int index = 0; index < players.Count; index++) {
                if ((index + 1) == heroIndex) {
                    hero = new Hero(players[index]);
                }
                else {
                    Guard guard = new Guard(players[index]);
                    guards.Add(guard);
                }
            }
            return true;
        }
    }
}
