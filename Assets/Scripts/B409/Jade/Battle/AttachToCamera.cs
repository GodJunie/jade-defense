using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace B409.Jade.Battle {
    public class AttachToCamera : MonoBehaviour {
        private Transform cameraTransform;
        private Vector3 offset;

        private void Awake() {
            cameraTransform = Camera.main.transform;
            offset = transform.position - cameraTransform.position;
        }

        private void LateUpdate() {
            if(cameraTransform != null) {
                transform.position = cameraTransform.position + offset;
            }
        }
    }
}