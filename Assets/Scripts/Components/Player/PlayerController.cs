using UnityEngine;
using SupHero.Model;
using SupHero.Components.Level;
using SupHero.Components.Weapon;
using SupHero.Components.Item;
using SupHero.Components.UI;
using System.Collections;

namespace SupHero.Components.Character {

    public struct State {
        public static string MOVING = "moving";
        public static string ROTATION = "rotation";
        public static string STEADY = "steady";
        public static string TRIGGER = "trigger";
        public static string HOR = "horizontal";
        public static string VERT = "vertical";
        public static string DIE = "die";
        public static string STUN = "stun";
        public static string RATE = "rate";
        public static string SPEED = "speed";
    }

    public class PlayerController : MonoBehaviour {

        // Variables
        public Player player;
        public string tokenName;
        public bool gamePadControl = false;
        public int gamePadNumber = 0;
        public bool createHero = true;
        // For item tests
        public int firstItemId;
        public int secondItemId;

        public Transform directionMark;
        public Vector3 originPosition;
        public Renderer mainRender;
        
        public PlayerUIController playerUI;
        private Vector3 moveVector; // Vector for moving character
        private Vector3 rotationVector; // Vector for rotating character
        private Vector3 prevRotationVector;

        // Input
        private bool usePrimaryWeapon = false;
        private bool useSecondaryWeapon = false;
        private bool useFirstItem = false;
        private bool useSecondItem = false;
        private bool requestTeleport = false;
        private bool aimMode = false;

        // Components
        public ZoneController zone { get; private set; } // Ref to current zone
        private Rigidbody playerRigidbody;
        public Animator mecanim { get; private set; } // Animator, attached to this player
        private Inventory inventory; // Store for weapons and items
        public Shield shield { get; private set; } // Ref to shield if exist

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

        public delegate void secondaryDownAction();
        public event secondaryDownAction OnSecondaryDown;
        public delegate void secondaryHoldAction();
        public event secondaryHoldAction OnSecondaryHold;
        public delegate void secondaryUpAction();
        public event secondaryUpAction OnSecondaryUp;

        private primaryUpAction primaryUp;
        private secondaryUpAction secondaryUp;

        void Start() {
            if (player == null) {
                // For standalone unit, like for test scene
                if (createHero) {
                    player = new Hero(1);
                    if (gamePadControl) {
                        player.inputType = InputType.GAMEPAD;
                        player.gamepadNumber = gamePadNumber;
                    }
                    else {
                        player.inputType = InputType.KEYBOARD;
                    }
                    player.character = Data.Instance.getCharByGender(Gender.FEMALE);
                    player.firstItemId = firstItemId;
                    player.secondItemId = secondItemId;
                    setPlayer(player);
                }
                else {
                    player = new Guard(1);
                    if (gamePadControl) {
                        player.inputType = InputType.GAMEPAD;
                        player.gamepadNumber = gamePadNumber;
                    }
                    else {
                        player.inputType = InputType.KEYBOARD;
                    }
                    player.character = Data.Instance.getCharByGender(Gender.MALE);
                    player.firstItemId = firstItemId;
                    player.secondItemId = secondItemId;
                    setPlayer(player);
                }
            }
            // Components;
            playerRigidbody = GetComponent<Rigidbody>();
            mecanim = GetComponent<Animator>();
            zone = GetComponentInParent<ZoneController>();
            inventory = GetComponent<Inventory>();
            inventory.setupWeapons();
            inventory.setupItems();
            drawPrimary();
            
            directionMark.gameObject.SetActive(false);
        }

        public void setAnimator(AnimatorOverrideController controller) {
            mecanim.runtimeAnimatorController = controller;
        }

        // In update we read input and check state of the player
        void Update() {
            if (player.isAlive && !player.isStunned) {
                // Record move input
                moveVector = getMovementVector();
                // Record rotate input
                rotationVector = getRotation();
                // Record actions
                readInput();
                // Processing gathered input
                processWeaponInput();
                processItemInput();

                // Other input
                if (player is Guard && requestTeleport) {
                    zone.teleportObjectToHero(gameObject);
                }

                // Stearing
                Move();
                Rotate();
            }
            if (transform.position.y <= -2f) {
                Death();
            }
        }

        private void Move() {
            if (moveVector != Vector3.zero) {
                mecanim.SetBool(State.MOVING, true);
                // For animation accordingly to look orientation
                float verticalRelative;
                if (moveVector.z > 0f) {
                    if (transform.forward.z > 0f) verticalRelative = moveVector.normalized.z;
                    else verticalRelative = -moveVector.normalized.z;
                }
                else {
                    if (transform.forward.z < 0f) verticalRelative = -moveVector.normalized.z;
                    else verticalRelative = moveVector.normalized.z;   
                }
                float horizontalRelative;
                if (moveVector.x > 0f) {
                    if (transform.right.x > 0f) horizontalRelative = moveVector.normalized.x;
                    else horizontalRelative = -moveVector.normalized.x;
                }
                else {
                    if (transform.right.x < 0f) horizontalRelative = -moveVector.normalized.x;
                    else horizontalRelative = moveVector.normalized.x;
                }
                mecanim.SetFloat(State.SPEED, player.speed * moveVector.sqrMagnitude);
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

        private void Rotate() {
            float deadzone = 0.2f;
            if (rotationVector != Vector3.zero && rotationVector.sqrMagnitude > deadzone) {
                // Precise aiming
                Vector3 preciseRotation = rotationVector.normalized * ((rotationVector.magnitude - deadzone) / (1 - deadzone));
                Quaternion rotation = Quaternion.LookRotation(preciseRotation, transform.up);
                playerRigidbody.rotation = rotation;
                // If we have old rotation, check if need to apply rotation animation
                float threshold = 0.1f;
                if (Mathf.Abs(rotationVector.x - prevRotationVector.x) >= threshold) {
                    mecanim.SetFloat(State.ROTATION, rotationVector.normalized.x);
                }
                else {
                    mecanim.SetFloat(State.ROTATION, 0f);
                }
                prevRotationVector = rotationVector;
            }
            else {
                mecanim.SetFloat(State.ROTATION, 0f);
            }
        }

        // In fixed update we apply motion and rotaion
        // According to move and rotation vectors
        // Better not to
        /*void FixedUpdate() {
            
        }*/

        // Draw primary weapon from inventory
        public void drawPrimary() {
            if (inventory.primaryWeapon != null) {
                if (player.character.gender == Gender.FEMALE) mecanim.runtimeAnimatorController = inventory.primaryWeapon.weapon.femaleController;
                else mecanim.runtimeAnimatorController = inventory.primaryWeapon.weapon.maleController;
                hideWeapon(inventory.secondaryWeapon);
                inventory.primaryWeapon.drawWeapon();
                mecanim.SetFloat(State.RATE, inventory.primaryWeapon.weapon.rate / 60f);
                if (playerUI != null) playerUI.updateWeapon();
            }
        }

        // Draw secondary weapon from inventory
        public void drawSecondary() {
            if (inventory.secondaryWeapon != null) {
                if (player.character.gender == Gender.FEMALE) mecanim.runtimeAnimatorController = inventory.secondaryWeapon.weapon.femaleController;
                else mecanim.runtimeAnimatorController = inventory.secondaryWeapon.weapon.maleController;
                hideWeapon(inventory.primaryWeapon);
                inventory.secondaryWeapon.drawWeapon();
                mecanim.SetFloat(State.RATE, inventory.secondaryWeapon.weapon.rate / 60f);
                if (playerUI != null) playerUI.updateWeapon();
            }
        }

        // Hide weapon
        public void hideWeapon(WeaponController weapon) {
            if (weapon != null) {
                weapon.hideWeapon();
            }
        }

        // Is weapon currenlty held in hands
        public bool isWeaponActive(WeaponController weapon) {
            if (weapon != null && weapon.gameObject.activeInHierarchy) {
                return true;
            }
            else return false;
        }

        public bool isHero() {
            if (player is Hero) return true;
            else return false;
        }

        public WeaponController activeWeapon() {
            if (inventory.primaryWeapon.gameObject.activeInHierarchy) return inventory.primaryWeapon;
            else if (inventory.secondaryWeapon.gameObject.activeInHierarchy) return inventory.secondaryWeapon;
            else return null;
        }

        public ItemController firstItem() {
            return inventory.firstItem;
        }

        public ItemController secondItem() {
            return inventory.secondItem;
        }

        // Receive damage
        public DamageResult receiveDamage(float damage, bool ignoreShield) {
            if (player.isAlive) {
                if (OnDamageReceived != null) {
                    OnDamageReceived();
                }
                DamageResult result = DamageResult.NONE;
                if (player is Hero && ignoreShield) {
                   result = (player as Hero).receiveDamageIgnoreShield(damage);
                }
                else {
                   result = player.receiveDamage(damage);
                }
                if (result == DamageResult.MORTAL_HIT) {
                    mecanim.SetTrigger(State.DIE);
                }
                return result;
            }
            else return DamageResult.NONE;
        }

        // Apply effect
        public void applyEffect(EffectData effect) {
            switch (effect.type) {
                case EffectType.FIRE:
                    CharFireEffect fire = gameObject.AddComponent<CharFireEffect>();
                    fire.effect = effect;
                    break;
                case EffectType.POISON:
                    CharPoisonEffect poison = gameObject.AddComponent<CharPoisonEffect>();
                    poison.effect = effect;
                    break;
                case EffectType.SLOWDOWN:
                    CharSlowdownEffect slow = gameObject.AddComponent<CharSlowdownEffect>();
                    slow.effect = effect;
                    break;
                case EffectType.STUN:
                    CharStunEffect stun = gameObject.AddComponent<CharStunEffect>();
                    stun.effect = effect;
                    break;
                default:
                    break;
            }
        }

        public void setPlayer(Player player) {
            this.player = player;
            if (LevelController.Instance != null) playerUI = LevelController.Instance.HUD.findUIforPlayer(this).GetComponent<PlayerUIController>();
            // If player is a hero
            if (player is Hero) {
                // Set it as camera target
                if (LevelController.Instance != null) LevelController.Instance.view.setTarget(gameObject);
                // Give him a shield
                GameObject shieldInstance = Instantiate(Data.Instance.mainSettings.hero.shieldPrefab) as GameObject;
                shieldInstance.transform.position = transform.position;
                shieldInstance.transform.SetParent(transform);
                shield = shieldInstance.GetComponent<Shield>();
                shield.hero = (Hero) player;
                OnDamageReceived += shield.refreshTimer;
                // Create heads up display for hero
                GameObject headsUp = Instantiate(Data.Instance.mainSettings.hero.headsUpDisplayPrefab) as GameObject;
                headsUp.transform.SetParent(transform);
                headsUp.transform.position = transform.position;
            }
            else {
                // Create heads up display for guard
                GameObject headsUp = Instantiate(Data.Instance.mainSettings.guard.headsUpDisplayPrefab) as GameObject;
                headsUp.transform.SetParent(transform);
                headsUp.transform.position = transform.position;
            }
            // Set texture
            mainRender.material.SetTexture("_MainTex", player.character.texture);
            // -----------
            gamePadControl = (player.inputType == InputType.GAMEPAD);
            gamePadNumber = player.gamepadNumber;
            // Start tracing coroutine
            StartCoroutine(defineVisibility());
        }

        public void setUI(PlayerUIController ui) {
            playerUI = ui;
        }

        public void Death() {
            // Stop tracing coroutine
            StopCoroutine(defineVisibility());
            player.die();
            if (OnDie != null) {
                OnDie(player);
            }
            //Destroy(gameObject);
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
            if (primaryUp != null) OnPrimaryUp -= primaryUp;
        }

        // Reading weapon input
        private void readInput() {
            bool primaryDown, primaryHold, primaryUp;
            bool secondaryDown, secondaryHold, secondaryUp;
            switch (player.inputType) {
                case InputType.KEYBOARD:
                    // For processing
                    usePrimaryWeapon = Input.GetButton(Control.LeftMouse);
                    useSecondaryWeapon = Input.GetButton(Control.RightMouse);
                    useFirstItem = Input.GetButtonUp(Control.Q);
                    useSecondItem = Input.GetButtonUp(Control.E);

                    // For firing events
                    primaryDown = Input.GetButtonDown(Control.LeftMouse);
                    primaryHold = usePrimaryWeapon;
                    primaryUp = Input.GetButtonUp(Control.LeftMouse);
                    secondaryDown = Input.GetButtonDown(Control.RightMouse);
                    secondaryHold = useSecondaryWeapon;
                    secondaryUp = Input.GetButtonUp(Control.RightMouse);
                    requestTeleport = Input.GetButtonDown(Control.LeftAlt);

                    break;
                case InputType.GAMEPAD:
                    float rightTrigger = Input.GetAxis(Utils.getControlForPlayer(Control.RightTrigger, player.gamepadNumber));
                    float leftTrigger = Input.GetAxis(Utils.getControlForPlayer(Control.LeftTrigger, player.gamepadNumber));

                    primaryDown = (!usePrimaryWeapon && (rightTrigger > 0f));
                    primaryHold = (rightTrigger > 0f);
                    primaryUp = (usePrimaryWeapon && (rightTrigger <= 0.1f));
                    secondaryDown = (!useSecondaryWeapon && (leftTrigger > 0f));
                    secondaryHold = (leftTrigger > 0f);
                    secondaryUp = (useSecondaryWeapon && (leftTrigger <= 0.1f));

                    usePrimaryWeapon = primaryHold;
                    useSecondaryWeapon = secondaryHold;
                    useFirstItem = Input.GetButtonUp(Utils.getControlForPlayer(Control.LeftBumper, player.gamepadNumber));
                    useSecondItem = Input.GetButtonUp(Utils.getControlForPlayer(Control.RightBumper, player.gamepadNumber));
                    requestTeleport = Input.GetButtonDown(Utils.getControlForPlayer(Control.X, player.gamepadNumber));

                    break;
                default:
                    usePrimaryWeapon = false;
                    useSecondaryWeapon = false;
                    useFirstItem = false;
                    useSecondItem = false;
                    primaryDown = false;
                    primaryHold = false;
                    primaryUp = false;
                    secondaryDown = false;
                    secondaryHold = false;
                    secondaryUp = false;
                    requestTeleport = false;
                    break;
            }
            // Firing freakin events
            if (primaryDown && OnPrimaryDown != null) OnPrimaryDown();
            if (primaryHold && OnPrimaryHold != null) OnPrimaryHold();
            if (primaryUp && OnPrimaryUp != null) OnPrimaryUp();
            if (secondaryDown && OnSecondaryDown != null) OnSecondaryDown();
            if (secondaryHold && OnSecondaryHold != null) OnSecondaryHold();
            if (secondaryUp && OnSecondaryUp != null) OnSecondaryUp();
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
                if (isHero()) {
                    shield.Charge();
                    if (OnSecondaryUp == null) {
                        secondaryUp = delegate () {
                            shield.Exile();
                            OnSecondaryUp -= secondaryUp;
                        };
                        OnSecondaryUp += secondaryUp;
                    }
                }
                else {
                    if (isWeaponActive(inventory.secondaryWeapon)) {
                        mecanim.SetBool(State.STEADY, true);
                        if (inventory.secondaryWeapon.canUseWeapon()) mecanim.SetBool(State.TRIGGER, true);
                        else mecanim.SetBool(State.TRIGGER, false);
                    }
                    else {
                        drawSecondary();
                    }
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
                if (aimMode) disableAimMode();
                else tryUseItem(inventory.firstItem);
            }
            else if (useSecondItem) {
                if (aimMode) disableAimMode();
                else tryUseItem(inventory.secondItem);
            }
        }

        private void tryUseItem(ItemController item) {
            ItemStatus status = item.checkStatus();
            switch (status) {
                case ItemStatus.ACTIVE_READY:
                    useItem(item);
                    break;
                case ItemStatus.NEED_AIM:
                    Debug.Log(item.item.name + " needs aiming");
                    enableAimMode();
                    primaryUp = delegate () {
                        disableAimMode();
                        useItem(item);
                    };
                    OnPrimaryUp += primaryUp;
                    break;
                case ItemStatus.COOLDOWN:
                    Debug.Log(item.item.name + " is cooling down");
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
            item.Activate();
            if (primaryUp != null) OnPrimaryUp -= primaryUp;
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
                    rotation = new Vector3(x, 0f, z);
                    break;
                case InputType.KEYBOARD:
                    Ray camRay = Camera.main.ScreenPointToRay(Input.mousePosition);
                    RaycastHit floorHit;
                    if (Physics.Raycast(camRay, out floorHit, 100f, LayerMask.GetMask(Layers.Floor))) {
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

        public IEnumerator defineVisibility() {
            while (true) {
                yield return new WaitForSeconds(5f);
                Plane[] planes = GeometryUtility.CalculateFrustumPlanes(Camera.main);
                bool visible = GeometryUtility.TestPlanesAABB(planes, GetComponent<Collider>().bounds);
                if (visible) stopTracing();
                else startTracing();
            }
        }

        public void startTracing() {
            if (player is Guard && playerUI != null) playerUI.startTracing();
        }

        public void stopTracing() {
            if (playerUI != null) playerUI.stopTracing();
        }
    }
}
