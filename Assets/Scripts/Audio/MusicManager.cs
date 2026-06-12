using UnityEngine;
using FMODUnity;
using FMOD.Studio;
using System.Collections;

using STOP_MODE = FMOD.Studio.STOP_MODE;

// plays music on scene, skips to next track if music ends or if you click the radio

public class MusicManager : MonoBehaviour
{
   public static MusicManager Instance;

    [Header("Tracks")]
    [SerializeField] private EventReference[] tracks;

    private EventInstance currentTrack;
    private int currentTrackIndex = -1;

    private Coroutine monitorRoutine;

    private void Awake()
    {
        if (Instance != null)
        {
            Destroy(gameObject);
            return;
        }

        Instance = this;
        DontDestroyOnLoad(gameObject);
    }

    private void Start()
    {
        if (tracks.Length > 0)
        {
            PlayTrack(0);
        }
    }

    public void NextTrack()
    {
        if (tracks.Length == 0)
            return;

        int next =
            (currentTrackIndex + 1) %
            tracks.Length;

        PlayTrack(next);
    }

    private void PlayTrack(int index)
    {
        if (monitorRoutine != null)
            StopCoroutine(monitorRoutine);

        if (currentTrack.isValid())
        {
            currentTrack.stop(STOP_MODE.IMMEDIATE);
            currentTrack.release();
        }

        currentTrackIndex = index;

        currentTrack =
            RuntimeManager.CreateInstance(
                tracks[currentTrackIndex]
            );

        currentTrack.start();

        monitorRoutine =
            StartCoroutine(
                WaitForTrackEnd()
            );
    }

    private IEnumerator WaitForTrackEnd()
    {
        PLAYBACK_STATE state;

        while (true)
        {
            if (!currentTrack.isValid())
                yield break;

            currentTrack.getPlaybackState(
                out state
            );

            if (state == PLAYBACK_STATE.STOPPED)
            {
                NextTrack();
                yield break;
            }

            yield return new WaitForSeconds(
                0.25f
            );
        }
    }

    private void OnDestroy()
    {
        if (currentTrack.isValid())
        {
            currentTrack.stop(
                STOP_MODE.IMMEDIATE
            );

            currentTrack.release();
        }
    }
}
