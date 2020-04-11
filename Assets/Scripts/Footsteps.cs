using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Com.Itronics.Highlife
{
    public class Footsteps : MonoBehaviour
    {
        public AudioSource[] footstepSounds;
        public LayerMask ground;
        public Transform groundDetector;

        private void Start()
        {
            footstepSounds = GetComponent<AudioSource[]>();
            foreach (AudioSource sound in footstepSounds) {
                sound.Play(0);
            }
            Debug.Log("Started");
        }

        void Update()
        {
            bool isGrounded = Physics.Raycast(groundDetector.position, Vector3.down, 0.1f, ground);
            int element = Random.Range(0, footstepSounds.Length);

            if (!isAnySoundPlaying(footstepSounds)
                && isPlayerMoving()
                && isGrounded)
            {
                footstepSounds[element].volume = Random.Range(0.8f, 1f);
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
    }
}