using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Com.Itronics.Highlife
{
    public class Grenade : MonoBehaviour
    {
        public float delay = 3f;
        float countdown;
        public float radius = 5f;
        public float explosionForce = 100f;
        bool hasExploded = false;
        public GameObject explosionEffect;
        public AudioSource explosionSound;
        void Start()
        {
            countdown = delay;
        }

        void Update()
        {
            countdown -= Time.deltaTime;
            if (countdown <= 0f && !hasExploded)
            {
                Explode();
            }
        }

        void Explode()
        {
            hasExploded = true;
            Instantiate(explosionEffect, transform.position, transform.rotation);
            Collider[] colliders = Physics.OverlapSphere(transform.position, radius);
            explosionSound.volume = 1.0f;
            explosionSound.Play();

            foreach (Collider nearbyObjects in colliders)
            {
                Rigidbody rig = nearbyObjects.GetComponent<Rigidbody>();

                if (rig != null)
                {
                    rig.AddExplosionForce(explosionForce, transform.position, radius);
                }

                Destructible destructible = nearbyObjects.GetComponent<Destructible>();
                if (destructible != null)
                {
                    destructible.explodeDestructible();
                }
            }

            Collider[] collidersToMove = Physics.OverlapSphere(transform.position, radius);

            foreach (Collider nearbyObject in collidersToMove)
            {
                Rigidbody rig = nearbyObject.GetComponent<Rigidbody>();
                if (rig != null)
                {
                    rig.AddExplosionForce(explosionForce, transform.position, radius);
                }
            }

            Destroy(gameObject, 0.6f);
        }
    }
}