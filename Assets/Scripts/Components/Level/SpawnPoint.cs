using UnityEngine;
using System.Collections;
using SupHero.Components.Character;
using SupHero.Model;

namespace SupHero.Components.Level {
    public class SpawnPoint : MonoBehaviour {

        public Player pilot;
        private Animator mecanim;
        
        void Start() {
            mecanim = GetComponent<Animator>();
            mecanim.SetTrigger("activate");
        }

        void Update() {
            if (pilot != null) {
                Vector3 moveVector = getMovement();
                if (moveVector != Vector3.zero) {
                    // Relative movement
                    Vector3 forward = Camera.main.transform.TransformDirection(Vector3.forward);
                    forward.y = 0f;
                    forward = forward.normalized;
                    Vector3 right = new Vector3(forward.z, 0f, -forward.x);
                    moveVector = (moveVector.x * right + moveVector.z * forward);
                    // Finally, moving!
                    moveVector = moveVector.normalized * pilot.speed * Time.deltaTime;
                    transform.position += moveVector;
                }
            }
        }

        private Vector3 getMovement() {
            Vector3 movement = Vector3.zero;
            float h, v;
            switch (pilot.inputType) {
                case InputType.KEYBOARD:
                    h = Input.GetAxis(Control.Horizontal);
                    v = Input.GetAxis(Control.Vertical);
                    movement = new Vector3(h, 0f, v);
                    break;
                case InputType.GAMEPAD:
                    h = Input.GetAxis(Utils.getControlForPlayer(Control.LeftStickX, pilot.gamepadNumber));
                    v = Input.GetAxis(Utils.getControlForPlayer(Control.LeftStickY, pilot.gamepadNumber));
                    movement = new Vector3(h, 0f, v);
                    break;
                default:
                    break;
            }
            return movement;
        }
    }
}