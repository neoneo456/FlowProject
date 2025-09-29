using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerInteract : MonoBehaviour
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
        if (playerInside && (Input.GetKeyDown(KeyCode.Return) || Input.GetKeyDown(KeyCode.JoystickButton0)))
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
