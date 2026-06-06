using UnityEngine;

public class BasketballHoop : MonoBehaviour
{
    // triggers confetti particle if potato is thrown into the basketball hoop
   
    
    [Header("Effects")]
    [SerializeField] private ParticleSystem confetti;

    private void OnTriggerEnter(Collider other)
    {
        if (!other.CompareTag("Potato"))
            return;

        if (confetti != null)
        {
            confetti.Play();
        }

        Debug.Log("SCORE!");
    }
}