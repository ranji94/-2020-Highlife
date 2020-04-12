using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Com.Itronics.Highlife
{
    public class Footsteps : MonoBehaviour
    {
        public AudioSource[] footstepSounds;
        public CharacterController characterController;
        public float crouchHeight;

        private void Start()
        {
            footstepSounds = GetComponent<AudioSource[]>();
            characterController = GetComponent<CharacterController>();
            foreach (AudioSource sound in footstepSounds) {
                sound.Play(0);
            }
            Debug.Log("Started");
        }

        void Update()
        {
            int element = Random.Range(0, footstepSounds.Length);

            if (!isAnySoundPlaying(footstepSounds)
                && isPlayerMoving()
                && characterController.isGrounded
                && !isCrouching())
            {
                footstepSounds[element].volume = Random.Range(0.8f, 1f);
                footstepSounds[element].pitch = Random.Range(0.95f, 1f);
                footstepSounds[element].Play();
            }
        }

        private static bool isAnySoundPlaying(AudioSource[] footstepSounds) {
            foreach (AudioSource sound in footstepSounds) {
                if (sound.isPlaying) {
                    return true;
                }
            }
            return false;
        }

        private static bool isPlayerMoving()
        {
            float tHmove = Input.GetAxisRaw("Horizontal");
            float tVmove = Input.GetAxisRaw("Vertical");

            return !(tVmove == 0 && tHmove == 0);
        }

        private bool isCrouching()
        {
            return characterController.height == crouchHeight;
        }
    }
}