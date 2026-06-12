using UnityEngine;

public class TutorialArrow : MonoBehaviour
{
    [SerializeField]
    private float distance = 0.15f;

    [SerializeField]
    private float speed = 3f;

    private Vector3 startPos;

    private void OnEnable()
    {
        startPos = transform.localPosition;
    }

    private void Update()
    {
        transform.localPosition =
            startPos +
            transform.up *
            Mathf.Sin(Time.time * speed) *
            distance;
    }
}
