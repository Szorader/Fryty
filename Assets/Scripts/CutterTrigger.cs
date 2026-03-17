using UnityEngine;

public class CutterTrigger : MonoBehaviour
{
    private Cutter parentSpawner;

    void Start()
    {
        // 🔥 znajdź parenta z naszym skryptem
        parentSpawner = GetComponentInParent<Cutter>();
    }

    private void OnTriggerEnter(Collider other)
    {
        if (parentSpawner != null)
        {
            parentSpawner.HandleTrigger(other);
        }
    }
}