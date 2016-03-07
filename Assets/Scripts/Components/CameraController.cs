using UnityEngine;
using System.Collections;
using SupHero;
using SupHero.Model;

namespace SupHero.Controllers {

    public class CameraController : MonoBehaviour {

        public GameObject mark;

        private LevelController levelController;
        private Camera cameraComponent;
        private GameObject target;
        private Vector3 offset;
        private Vector3 center;
        private float minDistance;
        private int surfaceMask;
        private float smoothing = 5f;
        
        // Use this for initialization
        void Start() {
            levelController = GetComponentInParent<LevelController>();
            cameraComponent = GetComponent<Camera>();
            surfaceMask = LayerMask.GetMask("Surface");
            minDistance = 8f;
        }

        void FixedUpdate() {
            if (target != null) {

                Vector3 distance = target.transform.position - center;

                if (distance.magnitude >= minDistance) {
                    // Create a postion the camera is aiming for based on the offset from the target.
                    Vector3 moveVector = target.transform.position + offset;
                    // Smoothly interpolate between the camera's current position and it's target position.
                    transform.position = Vector3.Lerp(transform.position, moveVector, smoothing * Time.deltaTime);
                    defineCenter();
                }
            }
        }

        private void defineCenter() {
            RaycastHit hit;
            Ray ray = cameraComponent.ScreenPointToRay(new Vector3(Screen.width / 2, Screen.height / 2, cameraComponent.nearClipPlane));
            if (Physics.Raycast(ray, out hit, surfaceMask)) {
                Vector3 hitPoint = hit.point;
                hitPoint.y = 1f;
                mark.transform.position = hitPoint;
                center = hitPoint;
                offset = transform.position - center;
            }
        }


        public void setTarget(GameObject target) {
            this.target = target;
            defineCenter();
        }
    }
}
