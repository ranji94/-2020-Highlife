using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Com.Itronics.Highlife { 
    public class Colt : MonoBehaviour
    {
        public Animator anim;
        public AudioSource fireSound;
        public float attackDamage;
        public float attackRange;
        public float impactForce;
        public float fireRate;
        public ParticleSystem muzzleFlash;
        public Camera weaponCam;
        public GameObject impactEffect;

        private float nextTimeToFire = 0f;

        void Start()
        {
            anim = GetComponent<Animator>();
            fireSound.Play(0);
        }

        void Update()
        {
            anim.SetBool("Walking", isPlayerMoving());

            if (Input.GetMouseButton(0) 
                && Time.time >= nextTimeToFire) {
                nextTimeToFire = Time.time + 1f / fireRate;
                Shoot();
            }
        }

        private void FixedUpdate()
        {
            if (isFiring()) {
                anim.SetBool("Fire", false);
            }
        }

        private bool isFiring()
        {
            AnimatorStateInfo info = anim.GetCurrentAnimatorStateInfo(anim.GetLayerIndex("Base Layer"));
            return info.IsName("Fire");
        }

        private void Shoot() {
            RaycastHit hit;
            Ray ray = weaponCam.ScreenPointToRay(Input.mousePosition);

            playColtFireSound();
            muzzleFlash.Play();
            anim.SetBool("Fire", true);

            //if (Physics.Raycast(weaponCam.transform.position, weaponCam.transform.forward, out hit, attackRange)) {
            if (Physics.Raycast(ray, out hit, attackRange)) { 
                if (hit.collider.tag == "Crate")
                {
                    Crate crate = hit.collider.GetComponent<Crate>();
                    crate.takeDamage(attackDamage);
                }
            }

            if (hit.rigidbody != null) {
                hit.rigidbody.AddForce(-hit.normal * impactForce);
            }

            GameObject impactGameObject = Instantiate(impactEffect, hit.point, Quaternion.LookRotation(hit.normal));
            Destroy(impactGameObject, 2f);
        }

        private static bool isPlayerMoving()
        {
            float tHmove = Input.GetAxisRaw("Horizontal");
            float tVmove = Input.GetAxisRaw("Vertical");

            return !(tVmove == 0 && tHmove == 0);
        }

        private void playColtFireSound()
        {
            fireSound.volume = 1f;
            fireSound.Play();
        }
    }
}