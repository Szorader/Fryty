using UnityEngine;
using FMODUnity;

public class Endgame : MonoBehaviour
{
    [SerializeField] private EventReference firstKnock;
    [SerializeField] private EventReference secondKnock;
    [SerializeField] private EventReference doorKickDown;
    
    // RuntimeManager.PlayOneShot(firstKnock, transform.position);
    // RuntimeManager.PlayOneShot(secondKnock, transform.position);
    // RuntimeManager.PlayOneShot(doorKickDown, transform.position);
}
