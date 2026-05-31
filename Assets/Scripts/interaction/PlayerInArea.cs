using UnityEngine;

public class PlayerInArea : MonoBehaviour
{
    //czy jest w srodku w trucku
    public bool inArea = false;
    
    private Camera cam;
    private void Start()
    {
        cam = Camera.main;
    }
    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            inArea = true;
            cam.transform.position += new Vector3(0f, -0.25f, 0f);
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            inArea = false;
            cam.transform.position += new Vector3(0f, 0.25f, 0f);
        }
    }
}
