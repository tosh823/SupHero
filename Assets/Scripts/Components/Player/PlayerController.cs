using UnityEngine;
using System;
using System.Collections;
using SupHero;
using SupHero.Model;
using SupHero.Components.Level;
using SupHero.Components.Weapon;
using SupHero.Components.Item;

namespace SupHero.Components.Character {

    public struct State {
        public static string MOVING = "moving";
        public static string ROTATION = "rotation";
        public static string STEADY = "steady";
        public static string TRIGGER = "trigger";
        public static string HOR = "horizontal";
        public static string VERT = "vertical";
        public static string DIE = "die";
        public static string RATE = "rate";
        public static string SPEED = "speed";
    }

    public class PlayerController : MonoBehaviour {

        // Variables
        public Player player;
        public string tokenName;
        public bool gamePadControl = false;
        public bool isHero = true;
        public Transform directionMark;
        
        private GameObject playerUI; // Ref to UI for this player, possibly unnessecary
        private Vector3 moveVector; // Vector for moving character
        private Vector3 rotation; // Vector for rotating character
        private Vector3 oldLookRotation;

        // Input
        private bool usePrimaryWeapon;
        private bool useSecondaryWeapon;
        private bool useFirstItem;
        private bool useSecondItem;

        private bool aimMode;

        // Components
        private ZoneController zone; // Ref to current zone
        private Rigidbody playerRigidbody;
        private Animator mecanim; // Animator, attached to this player
        private Inventory inventory; // Store for weapons and items

        // Events
        public delegate void dieAction(Player player);
        public event dieAction OnDie;
        public delegate void receiveDamageAction();
        public event receiveDamageAction OnDamageReceived;

        public delegate void primaryDownAction();
        public event primaryDownAction OnPrimaryDown;
        public delegate void primaryHoldAction();
        public event primaryHoldAction OnPrimaryHold;
        public delegate void primaryUpAction();
        public event primaryUpAction OnPrimaryUp;

        private primaryUpAction prepareItemUpAction;

        void Start() {
            if (player == null) {
                // For standalone unit, like for test scene
                if (isHero) {
                    player = new Hero(1);
                    if (gamePadControl) {
                        player.inputType = InputType.GAMEPAD;
                        player.gamepadNumber = 1;
                    }
                    else {
                        player.inputType = InputType.KEYBOARD;
                    }
                }
                else {
                    player = new Guard(1);
                    if (gamePadControl) {
                        player.inputType = InputType.GAMEPAD;
                        player.gamepadNumber = 1;
                    }
                    else {
                        player.inputType = InputType.KEYBOARD;
                    }
                }
                
            }
            playerRigidbody = GetComponent<Rigidbody>();
            mecanim = GetComponent<Animator>();
            zone = GetComponentInParent<ZoneController>();
            inventory = GetComponent<Inventory>();
            inventory.setupWeapons();
            inventory.setupItems();
            drawPrimary();
            usePrimaryWeapon = false;
            useSecondaryWeapon = false;
            useFirstItem = false;
            useSecondItem = false;
            aimMode = false;
            directionMark.gameObject.SetActive(false);
        }

        public void setAnimator(AnimatorOverrideController controller) {
            mecanim.runtimeAnimatorController = controller;
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
                    readInput();
                    // Processing gathered input
                    processWeaponInput();
                    processItemInput();

                    move();
                    rotate();
                }
            }
            else {
                // Invoking die in animation event
                mecanim.SetTrigger(State.DIE);
            }
        }

        private void move() {
            if (moveVector != null && moveVector != Vector3.zero) {
                mecanim.SetBool(State.MOVING, true);
                // For animation accordingly to look orientation
                float verticalRelative;
                if (moveVector.z > 0f) {
                    if (transform.forward.z > 0f) verticalRelative = moveVector.z;
                    else verticalRelative = -moveVector.z;
                }
                else {
                    if (transform.forward.z < 0f) verticalRelative = -moveVector.z;
                    else verticalRelative = moveVector.z;
                }

                float horizontalRelative;
                if (moveVector.x > 0f) {
                    if (transform.right.x > 0f) horizontalRelative = moveVector.x;
                    else horizontalRelative = -moveVector.x;
                }
                else {
                    if (transform.right.x < 0f) horizontalRelative = -moveVector.x;
                    else horizontalRelative = moveVector.x;
                }
                mecanim.SetFloat(State.SPEED, player.speed);
                mecanim.SetFloat(State.VERT, verticalRelative);
                mecanim.SetFloat(State.HOR, horizontalRelative);
                // For moving relative to camera
                Vector3 forward = Camera.main.transform.TransformDirection(Vector3.forward);
                forward.y = 0f;
                forward = forward.normalized;
                Vector3 right = new Vector3(forward.z, 0f, -forward.x);
                moveVector = (moveVector.x * right + moveVector.z * forward);
                // Finally, moving!
                moveVector = moveVector.normalized * player.speed * Time.deltaTime;
                playerRigidbody.MovePosition(transform.position + moveVector);
            }
            else {
                mecanim.SetFloat(State.SPEED, player.speed);
                mecanim.SetFloat(State.VERT, 0f);
                mecanim.SetFloat(State.HOR, 0f);
                mecanim.SetBool(State.MOVING, false);
            }
        }

        private void rotate() {
            if (rotation != null && rotation != Vector3.zero) {
                float smoothing = 4f;
                Quaternion rotate = Quaternion.LookRotation(rotation);
                Quaternion smoothRotation = Quaternion.Lerp(transform.rotation, rotate, smoothing * Time.deltaTime);
                transform.rotation = smoothRotation;
                // If we have old rotation, check if need to apply animation
                if (oldLookRotation != null) {
                    float threshold = 0.2f;
                    if (Mathf.Abs(rotation.x - oldLookRotation.x) >= threshold) {
                        mecanim.SetFloat(State.ROTATION, rotation.normalized.x);
                    }
                    else {
                        mecanim.SetFloat(State.ROTATION, 0f);
                    }
                }
                oldLookRotation = rotation;
            }
        }

        // In fixed update we apply motion and rotaion
        // According to move and rotation vectors
        void FixedUpdate() {
            // Moving
            /*if (moveVector != null && moveVector != Vector3.zero) {
                mecanim.SetBool(State.MOVING, true);
                // For animation accordingly to look orientation
                float verticalRelative;
                if (moveVector.z > 0f) {
                    if (transform.forward.z > 0f) verticalRelative = moveVector.z;
                    else verticalRelative = -moveVector.z;
                }
                else {
                    if (transform.forward.z < 0f) verticalRelative = -moveVector.z;
                    else verticalRelative = moveVector.z;
                }

                float horizontalRelative;
                if (moveVector.x > 0f) {
                    if (transform.right.x > 0f) horizontalRelative = moveVector.x;
                    else horizontalRelative = -moveVector.x;
                }
                else {
                    if (transform.right.x < 0f) horizontalRelative = -moveVector.x;
                    else horizontalRelative = moveVector.x;
                }
                mecanim.SetFloat(State.SPEED, player.speed);
                mecanim.SetFloat(State.VERT, verticalRelative);
                mecanim.SetFloat(State.HOR, horizontalRelative);
                // For moving relative to camera
                Vector3 forward = Camera.main.transform.TransformDirection(Vector3.forward);
                forward.y = 0f;
                forward = forward.normalized;
                Vector3 right = new Vector3(forward.z, 0f, -forward.x);
                moveVector = (moveVector.x * right + moveVector.z * forward);
                // Finally, moving!
                moveVector = moveVector.normalized * player.speed * Time.deltaTime;
                playerRigidbody.MovePosition(transform.position + moveVector);
            }
            else {
                mecanim.SetFloat(State.SPEED, player.speed);
                mecanim.SetFloat(State.VERT, 0f);
                mecanim.SetFloat(State.HOR, 0f);
                mecanim.SetBool(State.MOVING, false);
            }
            // Turning
            if (rotation != null && rotation != Vector3.zero) {
                float smoothing = 2.8f;
                Quaternion rotate = Quaternion.LookRotation(rotation);
                Quaternion smoothRotation = Quaternion.Lerp(transform.rotation, rotate, smoothing * Time.deltaTime);
                playerRigidbody.MoveRotation(smoothRotation);
                // If we have old rotation, check if need to apply animation
                if (oldLookRotation != null) {
                    float threshold = 0.2f;
                    if (Mathf.Abs(rotation.x - oldLookRotation.x) >= threshold) {
                        mecanim.SetFloat(State.ROTATION, rotation.normalized.x);
                    }
                    else {
                        mecanim.SetFloat(State.ROTATION, 0f);
                    }
                }
                oldLookRotation = rotation;
            }*/
        }

        // Draw primary weapon from inventory
        public void drawPrimary() {
            if (player is Hero) {
                mecanim.runtimeAnimatorController = inventory.primaryWeapon.weapon.controller;
            }
            else {
                mecanim.runtimeAnimatorController = inventory.primaryWeapon.weapon.guardVersion;
            }
            hideWeapon(inventory.secondaryWeapon);
            inventory.primaryWeapon.gameObject.SetActive(true);
            mecanim.SetFloat(State.RATE, inventory.primaryWeapon.weapon.rate / 60f);
        }

        // Draw secondary weapon from inventory
        public void drawSecondary() {
            if (player is Hero) {
                mecanim.runtimeAnimatorController = inventory.secondaryWeapon.weapon.controller;
            }
            else {
                mecanim.runtimeAnimatorController = inventory.secondaryWeapon.weapon.guardVersion;
            }
            hideWeapon(inventory.primaryWeapon);
            inventory.secondaryWeapon.gameObject.SetActive(true);
            mecanim.SetFloat(State.RATE, inventory.secondaryWeapon.weapon.rate / 60f);
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

        public void receiveDrop(Entity type, int id) {
            switch (type) {
                case Entity.WEAPON:
                    inventory.equipWeapon(id, true);
                    break;
                case Entity.ITEM:
                    inventory.equipItem(id);
                    break;
                case Entity.SUPPLY:
                    inventory.useSupply(id);
                    break;
                default:
                    break;
            }
        }

        public void enableAimMode() {
            aimMode = true;
            directionMark.gameObject.SetActive(true);
        }

        public void disableAimMode() {
            aimMode = false;
            directionMark.gameObject.SetActive(false);
        }

        // Reading weapon input
        private void readInput() {
            bool primaryDown, primaryHold, primaryUp;
            switch (player.inputType) {
                case InputType.KEYBOARD:
                    // For processing
                    usePrimaryWeapon = Input.GetButton(Control.LeftMouse);
                    useSecondaryWeapon = Input.GetButton(Control.RightMouse);
                    useFirstItem = Input.GetButtonUp(Control.Q);
                    useSecondItem = Input.GetButtonUp(Control.E);

                    // For firing events
                    primaryDown = Input.GetButtonDown(Control.LeftMouse);
                    primaryHold = Input.GetButton(Control.LeftMouse);
                    primaryUp = Input.GetButtonUp(Control.LeftMouse);

                    break;
                case InputType.GAMEPAD:
                    float rightTrigger = Input.GetAxis(Utils.getControlForPlayer(Control.RightTrigger, player.gamepadNumber));
                    float leftTrigger = Input.GetAxis(Utils.getControlForPlayer(Control.LeftTrigger, player.gamepadNumber));

                    // usePrimaryWeapon holds old value
                    // so we could use it for defining down, hold and up events
                    primaryDown = (!usePrimaryWeapon && (rightTrigger > 0f));
                    primaryHold = (rightTrigger > 0f);
                    primaryUp = (usePrimaryWeapon && (rightTrigger <= 0.1f));

                    usePrimaryWeapon = primaryHold;
                    useSecondaryWeapon = (leftTrigger > 0f);
                    useFirstItem = Input.GetButtonUp(Utils.getControlForPlayer(Control.LeftBumper, player.gamepadNumber));
                    useSecondItem = Input.GetButtonUp(Utils.getControlForPlayer(Control.RightBumper, player.gamepadNumber));

                    break;
                default:
                    usePrimaryWeapon = false;
                    useSecondaryWeapon = false;
                    useFirstItem = false;
                    useSecondItem = false;
                    primaryDown = false;
                    primaryHold = false;
                    primaryUp = false;
                    break;
            }

            // Firing freakin events
            if (primaryDown && OnPrimaryDown != null) OnPrimaryDown();
            if (primaryHold && OnPrimaryHold != null) OnPrimaryHold();
            if (primaryUp && OnPrimaryUp != null) OnPrimaryUp();

        }

        private void processWeaponInput() {
            // Attack with primary
            if (!aimMode && usePrimaryWeapon) {
                // New behavior
                if (isWeaponActive(inventory.primaryWeapon)) {
                    mecanim.SetBool(State.STEADY, true);
                    if (inventory.primaryWeapon.canUseWeapon()) mecanim.SetBool(State.TRIGGER, true);
                    else mecanim.SetBool(State.TRIGGER, false);
                }
                else {
                    drawPrimary();
                }
            }
            // Attack with secondary
            else if (!aimMode && useSecondaryWeapon) {
                if (isWeaponActive(inventory.secondaryWeapon)) {
                    mecanim.SetBool(State.STEADY, true);
                    if (inventory.secondaryWeapon.canUseWeapon()) mecanim.SetBool(State.TRIGGER, true);
                    else mecanim.SetBool(State.TRIGGER, false);
                }
                else {
                    drawSecondary();
                }
            }
            // No attacking at all
            else {
                mecanim.SetBool(State.STEADY, false);
                mecanim.SetBool(State.TRIGGER, false);
            }
        }

        private void processItemInput() {
            if (useFirstItem) {
                tryUseItem(inventory.firstItem);
            }
            else if (useSecondItem) {
                tryUseItem(inventory.secondItem);
            }
        }

        private void tryUseItem(ItemController item) {
            ItemStatus status = item.checkStatus();
            switch (status) {
                case ItemStatus.ACTIVE_READY:
                    useItem(item);
                    break;
                case ItemStatus.NEED_AIM:
                    enableAimMode();
                    prepareItemUpAction = delegate () {
                        Debug.Log("Item aimed");
                        disableAimMode();
                        useItem(item);
                    };
                    OnPrimaryUp += prepareItemUpAction;
                    break;
                case ItemStatus.COOLDOWN:
                    Debug.Log("Item is cooling down");
                    break;
                case ItemStatus.ONLY_PASSIVE:
                    Debug.Log("Item doesn't have active ability");
                    break;
                default:
                    break;
            }
        }

        public void useWeapon() {
            if (isWeaponActive(inventory.primaryWeapon)) {
                inventory.primaryWeapon.useWeapon();
            }
            else if (isWeaponActive(inventory.secondaryWeapon)) {
                inventory.secondaryWeapon.useWeapon();
            }
        }

        public void useItem(ItemController item) {
            Debug.Log("Use item");
            item.activate();
            if (prepareItemUpAction != null) OnPrimaryUp -= prepareItemUpAction;
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
