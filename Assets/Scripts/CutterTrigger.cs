using UnityEngine;

public class CutterTrigger : MonoBehaviour
{
    private Cutter parentSpawner;

    void Start()
    {
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