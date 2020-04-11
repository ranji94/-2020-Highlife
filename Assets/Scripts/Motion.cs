using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Com.Itronics.Highlife { 
    public class Motion : MonoBehaviour
    {
        public float speed;
        public float retarder;
        public float sprintModifier;
        private float baseFov;
        private float fovModifierIterated;
        private float sprintFovModifier = 1.1f;
        public Camera normalCam;
        private Rigidbody rig;

        private void Start() {
            baseFov = normalCam.fieldOfView;
            fovModifierIterated = 1f;
            rig = GetComponent<Rigidbody>();
        }

        void FixedUpdate() {
            float tHmove = Input.GetAxisRaw("Horizontal");
            float tVmove = Input.GetAxisRaw("Vertical");
            bool isSprinting = (Input.GetKey(KeyCode.LeftShift) 
                || Input.GetKey(KeyCode.RightShift)) 
                && tVmove > 0;
            bool isJumping = Input.GetKey(KeyCode.Space);

            Vector3 tDirection = new Vector3(tHmove, 0, tVmove);
            Vector3 jump = new Vector3(0, 10, 0);
            tDirection.Normalize();

            float tAdjustSpeed = speed;
            if (isSprinting)
            {
                tAdjustSpeed *= sprintModifier;
                normalCam.fieldOfView = baseFov * sprintFovModifier;
            }
            else {
                normalCam.fieldOfView = baseFov;
            }

            if (isJumping) {
                rig.velocity = transform.TransformDirection(jump);
            }

            if (isPlayerMoving())
            {
                if (retarder < 1f) {
                    retarder += 0.2f;
                }
                rig.velocity = transform.TransformDirection(tDirection)
                    * tAdjustSpeed
                    * retarder
                    * Time.deltaTime;
            }
            else
            {
                if (retarder > 0.02f) {
                    retarder -= 0.02f;
                }

                if (retarder < 0.35)
                {
                    rig.velocity = transform.TransformDirection(tDirection)
                    * tAdjustSpeed
                    * retarder
                    * Time.deltaTime;
                }
            }
        }

        private static bool isPlayerMoving() {
            float tHmove = Input.GetAxisRaw("Horizontal");
            float tVmove = Input.GetAxisRaw("Vertical");

            return !(tVmove == 0 && tHmove == 0);
        }
    }
}   