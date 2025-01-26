using UnityEngine;

public class BubblePad : MonoBehaviour
{
    void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            // Play the bubble pad sound from AudioManager
            AudioManager.instance.PlaySFX(AudioManager.instance.bubblePad);
        }
    }
}
