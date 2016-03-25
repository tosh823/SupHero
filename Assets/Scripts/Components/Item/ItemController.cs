using UnityEngine;

namespace SupHero.Components {
    public class ItemController : MonoBehaviour {

        public ItemData item;

        protected PlayerController owner;
        protected AudioSource audioSource;
        protected float timeBetweenUsage;

        public virtual void Start() {
            owner = GetComponentInParent<PlayerController>();
            audioSource = GetComponent<AudioSource>();
        }

        public virtual void Update() {

        }

        public virtual bool canUseItem() {
            return true;
        }

        public virtual void useItem() {
            if (canUseItem()) {
                trigger();
            }
        }

        protected virtual void trigger() {
            // Overdrive this method in children
            // to add custom behavior on using

            // ATTENTION: Assuming all checks
            // are made before trigger
        }

        protected virtual void playSound(AudioClip sound, bool inLoop = false) {
            if (sound != null) {
                audioSource.clip = sound;
                audioSource.loop = inLoop;
                audioSource.Play();
            }
        }
    }
}
