using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Com.Itronics.Highlife { 
    public class Motion : MonoBehaviour
    {
        public float speed;
        private Rigidbody rig;

        private void Start() {
            Camera.main.enabled = true;
            rig = GetComponent<Rigidbody>();
        }

        void FixedUpdate() {
            float tHmove = Input.GetAxis("Horizontal");
            float tVmove = Input.GetAxis("Vertical");

            Vector3 tDirection = new Vector3(tHmove, 0, tVmove);
            tDirection.Normalize();

            rig.velocity = transform.TransformDirection(tDirection) * speed * Time.deltaTime;
        }
    }
}   