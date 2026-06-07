using UnityEngine;
using FMODUnity;
using FMOD.Studio;

public class NightAmbienceManager : MonoBehaviour
{
    [Header("FMOD")]
    [SerializeField] private EventReference nightAmbienceEvent;

    private EventInstance nightAmbienceInstance;
    private bool isPlaying;

    private void Awake()
    {
        nightAmbienceInstance = RuntimeManager.CreateInstance(nightAmbienceEvent);
    }

    public void StartNightAmbience()
    {
        if (isPlaying)
            return;

        nightAmbienceInstance.start();
        isPlaying = true;
    }

    public void StopNightAmbience()
    {
        if (!isPlaying)
            return;

        nightAmbienceInstance.stop(FMOD.Studio.STOP_MODE.ALLOWFADEOUT);
        isPlaying = false;
    }

    private void OnDestroy()
    {
        if (nightAmbienceInstance.isValid())
        {
            nightAmbienceInstance.stop(FMOD.Studio.STOP_MODE.IMMEDIATE);
            nightAmbienceInstance.release();
        }
    }
}