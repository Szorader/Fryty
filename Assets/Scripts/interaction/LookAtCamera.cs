using UnityEngine;

public class LookAtCamera : MonoBehaviour
{
    public Transform target; // kapsuła / obiekt
    public float distance = 0.6f;
    private float height = 0f;
    private Camera cam;
    private Canvas prompt;

    private void Start()
    {
        cam = Camera.main;
        prompt = GetComponent<Canvas>();
        if (prompt != null)
            prompt.enabled = false;
    }

    private void LateUpdate()
    {
        if (target == null) return;

        // kierunek od obiektu do kamery
        Vector3 dir = (cam.transform.position - target.position).normalized;

        // ignorujemy Y żeby nie latało góra-dół
        dir.y = 0;

        // ustawiamy pozycję NA OKRĘGU wokół obiektu
        Vector3 pos = target.position + dir * distance;
        pos.y += height;

        transform.position = pos;

        // obrót do kamery
        transform.forward = cam.transform.forward;
    }
    
    
        

        
        
}