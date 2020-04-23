using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace Com.Itronics.Highlife
{
    public class PistolSMG : MonoBehaviour
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
        public GameObject defaultImpactEffect;
        public Text ammoText;

        private float nextTimeToFire;
        private int startAmmoInClip;

        void Start()
        {
            startAmmoInClip = ammoInClip;
            anim = GetComponent<Animator>();
        }

        void Update()
        {
            ammoTextRefresh();
            anim.SetBool("Walking", PlayerMove.isPlayerMoving);
            if (PlayerMove.isPlayerCrouching)
            {
                anim.SetFloat("WalkingSpeedMultipler", 0.5f);
            }
            else
            {
                anim.SetFloat("WalkingSpeedMultipler", 1f);
            }

            if (Input.GetMouseButton(0)
                && Time.time >= nextTimeToFire
                && !isTaking())
            {
                nextTimeToFire = Time.time + 1f / fireRate;
                if (!isReloading()
                    && ammoInClip > 0)
                {
                    Shoot();
                }
                else
                {
                    Reload();
                }
            }
        }

        private void FixedUpdate()
        {
            if (isFiring())
            {
                anim.SetBool("Fire", false);
            }

            if (isReloading())
            {
                anim.SetBool("Reload", false);
            }

            if (Input.GetKeyDown(KeyCode.R))
            {
                Reload();
            }
        }

        private void Reload()
        {
            if (totalAmmo > 0
                && ammoInClip != startAmmoInClip
                && !isReloading())
            {
                anim.SetBool("Reload", true);
                playReloadSound();

                if (totalAmmo + ammoInClip < startAmmoInClip)
                {
                    ammoInClip = totalAmmo + ammoInClip;
                    totalAmmo = 0;
                }
                else
                {
                    totalAmmo -= startAmmoInClip - ammoInClip;
                    ammoInClip = startAmmoInClip;
                }
            }
            else
            {
                playDryFireSound();
            }
        }

        private bool isFiring()
        {
            AnimatorStateInfo info = anim.GetCurrentAnimatorStateInfo(anim.GetLayerIndex("Base Layer"));
            return info.IsName("Fire");
        }

        private bool isReloading()
        {
            AnimatorStateInfo info = anim.GetCurrentAnimatorStateInfo(anim.GetLayerIndex("Base Layer"));
            return info.IsName("Reload");
        }

        private bool isTaking()
        {
            AnimatorStateInfo info = anim.GetCurrentAnimatorStateInfo(anim.GetLayerIndex("Base Layer"));
            return info.IsName("TakeIn");
        }

        private void Shoot()
        {
            RaycastHit hit;
            GameObject impactEffect = defaultImpactEffect;
            Vector3 crosshairCorrection = new Vector3(0f, -0.1f, 0f);
            Vector3 recoil = new Vector3(Random.Range(-0.02f, 0.02f), Random.Range(-0.02f, 0.02f), 0f);

            playFireSound();
            muzzleFlash.Play();
            anim.SetBool("Fire", true);

            if (Physics.Raycast(weaponCam.transform.position,
                weaponCam.transform.forward
                    + crosshairCorrection
                    + recoil,
                out hit, attackRange))
            {
                if (hit.collider.tag == "Destructible")
                {
                    Destructible destructible = hit.collider.GetComponent<Destructible>();
                    destructible.takeDamage(attackDamage);
                    impactEffect = destructible.bulletImpactEffect;
                }
            }

            if (hit.rigidbody != null)
            {
                hit.rigidbody.AddForce(-hit.normal * impactForce);
            }

            ammoInClip--;
            GameObject impactGameObject = Instantiate(impactEffect, hit.point, Quaternion.LookRotation(hit.normal));
            Destroy(impactGameObject, 2f);
        }

        private void playFireSound()
        {
            fireSound.volume = 1f;
            fireSound.Play();
        }

        private void playReloadSound()
        {
            if (!isReloading())
            {
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

        public void ammoTextRefresh()
        {
            ammoText.GetComponent<Text>().text = ammoInClip.ToString() + "/" + totalAmmo.ToString();
        }
    }
}