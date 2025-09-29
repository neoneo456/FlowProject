using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Footsteps : MonoBehaviour
{
    public AudioSource footstepSound;
    private Animator anim;

    void Start()
    {
        anim = GetComponent<Animator>();
    }

    void Update()
    {
        // ใช้ Speed จาก Animator ที่ PlayerController เซ็ตเอาไว้
        float speed = anim.GetFloat("Speed");

        if (speed > 0.01f)  // ตัวละครกำลังเดิน
        {
            if (!footstepSound.isPlaying)
            {
                footstepSound.Play();
            }
        }
        else
        {
            footstepSound.Stop();
        }
    }
}
