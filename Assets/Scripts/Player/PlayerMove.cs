using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Com.Itronics.Highlife { 
public class PlayerMove : MonoBehaviour
{
        [SerializeField] public string horizontalInputName;
        [SerializeField] public string verticalInputName;

        [SerializeField] public float walkSpeed, runSpeed;
        [SerializeField] public float runBuildUpSpeed;
        [SerializeField] public KeyCode runKey;

        private float movementSpeed;

        [SerializeField] public float slopeForce;
        [SerializeField] public float slopeForceRayLength;
        [SerializeField] private float afterJumpRetarder = 1f;
        [SerializeField] private float crouchRetarder = 0.3f;
        [SerializeField] public float crouchHeight;

        private CharacterController charController;

        [SerializeField] public AnimationCurve jumpFallOff;
        [SerializeField] public float jumpMultiplier;
        [SerializeField] public KeyCode jumpKey;
        public static bool isPlayerCrouching = false;
        public static bool isPlayerMoving = false;


        private bool isJumping;

        private void Awake()
        {
            charController = GetComponent<CharacterController>();
        }

        private void Update()
        {
            PlayerMovement();
            isPlayerCrouching = isCrouching();
            isPlayerMoving = isMoving();
        }

        private void PlayerMovement()
        {
            float horizInput = Input.GetAxis(horizontalInputName);
            float vertInput = Input.GetAxis(verticalInputName);

            Vector3 forwardMovement = transform.forward * vertInput;
            Vector3 rightMovement = transform.right * horizInput;


            charController.SimpleMove(Vector3.ClampMagnitude(forwardMovement + rightMovement, 1.0f) * movementSpeed);

            if ((vertInput != 0 || horizInput != 0) && OnSlope())
                charController.Move(Vector3.down * charController.height / 2 * slopeForce * Time.deltaTime);


            if (charController.isGrounded)
            {
                if (afterJumpRetarder < 1f)
                {
                    afterJumpRetarder += 0.04f;
                }
            }

            if (isJumping) {
                afterJumpRetarder = 0.10f;
            }

            SetMovementSpeed();
            JumpInput();
        }

        private void SetMovementSpeed()
        {
            if (Input.GetKey(runKey) && !isCrouching())
                movementSpeed = Mathf.Lerp(movementSpeed, 
                    runSpeed, 
                    Time.deltaTime * runBuildUpSpeed);
            else
                movementSpeed = Mathf.Lerp(movementSpeed, 
                    (isCrouching() ? crouchRetarder : 1f) 
                    * walkSpeed 
                    * afterJumpRetarder, 
                    Time.deltaTime * runBuildUpSpeed);
        }


        private bool OnSlope()
        {
            if (isJumping)
                return false;

            RaycastHit hit;

            if (Physics.Raycast(transform.position, Vector3.down, out hit, charController.height / 2 * slopeForceRayLength))
                if (hit.normal != Vector3.up)
                {
                    print("OnSlope");
                    return true;
                }

            return false;
        }

        private void JumpInput()
        {
            if (Input.GetKeyDown(jumpKey) 
                && !isJumping
                && !isCrouching())
            {
                isJumping = true;
                StartCoroutine(JumpEvent());
            }
        }


        private IEnumerator JumpEvent()
        {
            charController.slopeLimit = 90.0f;
            float timeInAir = 0.0f;
            do
            {
                float jumpForce = jumpFallOff.Evaluate(timeInAir);
                charController.Move(Vector3.up 
                    * jumpForce 
                    * jumpMultiplier 
                    * Time.deltaTime);
                timeInAir += Time.deltaTime;
                yield return null;
            } while (!charController.isGrounded && charController.collisionFlags != CollisionFlags.Above);

            charController.slopeLimit = 45.0f;
            isJumping = false;
        }

        private bool isCrouching()
        {
            return charController.height == crouchHeight;
        }

        private static bool isMoving()
        {
            float tHmove = Input.GetAxisRaw("Horizontal");
            float tVmove = Input.GetAxisRaw("Vertical");

            return !(tVmove == 0 && tHmove == 0);
        }
    }
}