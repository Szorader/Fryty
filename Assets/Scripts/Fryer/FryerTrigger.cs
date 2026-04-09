using UnityEngine;

public class FryerTrigger : MonoBehaviour
{
    private Fryer2 parentFryer;

    void Start()
    {
        parentFryer = GetComponentInParent<Fryer2>();
    }

    private void OnTriggerStay(Collider other)
    {
        if (!other.CompareTag("Fry")) return;
        
        if (parentFryer != null && !parentFryer.isFrying)
        {
            parentFryer.StartFrying();
            
            Destroy(other.gameObject);
        }
    }
}