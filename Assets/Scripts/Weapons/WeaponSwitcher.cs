using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Com.Itronics.Highlife
{
    public class WeaponSwitcher : MonoBehaviour
    {
        public int selectedWeapon = 0;
        void Start()
        {
            SelectWeapon(0.2f);
        }

        void Update()
        {
            int previousSelectedWeapon = selectedWeapon;

            if (Input.GetAxis("Mouse ScrollWheel") > 0f)
            {
                if (selectedWeapon >= transform.childCount - 1) selectedWeapon = 0;
                else selectedWeapon++;
            }
            if (Input.GetAxis("Mouse ScrollWheel") < 0f)
            {
                if (selectedWeapon <= 0)
                {
                    selectedWeapon = transform.childCount - 1;
                }
                else
                {
                    selectedWeapon--;
                }
            }

            if (previousSelectedWeapon != selectedWeapon)
            {
                StartCoroutine(SelectWeapon(0.2f));
            }
        }

        private IEnumerator SelectWeapon(float time)
        {
            yield return new WaitForSeconds(time);
            int i = 0;
            foreach (Transform weapon in transform)
            {
                if (i == selectedWeapon)
                {
                    weapon.gameObject.SetActive(true);
                }
                else
                {
                    weapon.gameObject.SetActive(false);
                }
                i++;
            }
        }
    }
}