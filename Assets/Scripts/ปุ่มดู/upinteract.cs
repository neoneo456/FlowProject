using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class upinteract : MonoBehaviour
{
    
   
    public GameObject targetUI;
    private GameObject player;
    private PlayerController playerController;
    private Rigidbody2D playerRb;
    private Animator playerAnimator;
    private bool playerInside = false;

    void Start()
    {
        if (targetUI != null)
        {
            targetUI.SetActive(false);
        }
    }

   void Update()
{
    // อ่านค่าแกน DPad_Y (ต้องไปตั้งค่าใน Input Manager ชื่อ "DPad_Y")
    float dpadY = Input.GetAxis("DPad_Y");

    // ---------- OPEN ----------
    if (playerInside && dpadY > 0.5f) // <<< แก้ตรงนี้ จาก GetKeyDown(...) เป็นเช็กแกน > 0.5
    {
        if (targetUI != null)
        {
            targetUI.SetActive(true);
        }

        if (playerController != null)
        {
            playerController.enabled = false;
        }

        if (playerRb != null)
        {
            playerRb.velocity = Vector2.zero;
        }

        if (playerAnimator != null)
        {
            playerAnimator.SetFloat("Speed", 0f);
        }
    }

    // ---------- CLOSE ----------
    if (Input.GetKeyDown(KeyCode.Escape) || Input.GetKeyDown(KeyCode.JoystickButton1))
    {
        if (targetUI != null)
        {
            targetUI.SetActive(false);
        }

        if (playerController != null)
        {
            playerController.enabled = true;
        }
    }
}

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Player"))
        {
            playerInside = true;
            player = other.gameObject;
            playerController = player.GetComponent<PlayerController>();
            playerRb = player.GetComponent<Rigidbody2D>();
            playerAnimator = player.GetComponent<Animator>();
        }
    }

    private void OnTriggerExit2D(Collider2D other)
    {
        if (other.CompareTag("Player"))
        {
            playerInside = false;

            if (targetUI != null)
            {
                targetUI.SetActive(false);
            }

            if (playerController != null)
            {
                playerController.enabled = true;
            }
        }
    }

}
