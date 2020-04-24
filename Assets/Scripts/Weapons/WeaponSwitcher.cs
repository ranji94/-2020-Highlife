using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Com.Itronics.Highlife
{
    public class WeaponSwitcher : MonoBehaviour
    {
        [SerializeField] private GameObject[] weapons;
        [SerializeField] private GameObject grenade;
        [SerializeField] private float SwitchDelay = 0.2f;

        private int index;
        private bool isSwitching;
        private GameObject currentWeapon;

        void Start()
        {
            InitializeWeapons();
        }

        private void InitializeWeapons()
        {
            for (int i = 0; i < weapons.Length; i++)
            {
                weapons[i].SetActive(false);
            }
            weapons[0].SetActive(true);

            currentWeapon = weapons[0];
        }

        private void Update()
        {
            if (Input.GetAxis("Mouse ScrollWheel") > 0 
                && !isSwitching
                && isAnyWeaponActive())
            {
                index++;

                if (index >= weapons.Length)
                {
                    index = 0;
                }
                StartCoroutine(SwitchAfterDelay(index));

            }

            if (Input.GetAxis("Mouse ScrollWheel") < 0 
                && !isSwitching
                && isAnyWeaponActive())
            {
                index--;

                if (index < 0)
                {
                    index = weapons.Length - 1;
                }
                StartCoroutine(SwitchAfterDelay(index));
            }
        }

        private IEnumerator SwitchAfterDelay(int newIndex)
        {
            isSwitching = true;
            currentWeapon.GetComponent<Animator>().SetTrigger("HolsterWeapon");

            yield return new WaitForSeconds(SwitchDelay);
            isSwitching = false;
            SwitchWeapons(newIndex);
        }

        private void SwitchWeapons(int newIndex)
        {
            for (int i = 0; i < weapons.Length; i++)
            {
                weapons[i].SetActive(false);
            }
            weapons[newIndex].SetActive(true);

            currentWeapon = weapons[newIndex];
        }

        public void deactivateCurrentWeapon()
        {
            weapons[index].SetActive(false);
        }

        public void activateLastWeapon()
        {
            weapons[index].SetActive(true);
        }

        private bool isAnyWeaponActive() {
            for (int i = 0; i < weapons.Length; i++) {
                if (weapons[i].activeSelf) {
                    return true;
                }
            }
            return false;
        }
    }
}