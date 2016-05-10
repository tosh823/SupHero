using UnityEngine;
using System.Collections;

namespace SupHero.Components {
    public class BaseVisualEffect : MonoBehaviour {

        protected Animator mecanim;

        public virtual void Start() {
            mecanim = GetComponent<Animator>();
        }

        public virtual void Update() {
            
        }

        public void destroyEffect() {
            Destroy(gameObject);
        }
    }
}
