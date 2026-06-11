using UnityEngine;

public class CarDrive : MonoBehaviour
{
    public float speed = 10f;

    private void Update()
    {
        transform.Translate(Vector3.forward * speed * Time.deltaTime);
    }
}