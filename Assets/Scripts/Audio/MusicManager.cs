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
    [SerializeField]
    private EventReference[] tracks;

    [SerializeField]
    private float fadeTime = 0.5f;

    private EventInstance currentTrack;

    private int currentTrackIndex = -1;

    private Coroutine transitionRoutine;

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

    public void PlayTrack(int index)
    {
        if (transitionRoutine != null)
            StopCoroutine(transitionRoutine);

        transitionRoutine =
            StartCoroutine(
                TransitionToTrack(index)
            );
    }

    private IEnumerator TransitionToTrack(
        int newIndex)
    {
        if (currentTrack.isValid())
        {

            currentTrack.stop(
                STOP_MODE.ALLOWFADEOUT
            );


            yield return new WaitForSeconds(
                fadeTime
            );
            currentTrack.release();
        }

        currentTrackIndex = newIndex;
        
        currentTrack =
            RuntimeManager.CreateInstance(
                tracks[currentTrackIndex]
            );
        
        FMOD.RESULT result =
            currentTrack.start();
        
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
