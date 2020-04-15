using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Com.Itronics.Highlife { 
public class Crate : MonoBehaviour
{
        public float durability;
        public AudioSource hitSound;
        public AudioSource crateDestroySound;
        public GameObject fracturedCrate;

        public void takeDamage(float amount)
        {
            durability -= amount;
            if (durability <= 0)
            {
                GameObject fracturedGameObj = Instantiate(fracturedCrate, transform.position, Quaternion.identity) as GameObject;
                Rigidbody[] allRigidBodies = fracturedGameObj.GetComponentsInChildren<Rigidbody>();
                if (allRigidBodies.Length > 0) {
                    foreach (var body in allRigidBodies) {
                        body.AddExplosionForce(Random.Range(4000, 11000), transform.position, 1);
                    }
                }
                Destroy(this.gameObject);
                Destroy(transform.parent.gameObject);
                Destroy(fracturedGameObj, 8f);
            }

            print("Enemy took damage. Durablility left:");
            print(durability);
        }
    }
}