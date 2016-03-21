using UnityEngine;
using System.Collections;
using SupHero;
using SupHero.Model;

namespace SupHero.Components {
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
        public delegate void takeDamageAction();
        public event dieAction OnDie;
        public event takeDamageAction OnTakenDamage;

        // Use this for initialization
        void Start() {
            playerRigidbody = GetComponent<Rigidbody>();
            animator = GetComponent<Animator>();
            zone = GetComponentInParent<ZoneController>();
            inventory = GetComponent<Inventory>();

            if (player == null) {
                // For standalone unit, like for test scene
                player = new Hero(1);
                player.inputType = InputType.KEYBOARD;
            }
        }

        // Update is called once per frame
        void Update() {
            if (player.isAlive && transform.position.y >= -10f) {
                // Record move input
                moveVector = getMovementVector();
                // Record rotate input
                rotation = getRotation();
                // Record actions
                readWeaponInput();
            }
            else {
                die();
            }
        }

        void FixedUpdate() {
            // Moving
            if (moveVector != null && moveVector != Vector3.zero) {
                // For moving relative to camera
                animator.SetBool("moving", true);
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
                animator.SetFloat("vertical", verticalRelative);
                animator.SetFloat("horizontal", horizontalRelative);

                Vector3 forward = Camera.main.transform.TransformDirection(Vector3.forward);
                forward.y = 0f;
                forward = forward.normalized;
                Vector3 right = new Vector3(forward.z, 0f, -forward.x);
                moveVector = (moveVector.x * right + moveVector.z * forward);

                moveVector = moveVector.normalized * player.speed * Time.deltaTime;
                playerRigidbody.MovePosition(transform.position + moveVector);
            }
            else {
                animator.SetBool("moving", false);
            }
            // Turning
            if (rotation != null && rotation != Vector3.zero) {
                float smoothing = 2.5f;
                Quaternion rotate = Quaternion.LookRotation(rotation);
                Quaternion smoothRotation = Quaternion.Lerp(transform.rotation, rotate, smoothing * Time.deltaTime);
                playerRigidbody.MoveRotation(smoothRotation);
            }
        }

        public void drawPrimary() {
            hideWeapon(inventory.secondary);
            inventory.primary.gameObject.SetActive(true);
        }

        public void drawSecondary() {
            hideWeapon(inventory.primary);
            inventory.secondary.gameObject.SetActive(true);
        }

        public void hideWeapon(WeaponController weapon) {
            if (weapon != null && weapon.gameObject.activeInHierarchy) {
                weapon.gameObject.SetActive(false);
            }
        }

        public bool isWeaponActive(WeaponController weapon) {
            if (weapon != null && weapon.gameObject.activeInHierarchy) {
                return true;
            }
            else return false;
        }

        public DamageResult takeDamage(float damage) {
            if (OnTakenDamage != null) {
                OnTakenDamage();
            }
            return player.takeDamage(damage);
        }

        public void setPlayer(Player player) {
            this.player = player;
            // If player is a hero
            if (player is Hero) {
                // Set it as camera target
                LevelController.instance.view.setTarget(gameObject);
                // Add to him a shield
                Shield sc = gameObject.AddComponent<Shield>();
                sc.owner = (Hero) player;
                OnTakenDamage += sc.refreshTimer;
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
                    usePrimaryWeapon = Input.GetButton("Fire1");
                    useSecondaryWeapon = Input.GetButton("Fire2");
                    break;
                case InputType.GAMEPAD:
                    float rightBumper = Input.GetAxis(Utils.getControlForPlayer("R2", player.gamepadNumber));
                    float leftBumper = Input.GetAxis(Utils.getControlForPlayer("L2", player.gamepadNumber));
                    usePrimaryWeapon = (rightBumper > 0f);
                    useSecondaryWeapon = (leftBumper > 0f);
                    break;
                default:
                    break;
            }
            // Attack with primary
            if (usePrimaryWeapon) {
                if (isWeaponActive(inventory.primary) && inventory.primary.canUseWeapon()) {
                    animator.SetBool("attacking", true);
                }
                else {
                    animator.SetBool("attacking", false);
                    animator.SetBool("secondary", false);
                    animator.SetBool("primary", true);
                    drawPrimary();
                } 
            }
            // Attack with secondary
            else if (useSecondaryWeapon) {
                if (isWeaponActive(inventory.secondary) && inventory.secondary.canUseWeapon()) {
                    animator.SetBool("attacking", true);
                }
                else {
                    animator.SetBool("attacking", false);
                    animator.SetBool("primary", false);
                    animator.SetBool("secondary", true);
                    drawSecondary();
                }
            }
            // No attacking at all
            else {
                animator.SetBool("attacking", false);
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
                    h = Input.GetAxis("Horizontal");
                    v = Input.GetAxis("Vertical");
                    movement = new Vector3(h, 0f, v);
                    break;
                case InputType.GAMEPAD:
                    h = Input.GetAxis(Utils.getControlForPlayer("LeftStickX", player.gamepadNumber));
                    v = Input.GetAxis(Utils.getControlForPlayer("LeftStickY", player.gamepadNumber));
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
                    float x = Input.GetAxis(Utils.getControlForPlayer("RightStickX", player.gamepadNumber));
                    float z = Input.GetAxis(Utils.getControlForPlayer("RightStickY", player.gamepadNumber));
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
