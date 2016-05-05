using SupHero.Components.Character;
using UnityEngine;

namespace SupHero.Components.Item {

    public enum ItemStatus {
        ACTIVE_READY,
        NEED_AIM,
        COOLDOWN,
        ONLY_PASSIVE,
        NONE
    }

    public class ItemController : MonoBehaviour {

        public ItemData item;
        public PlayerController owner;

        protected AudioSource audioSource;
        protected bool ready = true;

        public virtual void Start() {
            audioSource = GetComponent<AudioSource>();
        }

        public virtual void Update() {

        }

        // Must define in child classes
        // Otherwise it is useless
        public virtual ItemStatus checkStatus() {
            return ItemStatus.NONE;
        }

        public virtual void Activate() {
            Trigger();
        }

        // ATTENTION: Assuming all checks
        // are made before trigger
        // Overdrive this method in children
        // to add custom behavior on using
        protected virtual void Trigger() {
            
        }

        protected virtual void Cooldown() {
            Timer cooldown = gameObject.AddComponent<Timer>();
            cooldown.time = item.activeData.cooldown;
            cooldown.OnEnd += delegate () {
                ready = true;
                Debug.Log(item.name + " is ready");
            };
            cooldown.Launch();
        }

        protected virtual void enablePassive() {

        }

        protected virtual void disablePassive() {

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
