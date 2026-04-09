using UnityEngine;

public class CutterTrigger : MonoBehaviour
{
    private Cutter parentCutter;

    private void Start()
    {
        parentCutter = GetComponentInParent<Cutter>();
    }

    private void OnTriggerEnter(Collider other)
    {
        if (parentCutter != null)
        {
            parentCutter.HandleTrigger(other);
        }
    }
}