using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Com.Itronics.Highlife { 
public class Crouch : MonoBehaviour
{
    public CharacterController characterController;
    public Transform topDetector;
    public float crouchHeight;
    public float standHeight;
    public bool isBlocked;
    public KeyCode crouchKey;

    void Start()
    {
            characterController = gameObject.GetComponent<CharacterController>();
    }

    void Update()
    {
            CheckCrouching();
    }

        void CheckCrouching()
        {
            isBlocked = Physics.SphereCast(topDetector.position, characterController.radius, Vector3.up, out var hit, standHeight - characterController.radius);
            if (!characterController.isGrounded) return;

            if (Input.GetKey(crouchKey))
            {
                characterController.height = crouchHeight;
            }
            else {
                if(!isBlocked) { 
                    characterController.height = standHeight;
                }
            }
        }
    }
}