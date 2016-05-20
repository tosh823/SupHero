using UnityEngine;
using System.Collections;

namespace SupHero.Components.Level {
    public class MeshCover : BaseDestructable {

        public Mesh halfDestroyed;
        public Mesh fullDestroyed;

        private float initialDurability;
        private MeshFilter model;
        
        public override void Start() {
            base.Start();
            initialDurability = durability;
            model = GetComponentInChildren<MeshFilter>();
        }

        public override void Update() {
            base.Update();
        }

        public override void takeDamage(float damage) {
            base.takeDamage(damage);
            if (durability <= 0.2f * initialDurability) {
                model.mesh = fullDestroyed;
            }
            else if (durability <= 0.6f * initialDurability) {
                model.mesh = halfDestroyed;
            }
        }
    }
}
