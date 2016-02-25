using UnityEngine;
using System.Collections;

namespace SupHero {
    public class Player : MonoBehaviour {

        protected int armor = Constants.defaultArmor;
        protected int health = Constants.defaultHealth;
        ItemSlot[] itemSlots = new ItemSlot[2];
        protected string playerName;

        // Use this for initialization
        void Start() {

        }

        // Update is called once per frame
        void Update() {

        }
    }
}
