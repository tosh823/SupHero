using UnityEngine;
using System.Collections;
using System.Collections.Generic;

namespace SupHero.Model {
    public class Level {

        public List<Player> players;
        public Hero hero;
        public List<Guard> guards;
        public bool isPlaying;

        private int heroIndex;

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
            string[] gamepads = Input.GetJoystickNames();
            for (int index = 1; index <= Constants.playersCount; index++) {
                if (index == 1) {
                    hero = new Hero(index);
                    if (gamepads.Length >= Constants.playersCount) {
                        hero.inputType = InputType.GAMEPAD;
                        hero.gamepadNumber = index;
                        hero.gamepadName = gamepads[index - 1];
                    }
                    else hero.inputType = InputType.KEYBOARD;
                    players.Add(hero);
                }
                else {
                    Guard guard = new Guard(index);
                    if (gamepads.Length >= Constants.playersCount) {
                        guard.inputType = InputType.GAMEPAD;
                        guard.gamepadNumber = index;
                        guard.gamepadName = gamepads[index - 1];
                    }
                    else if (gamepads.Length >= (index - 1)) {
                        guard.inputType = InputType.GAMEPAD;
                        guard.gamepadNumber = index - 1;
                        guard.gamepadName = gamepads[index - 2];
                    }
                    else guard.inputType = InputType.NONE;
                    guards.Add(guard);
                    players.Add(guard);
                }
            }
            isPlaying = true;
        }

        public bool changeRoles() {
            List<Player> tmp = new List<Player>();
            if (heroIndex == players.Count) {
                isPlaying = false;
                return false;
            }

            heroIndex++;
            guards.Clear();
            for (int index = 0; index < players.Count; index++) {
                if ((index + 1) == heroIndex) {
                    hero = new Hero(players[index]);
                    tmp.Add(hero);
                }
                else {
                    Guard guard = new Guard(players[index]);
                    guards.Add(guard);
                    tmp.Add(guard);
                }
            }
            players.Clear();
            players = tmp;
            return true;
        }
    }
}
