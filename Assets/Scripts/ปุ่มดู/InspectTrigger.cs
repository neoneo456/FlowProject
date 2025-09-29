using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InspectTrigger : MonoBehaviour
{
   
    
    public GameObject boardPrefab;
    public Transform spawnPoint;
    private GameObject currentBoard;

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Player"))
        {
            if (boardPrefab != null && currentBoard == null)
            {
                currentBoard = Instantiate(boardPrefab, spawnPoint.position, Quaternion.identity);
            }
        }
    }

    private void OnTriggerExit2D(Collider2D other)
    {
        if (other.CompareTag("Player"))
        {
            if (currentBoard != null)
            {
                Destroy(currentBoard);
                currentBoard = null;
            }
        }
    }
}
