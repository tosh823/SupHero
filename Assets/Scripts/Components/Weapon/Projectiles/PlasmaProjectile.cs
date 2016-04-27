using UnityEngine;
using System.Collections;


namespace SupHero.Components.Weapon {
    public class PlasmaProjectile : Projectile {

        private Animator mecanim;

        // Use this for initialization
        public override void Start() {
            base.Start();
            mecanim = GetComponent<Animator>();
        }

        public override void Launch(Vector3 start, Vector3 direction) {
            base.Launch(start, direction);
            mecanim.SetTrigger("run");
        }
    }
}
