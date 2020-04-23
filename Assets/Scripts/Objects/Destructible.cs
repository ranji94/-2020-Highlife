using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Com.Itronics.Highlife
{
    public class Destructible : MonoBehaviour
    {
        public float durability;
        public AudioSource hitSound;
        public AudioSource destructibleDestroySound;
        public GameObject fracturedDestructible;
        public GameObject bulletImpactEffect;

        public void takeDamage(float amount)
        {
            durability -= amount;
            if (durability <= 0)
            {
                GameObject fracturedGameObj = Instantiate(fracturedDestructible, transform.position, Quaternion.identity) as GameObject;
                Rigidbody[] allRigidBodies = fracturedGameObj.GetComponentsInChildren<Rigidbody>();
                if (allRigidBodies.Length > 0)
                {
                    foreach (var body in allRigidBodies)
                    {
                        body.AddExplosionForce(Random.Range(400, 800), transform.position, 1);
                    }
                }
                Destroy(this.gameObject);
                destructibleDestroySound.volume = 1.0f;
                destructibleDestroySound.Play();
                Destroy(transform.parent.gameObject);
                Destroy(fracturedGameObj, 8f);
            }
        }

        public void explodeDestructible()
        {
            takeDamage(durability);
        }
    }
}