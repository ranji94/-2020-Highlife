using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Com.Itronics.Highlife { 
public class Crate : MonoBehaviour
{
        public float durability;

        public void takeDamage(float amount) {
            durability -= amount;

            if (durability <= 0) {
                Debug.Log("CRATE DESTROYED");
            }

            Debug.Log("Crate takes hit");
        }
}
}