﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Com.Itronics.Highlife {
    public class Crowbar : MonoBehaviour
    {
        public Animator anim;
        public AudioSource missSound;
        public Camera weaponCam;
        public float attackDamage;
        public float attackRange;

        void Start()
        {
            anim = GetComponent<Animator>();
            missSound.Play(0);
        }

        void Update()
        {
            if (Input.GetMouseButton(0) && !isFiring())
            { 
                Ray ray = weaponCam.ScreenPointToRay(Input.mousePosition);
                RaycastHit hit;

                if (Physics.Raycast(ray, out hit, attackRange))
                {
                    if (hit.collider.tag == "Crate")
                    {
                        Crate crate = hit.collider.GetComponent<Crate>();
                        StartCoroutine(takeDamage(0.2f, crate));
                    }  
                }
                else {
                    anim.SetBool("Fire", true);
                    playCrowbarMissSound();
                }
            } 
        }

        private void FixedUpdate()
        {
            if (isFiring())
            {
                anim.SetBool("Fire", false);
                anim.SetBool("Attack", false);
            }
        }

        private IEnumerator takeDamage(float time, Crate crate)
        {
            yield return new WaitForSeconds(time);

            if(!isFiring()) { 
                crate.takeDamage(attackDamage);
                playCrowbarHit(crate.hitSound);
                anim.SetBool("Attack", true);
            }
        }


        private bool isFiring()
        {
            AnimatorStateInfo info = anim.GetCurrentAnimatorStateInfo(anim.GetLayerIndex("Base Layer"));
            return info.IsName("Fire") || info.IsName("Attack");
        }

        private void playCrowbarMissSound()
        {
            if (!missSound.isPlaying)
            {
                missSound.volume = 1f;
                missSound.Play();
            }
        }

        private void playCrowbarHit(AudioSource hitSound)
        {
            hitSound.volume = 1f;
            hitSound.Play();
        }
    }
}