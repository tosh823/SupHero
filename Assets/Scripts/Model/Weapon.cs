using UnityEngine;
using System.Collections;

namespace SupHero.Model {
    public class Weapon {

        public int damage { get; protected set; }
        public float rate { get; protected set; }
        public float range { get; protected set; }

        public Weapon() {
            damage = 10;
            rate = 0.3f;
            range = 10f;
        }

        public bool trigger() {
            return true;
        }
    }
}
