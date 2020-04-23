using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace Com.Itronics.Highlife
{
    public class Crowbar : MonoBehaviour
    {
        public Animator anim;
        public Text ammoText;
        public AudioSource missSound;
        public AudioSource defaultHitSound;
        public Camera weaponCam;
        public float attackDamage;
        public float attackRange;
        public float fireRate;
        public float impactForce;
        public GameObject defaultImpactEffect;

        private float nextTimeToFire = 0f;

        void Start()
        {
            anim = GetComponent<Animator>();
            missSound.Play(0);
        }

        void Update()
        {
            if (Input.GetMouseButton(0)
                && Time.time >= nextTimeToFire)
            {
                Ray ray = weaponCam.ScreenPointToRay(Input.mousePosition);
                Vector3 crosshairCorrection = new Vector3(0.05f, -0.55f, 0f);
                RaycastHit hit;
                nextTimeToFire = Time.time + 1f / fireRate;

                if (Physics.Raycast(weaponCam.transform.position + crosshairCorrection,
                weaponCam.transform.forward,
                out hit, attackRange))
                {

                    hitObject(hit);

                }
                else
                {
                    anim.SetBool("Fire", true);
                    playCrowbarMissSound();
                }

                if (hit.rigidbody != null)
                {
                    hit.rigidbody.AddForce(-hit.normal * impactForce);
                }
            }

            refreshAmmoText();
        }

        private void refreshAmmoText()
        {
            ammoText.GetComponent<Text>().text = "";
        }

        private void hitObject(RaycastHit hit)
        {
            if (!isFiring())
            {
                AudioSource hitSound = defaultHitSound;
                GameObject impactEffect = defaultImpactEffect;

                Destructible destructible = hit.collider.GetComponent<Destructible>();
                if (hit.collider.tag == "Destructible")
                {
                    destructible.takeDamage(attackDamage);
                    hitSound = destructible.hitSound;
                    impactEffect = destructible.bulletImpactEffect;
                }

                GameObject impactGameObject = Instantiate(impactEffect, hit.point, Quaternion.LookRotation(hit.normal));
                Destroy(impactGameObject, 2f);
                playCrowbarHit(hitSound);
                anim.SetBool("Attack", true);
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