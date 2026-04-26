using UnityEngine;

public class sexykon : MonoBehaviour
{
    void Update()
    {
        Vector3 direction = Camera.main.transform.position - transform.position;
        direction.y = 0; // blokuje przechylanie góra-dół
        transform.rotation = Quaternion.LookRotation(direction);
    }
}
