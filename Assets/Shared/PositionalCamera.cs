using System.Collections;
using UnityEngine;

namespace Shared {
    public class PositionalCamera : MonoBehaviour {
        public Camera[] fixedCameras;
        public Camera movingCamera;

        private void Awake() {
            movingCamera.enabled = true;
            foreach (var fixedCamera in fixedCameras) {
                fixedCamera.enabled = false;
            }
        }

        private IEnumerator Merge(Camera destination) {
            const int steps = 20;

            var transform1 = movingCamera.transform;
            var transform2 = destination.transform;

            var initRotation = transform1.rotation.eulerAngles;

            var deltaFov = (destination.fieldOfView - movingCamera.fieldOfView) / steps;
            var deltaPosition = (transform2.localPosition - transform1.localPosition) / steps;
            var deltaRotation = (transform2.rotation.eulerAngles - initRotation) / steps;

            for (var i = 0; i < steps; i++) {
                movingCamera.fieldOfView += deltaFov;
                transform1.position += deltaPosition;
                movingCamera.transform.rotation = Quaternion.Euler(initRotation + deltaRotation * i);
                yield return null;
            }
        }

        public void MoveToCamera(int cameraIndex) {
            if (cameraIndex >= fixedCameras.Length) {
                Debug.LogWarning($"{cameraIndex} is greater than {fixedCameras.Length}");
                return;
            }

            StartCoroutine(Merge(fixedCameras[cameraIndex]));
        }

        private void Update() {
            if (Input.GetKeyDown("0")) MoveToCamera(0);
            if (Input.GetKeyDown("1")) MoveToCamera(1);
            if (Input.GetKeyDown("2")) MoveToCamera(2);
            if (Input.GetKeyDown("l")) Debug.Log("dsadasd");
        }
    }
}