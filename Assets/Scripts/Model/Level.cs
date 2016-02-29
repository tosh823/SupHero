using UnityEngine;
using System.Collections;
using System.Collections.Generic;

namespace SupHero.Model {
    public class Level {

        public List<Player> players;
        public Hero hero;
        public List<Guard> guards;
        public Hero currentHero;
        private int heroIndex;

        public Level() {
            players = new List<Player>();
            hero = new Hero();
            guards = new List<Guard>();
            heroIndex = 0;
        }

        public void addPlayers(List<Player> players) {
            this.players.AddRange(players);
        }

        public void addPlayer(Player player) {
            players.Add(player);
        }

        // TODO!!!
        // Complete players initialization
        // Downcasting is not possible
        public void setupRoles() {
            for (int index = 0; index < players.Count; index++) {
                if (index == 0) {
                    //hero = players[index] as Hero;
                    //players[index] = players[index] as Hero;
                    hero = players[index] as Hero;
                    if (hero != null) {
                        Debug.Log(hero.GetType().ToString() + " with number " + hero.number + " on the battlefield");
                    }
                    
                }
                else {
                    //guards.Add(players[index] as Guard);
                    //players[index] = players[index] as Guard;
                    Guard guard = players[index] as Guard;
                    if (guard != null) {
                        Debug.Log(guard.GetType().ToString() + " with number " + guard.number + " on the battlefield");
                        guards.Add(guard);
                    }
                }
            }
        }

        public void changeRoles() {
            heroIndex++;
            guards.Clear();
            for (int index = 0; index < players.Count; index++) {
                if (index == heroIndex) {
                    hero = players[index] as Hero;
                }
                else {
                    guards.Add(players[index] as Guard);
                }
            }
        }
    }
}
