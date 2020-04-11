using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Com.Itronics.Highlife { 
    public class Motion : MonoBehaviour
    {
        public float speed;
        public float retarder;
        public float jumpForce;
        public float sprintModifier;
        public LayerMask ground;
        public Transform groundDetector;
        private float baseFov;
        private float sprintFovModifier = 1.1f;
        public Camera normalCam;
        private Rigidbody rig;

        private void Start() {
            baseFov = normalCam.fieldOfView;
            rig = GetComponent<Rigidbody>();
        }

        void FixedUpdate() {
            float tHmove = Input.GetAxisRaw("Horizontal");
            float tVmove = Input.GetAxisRaw("Vertical");
            bool isSprinting = (Input.GetKey(KeyCode.LeftShift) 
                || Input.GetKey(KeyCode.RightShift)) 
                && tVmove > 0;
            bool isJumping = Input.GetKey(KeyCode.Space) && isGrounded(groundDetector, ground);
            
            Vector3 tDirection = new Vector3(tHmove, 0, tVmove);
            tDirection.Normalize();

            float tAdjustSpeed = speed;
            if (isSprinting)
            {
                tAdjustSpeed *= sprintModifier;
                normalCam.fieldOfView = baseFov * sprintFovModifier;
            }
            else {
                rig.AddForce(new Vector3(-tHmove, 0, -tVmove) * 30);
                normalCam.fieldOfView = baseFov;
            }

            if (isJumping) {
                rig.AddForce(Vector3.up * jumpForce);
            }

            if (isPlayerMoving())
            {
                if (retarder < 1f) {
                    retarder += 0.2f;
                }

                Vector3 tTargetVelocity = transform.TransformDirection(tDirection)
                    * tAdjustSpeed
                    //* retarder
                    * (isGrounded(groundDetector, ground) ? 1f : 0.6f)
                    * Time.deltaTime;
                tTargetVelocity.y = rig.velocity.y;
                rig.velocity = tTargetVelocity;
            }
            else
            {
                if (retarder > 0.02f) {
                    retarder -= 0.02f;
                }
            }
        }

        private static bool isPlayerMoving() {
            float tHmove = Input.GetAxisRaw("Horizontal");
            float tVmove = Input.GetAxisRaw("Vertical");

            return !(tVmove == 0 && tHmove == 0);
        }

        private static bool isGrounded(Transform groundDetector, LayerMask ground) {
            return Physics.Raycast(groundDetector.position, Vector3.down, 0.1f, ground);
        }
    }
}   