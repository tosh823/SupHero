using UnityEngine;
using System.Collections;

namespace SupHero.Model {
    public class Guard : Player {

        public Guard(int number) : base(number) {
            playerName = "Guard";
        }
    }
}
