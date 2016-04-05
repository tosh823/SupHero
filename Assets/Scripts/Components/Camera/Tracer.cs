using UnityEngine;
using System.Collections;


namespace SupHero.Components {
    public class Tracer : MonoBehaviour {

        public Transform target;
        public float smoothing = 5f;

        private Camera cameraComponent;
        private Vector3 cameraOffset;

        void Start() {
            cameraComponent = GetComponent<Camera>();
            transform.LookAt(target.position);
            cameraOffset = transform.position - target.position;
        }
        
        void FixedUpdate() {
            Vector3 targetCamPos = target.position + cameraOffset;
            transform.position = Vector3.Lerp(transform.position, targetCamPos, smoothing * Time.deltaTime);
        }
    }
}
