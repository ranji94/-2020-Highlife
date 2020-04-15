using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace Com.Itronics.Highlife { 
    public class Deagle : MonoBehaviour
    {
        public Animator anim;
        public AudioSource fireSound;
        public AudioSource reloadSound;
        public AudioSource dryFireSound;
        public float attackDamage;
        public float attackRange;
        public int ammoInClip;
        public int totalAmmo;
        public float impactForce;
        public float fireRate;
        public ParticleSystem muzzleFlash;
        public Camera weaponCam;
        public GameObject impactEffect;
        public Text ammoText;

        private float nextTimeToFire = 0f;
        private int startAmmoInClip;

        void Start()
        {
            startAmmoInClip = ammoInClip;
            anim = GetComponent<Animator>();
        }

        void Update()
        {
            anim.SetBool("Walking", isPlayerMoving());
            ammoTextRefresh();

            if (Input.GetMouseButton(0) 
                && Time.time >= nextTimeToFire) {
                nextTimeToFire = Time.time + 1f / fireRate;
                if (!isReloading()
                    && ammoInClip > 0)
                {
                    Shoot();
                }
                else {
                    Reload();
                }
            } 
        }

        private void FixedUpdate()
        {
            if (isFiring()) {
                anim.SetBool("Fire", false);
            }

            if (isReloading()) {
                anim.SetBool("Reload", false);
            }

            if (Input.GetKeyDown(KeyCode.R))
            {
                Reload();
            }
        }

        private void Reload() {
            if (totalAmmo > 0
                && ammoInClip != startAmmoInClip
                && !isReloading())
            {
                anim.SetBool("Reload", true);
                playDeagleReloadSound();
                totalAmmo -= totalAmmo > ammoInClip ? startAmmoInClip - ammoInClip : totalAmmo;
                ammoInClip = startAmmoInClip;
            }
            else {
                playDryFireSound();
            }
        }

        private bool isFiring()
        {
            AnimatorStateInfo info = anim.GetCurrentAnimatorStateInfo(anim.GetLayerIndex("Base Layer"));
            return info.IsName("Fire");
        }

        private bool isReloading() {
            AnimatorStateInfo info = anim.GetCurrentAnimatorStateInfo(anim.GetLayerIndex("Base Layer"));
            return info.IsName("Reload");
        }

        private void Shoot() {
            RaycastHit hit;
            Ray ray = weaponCam.ScreenPointToRay(Input.mousePosition);

            playDeagleFireSound();
            muzzleFlash.Play();
            anim.SetBool("Fire", true);

            if (Physics.Raycast(weaponCam.transform.position, weaponCam.transform.forward, out hit, attackRange)) {
            //if (Physics.Raycast(ray, out hit, attackRange)) { 
                if (hit.collider.tag == "Crate")
                {
                    Crate crate = hit.collider.GetComponent<Crate>();
                    crate.takeDamage(attackDamage);
                }
            }

            if (hit.rigidbody != null) {
                hit.rigidbody.AddForce(-hit.normal * impactForce);
            }

            ammoInClip--;
            GameObject impactGameObject = Instantiate(impactEffect, hit.point, Quaternion.LookRotation(hit.normal));
            Destroy(impactGameObject, 2f);
        }

        private static bool isPlayerMoving()
        {
            float tHmove = Input.GetAxisRaw("Horizontal");
            float tVmove = Input.GetAxisRaw("Vertical");

            return !(tVmove == 0 && tHmove == 0);
        }

        private void playDeagleFireSound()
        {
            fireSound.volume = 1f;
            fireSound.Play();
        }

        private void playDeagleReloadSound()
        {
            if(!isReloading()) { 
                reloadSound.volume = 1f;
                reloadSound.Play();
            }
        }

        private void playDryFireSound()
        {
            if (!dryFireSound.isPlaying)
            {
                dryFireSound.volume = 1f;
                dryFireSound.Play();
            }
        }

        public void ammoTextRefresh() {
            ammoText.GetComponent<Text>().text = ammoInClip.ToString() + "/" + totalAmmo.ToString();
        }
    }
}