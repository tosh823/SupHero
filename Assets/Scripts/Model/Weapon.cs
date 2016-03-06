using UnityEngine;
using System.Collections;

namespace SupHero.Model {
    public class Weapon {

        public float damage { get; protected set; }
        public float rate { get; protected set; }
        public float range { get; protected set; }

        public Weapon() {
            damage = Constants.damageMedium;
            rate = Constants.rateNormal;
            range = Constants.rangeShort;
        }

        public bool trigger() {
            return true;
        }
    }
}
