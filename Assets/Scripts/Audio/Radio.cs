using UnityEngine;

public class Radio : MonoBehaviour
{
    private void OnMouseDown()
    {
        if (MusicManager.Instance != null)
        {
            MusicManager.Instance.NextTrack();
        }
    }
}
