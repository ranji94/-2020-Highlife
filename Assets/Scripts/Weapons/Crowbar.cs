using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Com.Itronocs.Highlife { 
public class Crowbar : MonoBehaviour
{
    public Animator anim;
    public AudioSource missSound;
    void Start()
    {
            anim = GetComponent<Animator>();
            missSound.Play(0);
        }

    void Update()
    {
            if (Input.GetMouseButton(0))
            {
                Debug.Log("CLICKED MOUSE");
                anim.SetBool("Fire", true);
                anim.SetInteger("AnimationNumber", 1);
                AnimatorStateInfo info = anim.GetCurrentAnimatorStateInfo(anim.GetLayerIndex("Base Layer"));
                if (!info.IsName("Fire"))
                {
                    playCrowbarMissSound();
                }
            }
    }

        private void FixedUpdate()
        {
            AnimatorStateInfo info = anim.GetCurrentAnimatorStateInfo(anim.GetLayerIndex("Base Layer"));
            Debug.Log(info.IsName("Fire"));

            if (info.IsName("Fire"))
            {
                anim.SetBool("Fire", false);
                anim.SetInteger("AnimationNumber", 0);
            }
        }

        private void playCrowbarMissSound()
        {
            if (!missSound.isPlaying)
            {
                missSound.volume = 1f;
                missSound.Play();
            }
        }
    }
}