using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace Com.Itronics.Highlife
{
    public class GrenadeThrow : MonoBehaviour
    {
        public float throwForce = 10f;
        public int totalAmmo = 5;
        public float animationTime = 1f;
        public GameObject grenade;
        public Text ammoText;

        private float fireRate = 0.5f;
        private float nextTimeToFire;
        public GameObject grenadeHands;
        public WeaponSwitcher weaponSwitcher;

        void Update()
        {
            if (Input.GetKeyUp(KeyCode.G)
                && totalAmmo > 0
                && Time.time >= nextTimeToFire)
            {
                nextTimeToFire = Time.time + 1f / fireRate;
                ThrowGrenade();
            }
            refreshAmmoText();
        }

        private void ThrowGrenade()
        {
            grenadeHands.SetActive(true);
            weaponSwitcher.deactivateCurrentWeapon();
            StartCoroutine(deactivateAnimation(animationTime));
            StartCoroutine(performThrow(animationTime));
        }

        private void refreshAmmoText()
        {
            ammoText.text = totalAmmo.ToString();
        }

        private IEnumerator deactivateAnimation(float time)
        {
            yield return new WaitForSeconds(time);
            grenadeHands.SetActive(false);
            weaponSwitcher.activateLastWeapon();
        }

        private IEnumerator performThrow(float animationTime)
        {
            yield return new WaitForSeconds(animationTime - 0.2f);
            totalAmmo--;
            GameObject grenadeObject = Instantiate(grenade, transform.position, transform.rotation);
            Rigidbody rig = grenadeObject.GetComponent<Rigidbody>();
            rig.AddForce(transform.forward * throwForce, ForceMode.VelocityChange);
            rig.AddForce(transform.up * 5f, ForceMode.VelocityChange);
        }
    }
}