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
        public GameObject grenade;
        public Text ammoText;

        private float fireRate = 0.5f;
        private float nextTimeToFire;

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
            totalAmmo--;
            GameObject grenadeObject = Instantiate(grenade, transform.position, transform.rotation);
            Rigidbody rig = grenadeObject.GetComponent<Rigidbody>();
            rig.AddForce(transform.forward * throwForce, ForceMode.VelocityChange);
            rig.AddForce(transform.up * 5f, ForceMode.VelocityChange);
        }

        private void refreshAmmoText()
        {
            ammoText.text = totalAmmo.ToString();
        }
    }
}