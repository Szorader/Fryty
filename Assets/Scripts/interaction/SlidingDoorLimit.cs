using UnityEngine;

public class SlidingDoorLimitedRange : MonoBehaviour
{
    [Header("Ruch")]
    public Vector3 moveAxis = Vector3.right;

    [Header("Zakres ruchu")]
    public float minOffset = -2f; // ile w lewo
    public float maxOffset = 3f;  // ile w prawo

    private Vector3 startPosition;

    void Start()
    {
        startPosition = transform.position;
        moveAxis = moveAxis.normalized;
    }

    void Update()
    {
        Vector3 offset = transform.position - startPosition;

        // ile drzwi przesunęły się wzdłuż osi
        float distance = Vector3.Dot(offset, moveAxis);

        // ograniczenie zakresu
        float clampedDistance = Mathf.Clamp(distance, minOffset, maxOffset);

        // ustawienie pozycji
        transform.position = startPosition + moveAxis * clampedDistance;
    }
}