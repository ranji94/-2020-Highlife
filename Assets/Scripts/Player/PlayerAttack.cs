using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Com.Itronics.Highlife;

namespace Com.Itronics.Highlife {
    public class PlayerAttack : MonoBehaviour
    {
        public Crowbar crowbar;
        public Camera weaponCam;

        void Start()
        {
            crowbar = GetComponent<Crowbar>();
        }

        private void Update()
        {
            if (Input.GetMouseButtonUp(0)) {
                crowbarHit();
            }
        }

        private void crowbarHit()
        {
            Ray ray = weaponCam.ScreenPointToRay(Input.mousePosition);
            RaycastHit hit;

            if (Physics.Raycast(ray, out hit, crowbar.attackRange)) {
                if (hit.collider.tag == "Crate") {
                    Crate durability = hit.collider.GetComponent<Crate>();
                    durability.takeDamage(crowbar.attackDamage);
                }
            }
        }
}
}