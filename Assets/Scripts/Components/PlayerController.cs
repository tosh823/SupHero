using UnityEngine;
using System.Collections;
using SupHero;
using SupHero.Model;

namespace SupHero.Controllers {
    public class PlayerController : MonoBehaviour {

        public Player player;
        public GameObject weapon;

        private Rigidbody playerRigidbody;

        // Use this for initialization
        void Start() {
            // For standalone test init
            if (player == null) {
                player = new Hero(1);
                player.inputType = InputType.KEYBOARD;
            }
            playerRigidbody = GetComponent<Rigidbody>();
        }

        // Update is called once per frame
        void Update() {

        }

        void FixedUpdate() {
            // Moving
            Vector3 moveVector = getMovementVector();
            if (moveVector != Vector3.zero) {
                moveVector = moveVector.normalized * player.speed * Time.deltaTime;
                playerRigidbody.MovePosition(transform.position + moveVector);
            }
            // Turning
            Quaternion rotation = getRotation();
            playerRigidbody.MoveRotation(rotation);
            // Actions
            getActions();
        }

        public void setPlayer(Player player) {
            this.player = player;
        }

        public void killSelf() {
            Destroy(gameObject);
        }

        private void getActions() {
            bool useWeapon = false;
            switch (player.inputType) {
                case InputType.KEYBOARD:
                    useWeapon = Input.GetButton("Fire1");
                    break;
                case InputType.GAMEPAD:
                    float rightBumper = Input.GetAxis(Utils.getControlForPlayer("R2", player.gamepadNumber));
                    useWeapon = (rightBumper > 0f);
                    break;
                default:
                    break;
            }
            if (useWeapon) weapon.GetComponent<WeaponController>().useWeapon();
        }

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

        private Quaternion getRotation() {
            Quaternion rotation = transform.rotation;
            switch (player.inputType) {
                case InputType.GAMEPAD:
                    float x = Input.GetAxis(Utils.getControlForPlayer("RightStickX", player.gamepadNumber));
                    float z = Input.GetAxis(Utils.getControlForPlayer("RightStickY", player.gamepadNumber));
                    if (x != 0f && z != 0f) {
                        rotation = Quaternion.LookRotation(new Vector3(x, 0f, z) * Time.deltaTime);
                    }
                    break;
                case InputType.KEYBOARD:
                    Ray camRay = Camera.main.ScreenPointToRay(Input.mousePosition);
                    RaycastHit floorHit;
                    if (Physics.Raycast(camRay, out floorHit)) {
                        Vector3 playerToMouse = floorHit.point - transform.position;
                        playerToMouse.y = 0f;
                        rotation = Quaternion.LookRotation(playerToMouse * Time.deltaTime);
                    }
                    break;
                default:
                    break;
            }
            return rotation;
        }
    }
}
