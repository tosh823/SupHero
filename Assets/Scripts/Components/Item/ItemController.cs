using SupHero.Components.Character;
using UnityEngine;

namespace SupHero.Components.Item {
    public class ItemController : MonoBehaviour {

        public ItemData item;

        protected PlayerController owner;
        protected AudioSource audioSource;
        protected float timeBetweenUsage;

        public virtual void Start() {
            owner = GetComponentInParent<PlayerController>();
            audioSource = GetComponent<AudioSource>();
            // If item has passive attributes, turn them on
            if (item.hasPassive) enablePassive();
        }

        public virtual void Update() {

        }

        public virtual bool activeReady() {
            if (item.hasActive) {
                // Check cooldown here
                return true;
            }
            else return false;
        }

        public virtual void activate() {
            if (activeReady()) {
                trigger();
            }
        }

        // ATTENTION: Assuming all checks
        // are made before trigger
        // Overdrive this method in children
        // to add custom behavior on using
        protected virtual void trigger() {
            
        }

        protected virtual void enablePassive() {

        }

        protected virtual void playSound(AudioClip sound, bool inLoop = false) {
            if (sound != null) {
                audioSource.clip = sound;
                audioSource.loop = inLoop;
                audioSource.Play();
            }
        }

        protected void playActivationSound() {
            playSound(item.activationSound);
        }
    }
}
