using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Com.Itronics.Highlife {
    public class FlashLight : MonoBehaviour
    {
        public Light flashLight;
        public AudioSource flashlightSound;
        public KeyCode lightKey;
        void Start()
        {
            flashLight.enabled = false;
            flashlightSound.Play(0);
        }

        void Update()
        {
            if (Input.GetKeyDown(lightKey))
            {
                if (!flashLight.enabled)
                {
                    StartCoroutine(enableFlashlight(0.1f));
                }
                else {
                    StartCoroutine(disableFlashlight(0.1f));
                }
            }

        }

        private IEnumerator enableFlashlight(float time)
        {
            yield return new WaitForSeconds(time);
            playFlashlightSound();
            flashLight.enabled = true;
        }

        private IEnumerator disableFlashlight(float time)
        {
            yield return new WaitForSeconds(time);
            playFlashlightSound();
            flashLight.enabled = false;
        }

        private void playFlashlightSound() {
            if (!flashlightSound.isPlaying)
            {
                flashlightSound.volume = 1f;
                flashlightSound.Play();
            }
        } 
    }
}
