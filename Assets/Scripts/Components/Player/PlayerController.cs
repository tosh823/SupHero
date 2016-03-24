using UnityEngine;
using System.Collections;
using SupHero;
using SupHero.Model;

namespace SupHero.Components {

    public struct State {
        public static string MOVING = "moving";
        public static string STEADY = "steady";
        public static string TRIGGER = "trigger";
        public static string PRIMARY = "primary";
        public static string SECONDARY = "secondary";
        public static string HOR = "horizontal";
        public static string VERT = "vertical";
    }

    public class PlayerController : MonoBehaviour {

        // Variables
        public Player player;
        
        private GameObject playerUI; // Ref to UI for this player, possibly unnessecary
        private Vector3 moveVector; // Vector for moving character
        private Vector3 rotation; // Vector for rotating character

        // Components
        private ZoneController zone; // Ref to current zone
        private Rigidbody playerRigidbody;
        private Animator animator; // Animator, attached to this player
        private Inventory inventory; // Store for weapons and items

        // Events
        public delegate void dieAction(Player player);
        public event dieAction OnDie;
        public delegate void receiveDamageAction();
        public event receiveDamageAction OnDamageReceived;

        void Start() {
            if (player == null) {
                // For standalone unit, like for test scene
                player = new Hero(1);
                player.inputType = InputType.KEYBOARD;
            }

            playerRigidbody = GetComponent<Rigidbody>();
            animator = GetComponent<Animator>();
            zone = GetComponentInParent<ZoneController>();
            inventory = GetComponent<Inventory>();
            inventory.setupWeapons();
        }

        // In update we read input and check state of the player
        void Update() {
            if (player.isAlive && transform.position.y >= -10f) {
                if (!player.isStunned) {
                    // Record move input
                    moveVector = getMovementVector();
                    // Record rotate input
                    rotation = getRotation();
                    // Record actions
                    readWeaponInput();
                }
            }
            else {
                die();
            }
        }

        // In fixed update we apply motion and rotaion
        void FixedUpdate() {
            // Moving
            if (moveVector != null && moveVector != Vector3.zero) {
                // For moving relative to camera
                animator.SetBool(State.MOVING, true);
                float verticalRelative;
                if (moveVector.normalized.z != 0f) {
                    verticalRelative = transform.forward.z - moveVector.normalized.z;
                }
                else {
                    verticalRelative = moveVector.normalized.z;
                } 
                float horizontalRelative;
                if (moveVector.normalized.x != 0f) {
                    horizontalRelative = transform.right.x - moveVector.normalized.x;
                }
                else {
                    horizontalRelative = moveVector.normalized.x;
                }
                animator.SetFloat(State.VERT, verticalRelative);
                animator.SetFloat(State.HOR, horizontalRelative);

                Vector3 forward = Camera.main.transform.TransformDirection(Vector3.forward);
                forward.y = 0f;
                forward = forward.normalized;
                Vector3 right = new Vector3(forward.z, 0f, -forward.x);
                moveVector = (moveVector.x * right + moveVector.z * forward);
                moveVector = moveVector.normalized * player.speed * Time.deltaTime;
                playerRigidbody.MovePosition(transform.position + moveVector);
            }
            else {
                animator.SetBool(State.MOVING, false);
            }
            // Turning
            if (rotation != null && rotation != Vector3.zero) {
                float smoothing = 2.8f;
                Quaternion rotate = Quaternion.LookRotation(rotation);
                Quaternion smoothRotation = Quaternion.Lerp(transform.rotation, rotate, smoothing * Time.deltaTime);
                playerRigidbody.MoveRotation(smoothRotation);
            }
        }

        // Draw primary weapon from inventory
        public void drawPrimary() {
            hideWeapon(inventory.secondary);
            inventory.primary.gameObject.SetActive(true);
        }

        // Draw secondary weapon from inventory
        public void drawSecondary() {
            hideWeapon(inventory.primary);
            inventory.secondary.gameObject.SetActive(true);
        }

        // Hide weapon
        public void hideWeapon(WeaponController weapon) {
            if (weapon != null && weapon.gameObject.activeInHierarchy) {
                weapon.gameObject.SetActive(false);
            }
        }

        // Is weapon currenlty held in hands
        public bool isWeaponActive(WeaponController weapon) {
            if (weapon != null && weapon.gameObject.activeInHierarchy) {
                return true;
            }
            else return false;
        }

        // Receive damage
        public DamageResult receiveDamage(float damage) {
            if (OnDamageReceived != null) {
                OnDamageReceived();
            }
            return player.receiveDamage(damage);
        }

        // Apply effect
        public void applyEffect(EffectData effect) {
            switch (effect.type) {
                case EffectType.FIRE:
                    FireEffect fire = gameObject.AddComponent<FireEffect>();
                    fire.effect = effect;
                    break;
                case EffectType.POISON:
                    PoisonEffect poison = gameObject.AddComponent<PoisonEffect>();
                    poison.effect = effect;
                    break;
                case EffectType.SLOWDOWN:
                    SlowdownEffect slow = gameObject.AddComponent<SlowdownEffect>();
                    slow.effect = effect;
                    break;
                case EffectType.STUN:
                    StunEffect stun = gameObject.AddComponent<StunEffect>();
                    stun.effect = effect;
                    break;
                default:
                    break;
            }
        }

        public void setPlayer(Player player) {
            this.player = player;
            // If player is a hero
            if (player is Hero) {
                // Set it as camera target
                LevelController.Instance.view.setTarget(gameObject);
                // Add to him a shield
                Shield sc = gameObject.AddComponent<Shield>();
                sc.owner = (Hero) player;
                OnDamageReceived += sc.refreshTimer;
            }
        }

        public void setUI(GameObject ui) {
            playerUI = ui;
        }

        public void die() {
            if (OnDie != null) {
                player.die();
                OnDie(player);
            }
        }

        // Reading weapon input
        private void readWeaponInput() {
            bool usePrimaryWeapon = false;
            bool useSecondaryWeapon = false;
            switch (player.inputType) {
                case InputType.KEYBOARD:
                    usePrimaryWeapon = Input.GetButton(Control.LeftMouse);
                    useSecondaryWeapon = Input.GetButton(Control.RightMouse);
                    break;
                case InputType.GAMEPAD:
                    float rightTrigger = Input.GetAxis(Utils.getControlForPlayer(Control.RightTrigger, player.gamepadNumber));
                    float leftTrigger = Input.GetAxis(Utils.getControlForPlayer(Control.LeftTrigger, player.gamepadNumber));
                    usePrimaryWeapon = (rightTrigger > 0f);
                    useSecondaryWeapon = (leftTrigger > 0f);
                    break;
                default:
                    break;
            }
            // Attack with primary
            if (usePrimaryWeapon) {
                // New behavior
                if (isWeaponActive(inventory.primary)) {
                    animator.SetBool(State.STEADY, true);
                    if (inventory.primary.canUseWeapon()) animator.SetBool(State.TRIGGER, true);
                    else animator.SetBool(State.TRIGGER, false);
                }
                else {
                    animator.SetBool(State.SECONDARY, false);
                    animator.SetBool(State.PRIMARY, true);
                    drawPrimary();
                }
            }
            // Attack with secondary
            else if (useSecondaryWeapon) {
                if (isWeaponActive(inventory.secondary)) {
                    animator.SetBool(State.STEADY, true);
                    if (inventory.secondary.canUseWeapon()) animator.SetBool(State.TRIGGER, true);
                    else animator.SetBool(State.TRIGGER, false);
                }
                else {
                    animator.SetBool(State.PRIMARY, false);
                    animator.SetBool(State.SECONDARY, true);
                    drawSecondary();
                }
            }
            // No attacking at all
            else {
                animator.SetBool(State.STEADY, false);
                animator.SetBool(State.TRIGGER, false);
            }
        }

        public void useWeapon() {
            if (isWeaponActive(inventory.primary)) {
                inventory.primary.useWeapon();
            }
            else if (isWeaponActive(inventory.secondary)) {
                inventory.secondary.useWeapon();
            }
        }

        // Reading movement input
        private Vector3 getMovementVector() {
            Vector3 movement = Vector3.zero;
            float h, v;
            switch (player.inputType) {
                case InputType.KEYBOARD:
                    h = Input.GetAxis(Control.Horizontal);
                    v = Input.GetAxis(Control.Vertical);
                    movement = new Vector3(h, 0f, v);
                    break;
                case InputType.GAMEPAD:
                    h = Input.GetAxis(Utils.getControlForPlayer(Control.LeftStickX, player.gamepadNumber));
                    v = Input.GetAxis(Utils.getControlForPlayer(Control.LeftStickY, player.gamepadNumber));
                    movement = new Vector3(h, 0f, v);
                    break;
                default:
                    break;
            }
            return movement;
        }

        // Reading rotation input
        private Vector3 getRotation() {
            Vector3 rotation = Vector3.zero;
            switch (player.inputType) {
                case InputType.GAMEPAD:
                    float x = Input.GetAxis(Utils.getControlForPlayer(Control.RightStickX, player.gamepadNumber));
                    float z = Input.GetAxis(Utils.getControlForPlayer(Control.RightStickY, player.gamepadNumber));
                    if (x != 0f && z != 0f) {
                        rotation = new Vector3(x, 0f, z);
                    }
                    break;
                case InputType.KEYBOARD:
                    Ray camRay = Camera.main.ScreenPointToRay(Input.mousePosition);
                    RaycastHit floorHit;
                    if (Physics.Raycast(camRay, out floorHit)) {
                        Vector3 playerToMouse = floorHit.point - transform.position;
                        playerToMouse.y = 0f;
                        rotation = playerToMouse;
                    }
                    break;
                default:
                    break;
            }
            return rotation;
        }
    }
}
