/*using UnityEngine;

public class LookAtCamera : MonoBehaviour
{
    public Transform target; // kapsuła / obiekt
    public float distance = 0.6f;
    private float height = 0f;
    private Camera cam;
    private Canvas prompt;
    public float canvasHeight;

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
        dir.y = height;

        // ustawiamy pozycję NA OKRĘGU wokół obiektu
        Vector3 pos = target.position + dir * distance;
        pos.y += canvasHeight;

        transform.position = pos;

        // obrót do kamery
        transform.forward = cam.transform.forward;
    }






}*/
using UnityEngine;

public class LookAtCamera : MonoBehaviour
{
    public Transform target;
    public float height = 1.8f;
    private Camera cam;

    void Start()
    {
        cam = Camera.main;
    }

    void LateUpdate()
    {
        if (!target) return;

        // pozycja centralnie nad głową
        transform.position = target.position + Vector3.up * height;

        // kierunek do kamery (tylko Y)
        Vector3 lookPos = cam.transform.position - transform.position;
        lookPos.y = 0;

        // obrót tylko w osi Y
        transform.rotation = Quaternion.LookRotation(-lookPos);
    }
}