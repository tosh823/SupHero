using UnityEngine;
using System.Collections;
using SupHero;
using SupHero.Model;

namespace SupHero.Components {

    public struct Area {
        public Vector3 topLeft;
        public Vector3 topRight;
        public Vector3 botRight;
        public Vector3 botLeft;
    }

    public class CameraController : MonoBehaviour {

        public GameObject mark;
        public Vector3 center;

        private LevelController levelController;
        private Camera cameraComponent;
        private GameObject target;
        private Vector3 offset;
        private float minDistance;
        private int surfaceMask;
        private float smoothing = 0.8f;
        
        // Use this for initialization
        void Start() {
            levelController = GetComponentInParent<LevelController>();
            cameraComponent = GetComponent<Camera>();
            surfaceMask = LayerMask.GetMask(Layers.Floor);
            minDistance = Constants.heroDistance;
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
            Ray ray = cameraComponent.ViewportPointToRay(new Vector3(0.5f, 0.5f, 0));
            if (Physics.Raycast(ray, out hit, surfaceMask)) {
                Vector3 hitPoint = hit.point;
                hitPoint.y = 0f;
                center = hitPoint;
                center.z -= 5f;
                center.x += 2f;
                offset = transform.position - center;
                mark.transform.position = center;
            }
        }

        public Area getVisibleArea() {
            Area visibleArea = new Area();
            RaycastHit hit;
            Ray rayTopLeft = cameraComponent.ViewportPointToRay(new Vector3(0f, 0f, 0f));
            if (Physics.Raycast(rayTopLeft, out hit, surfaceMask)) {
                Vector3 hitPoint = hit.point;
                //hitPoint.y = 0f;
                visibleArea.topLeft = hitPoint;
            }
            Ray rayTopRight = cameraComponent.ViewportPointToRay(new Vector3(1f, 0f, 0f));
            if (Physics.Raycast(rayTopRight, out hit, surfaceMask)) {
                Vector3 hitPoint = hit.point;
                //hitPoint.y = 0f;
                visibleArea.topRight = hitPoint;
            }
            Ray rayBotRight = cameraComponent.ViewportPointToRay(new Vector3(1f, 1f, 0f));
            if (Physics.Raycast(rayBotRight, out hit, surfaceMask)) {
                Vector3 hitPoint = hit.point;
                //hitPoint.y = 0f;
                visibleArea.botRight = hitPoint;
            }
            Ray rayBotLeft = cameraComponent.ViewportPointToRay(new Vector3(0f, 1f, 0f));
            if (Physics.Raycast(rayBotLeft, out hit, surfaceMask)) {
                Vector3 hitPoint = hit.point;
                //hitPoint.y = 0f;
                visibleArea.botLeft = hitPoint;
            }
            return visibleArea;
        }

        public void setTarget(GameObject target) {
            this.target = target;
            defineCenter();
        }
    }
}
