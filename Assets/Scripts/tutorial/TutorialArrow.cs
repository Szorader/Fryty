using UnityEngine;

public class TutorialArrow : MonoBehaviour
{
    [SerializeField] private float bounceDistance = 0.15f;
    [SerializeField] private float bounceSpeed = 3f;

    private Vector3 startPos;

    private void OnEnable()
    {
        startPos = transform.localPosition;
    }

    private void Update()
    {
        float offset =
            Mathf.Sin(Time.time * bounceSpeed)
            * bounceDistance;

        transform.localPosition =
            startPos +
            transform.up * offset;
    }
}
