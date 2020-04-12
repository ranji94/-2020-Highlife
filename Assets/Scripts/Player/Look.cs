using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Com.Itronics.Highlife
{
    public class Look : MonoBehaviour
    {
        public Transform player;
        public Transform cams;

        public static bool cursorLocked;

        public float xSensitivity;
        public float ySensitivity;
        public float maxAngle;

        private Quaternion camCenter;

        private void Start() {
            camCenter = cams.localRotation;
        }

        void Update() {
            SetY();
            SetX();

            UpdateCursorLock();
        }

        void SetY() {
            float tInput = Input.GetAxis("Mouse Y") * ySensitivity * Time.deltaTime;
            Quaternion tAdj = Quaternion.AngleAxis(tInput, -Vector3.right);
            Quaternion tDelta = cams.localRotation * tAdj;

            if (Quaternion.Angle(camCenter, tDelta) < maxAngle) {
                cams.localRotation = tDelta;
            }
        }

        void SetX()
        {
            float tInput = Input.GetAxis("Mouse X") * xSensitivity * Time.deltaTime;
            Quaternion tAdj = Quaternion.AngleAxis(tInput, Vector3.up);
            Quaternion tDelta = player.localRotation * tAdj;
            player.localRotation = tDelta;
        }

        void UpdateCursorLock() {
            if (cursorLocked)
            {
                Cursor.lockState = CursorLockMode.Locked;
                Cursor.visible = false;

                if (Input.GetKeyDown(KeyCode.Escape)) {
                    cursorLocked = false;
                }
            }
            else {
                Cursor.lockState = CursorLockMode.None;
                Cursor.visible = true;

                if (Input.GetKeyDown(KeyCode.Escape))
                {
                    cursorLocked = true;
                }
            }
        }
    }
}