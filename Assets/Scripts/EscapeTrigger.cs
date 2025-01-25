using UnityEngine;

public class EscapeTrigger : MonoBehaviour
{
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            // Disable player movement
            other.GetComponent<PlayerController>().enabled = false;

            // Display a winning message in the console
            Debug.Log("Woohoo You Escaped!");
        }
    }
        }
