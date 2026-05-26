using System;
using UnityEngine;
using System.Collections;
using FMODUnity;
using FMOD.Studio;

public class FaceController : MonoBehaviour
{
    // skrypt pozwala nam zmienic ekspresje klienta. Zeby wywołać zmiane wystarczy linijka kodu np:
    // face.SetExpression(FaceController.Expression.Surprised);
    // audio klientów
    public enum Expression
    {
        // lista dostępnych emocji
        Neutral,
        Talk,
        Talk2,
        Surprised,
        Happy,
        Sad,
        Angry,
        TalkAngry,
        Extra
    }

    [Header("Material index of face material")]
    [SerializeField] private int faceMaterialIndex = 1;
    
    [Header("Audio")]
    [SerializeField] private EventReference clientTalk;
    public int voiceActorID;

    private Material faceMat;
    private Renderer rend;

    public float delay = 0.3f;
    
    private ClientController clientController; // access to client controller to get the Voice Actor index
    private Coroutine talkingRoutine; // to prevent audio spam

    // expression offsets
    private readonly Vector2[] offsets =
    {
        new Vector2(0f, 0f),       // Neutral
        new Vector2(0.333f, 0f),   // Talk
        new Vector2(0.666f, 0f),   // Talk2

        new Vector2(0f, -0.333f),       // Surprised
        new Vector2(0.333f, -0.333f),  // Happy
        new Vector2(0.666f, -0.333f),  // Sad

        new Vector2(0f, -0.666f),           // Angry
        new Vector2(0.333f, -0.666f),       // TalkAngry
        new Vector2(0.666f, -0.666f)        // Extra
    };
    
    // audio; emotions and voicelines
    public enum VoiceLine
    {
        Neutral,
        Angry,
        Happy,
        Waiting,
        Sad
    }

    private void Start()
    {
        rend = GetComponentInChildren<SkinnedMeshRenderer>();
        clientController = GetComponentInParent<ClientController>();

        if (rend == null)
        {
            //Debug.LogError("Renderer not found!");
            return;
        }

        Material[] mats = rend.materials;

        if (faceMaterialIndex >= mats.Length)
        {
            //Debug.LogError("Face material index out of range!");
            return;
        }

        faceMat = mats[faceMaterialIndex];

        //Debug.Log("Face material found: " + faceMat.name);
    }
    public void SetVoiceActor(int id)
    {
        voiceActorID = id;
    }

    public void SetExpression(Expression expression)
    {
        Vector2 offset = offsets[(int)expression];

        //Debug.Log("Changing to: " + expression);
        //Debug.Log("Offset: " + offset);

        faceMat.SetVector("_ExpressionOffset", offset);
    }

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.P))
        {
            //Debug.Log("Pressed P");
            TalkingNeutral();

        }
        
    }
    
    private EventInstance PlayVoiceLine(
        VoiceLine voiceLine)
    {
        if (clientController == null)
            return default;

        EventInstance instance =
            RuntimeManager.CreateInstance(clientTalk);

        instance.set3DAttributes(
            RuntimeUtils.To3DAttributes(transform)
        );

        // Voice Actor (discrete)
        instance.setParameterByName(
            "VA",
            voiceActorID
        );

        // Voice line (labeled)
        instance.setParameterByNameWithLabel(
            "Voice-line",
            GetVoiceLineLabel(voiceLine)
        );

        instance.start();

        return instance;
    }
    private string GetVoiceLineLabel(
        VoiceLine voiceLine)
    {
        switch (voiceLine)
        {
            case VoiceLine.Neutral:
                return "Neutral";

            case VoiceLine.Angry:
                return "Angry";

            case VoiceLine.Happy:
                return "Happy";

            case VoiceLine.Sad:
                return "Sad";

            case VoiceLine.Waiting:
                return "Waiting";

            default:
                return "Placeholder";
        }
    }
    // play audio, these are called from other scripts and start coroutines here 
    
    public void PlayTalkingHappy()
    {
        if (talkingRoutine != null)
            StopCoroutine(talkingRoutine);

        talkingRoutine =
            StartCoroutine(TalkingHappy());
    }

    public void PlayTalkingSad()
    {
        if (talkingRoutine != null)
            StopCoroutine(talkingRoutine);

        talkingRoutine =
            StartCoroutine(TalkingSad());
    }

    public void PlayTalkingMad()
    {
        if (talkingRoutine != null)
            StopCoroutine(talkingRoutine);

        talkingRoutine =
            StartCoroutine(TalkingMad());
    }
    
    public void PlayTalkingNeutral()
    {
        if (talkingRoutine != null)
            StopCoroutine(talkingRoutine);

        talkingRoutine =
            StartCoroutine(TalkingNeutral());
    }
    
    /// 
    /// NEUTRAL, TAKING ORDER
    /// 
    /// 

    public IEnumerator TalkingNeutral()
    {
        EventInstance voiceInstance =
            PlayVoiceLine(VoiceLine.Neutral);

        if (!voiceInstance.isValid())
            yield break;

        Expression[] talkingCycle =
        {
            Expression.Talk,
            Expression.Talk2,
            Expression.Neutral,
            Expression.Talk,
            Expression.Neutral
        };

        int index = 0;
        PLAYBACK_STATE playbackState;

        // Loop while audio is playing
        do
        {
            SetExpression(talkingCycle[index]);

            yield return new WaitForSecondsRealtime(delay);

            index++;

            if (index >= talkingCycle.Length)
                index = 0;

            voiceInstance.getPlaybackState(
                out playbackState
            );

        } while (playbackState != PLAYBACK_STATE.STOPPED);

        // Return face to neutral
        SetExpression(Expression.Neutral);

        voiceInstance.release();
    }
    
    /// 
    /// ORDER REACTIONS
    /// 
    /// happy reaction
    public IEnumerator TalkingHappy()
    {
        EventInstance voiceInstance =
            PlayVoiceLine(VoiceLine.Happy);

        PLAYBACK_STATE playbackState;

        do
        {
            SetExpression(Expression.Happy);
            yield return new WaitForSecondsRealtime(delay);

            SetExpression(Expression.Talk);
            yield return new WaitForSecondsRealtime(delay);

            voiceInstance.getPlaybackState(
                out playbackState
            );

        } while (playbackState != PLAYBACK_STATE.STOPPED);

        SetExpression(Expression.Neutral);

        voiceInstance.release();
    }
    
    // sad reaction (50%)
    public IEnumerator TalkingSad()
    {
        EventInstance voiceInstance =
            PlayVoiceLine(VoiceLine.Sad);

        PLAYBACK_STATE playbackState;

        do
        {
            SetExpression(Expression.Sad);
            yield return new WaitForSecondsRealtime(delay);

            SetExpression(Expression.Talk);
            yield return new WaitForSecondsRealtime(delay);

            voiceInstance.getPlaybackState(
                out playbackState
            );

        } while (playbackState != PLAYBACK_STATE.STOPPED);

        SetExpression(Expression.Neutral);

        voiceInstance.release();
    }
    
    // angry reaction (50%)
    public IEnumerator TalkingMad()
    {
        EventInstance voiceInstance =
            PlayVoiceLine(VoiceLine.Angry);

        PLAYBACK_STATE playbackState;

        do
        {
            SetExpression(Expression.Angry);
            yield return new WaitForSecondsRealtime(delay);

            SetExpression(Expression.TalkAngry);
            yield return new WaitForSecondsRealtime(delay);
 
            voiceInstance.getPlaybackState(
                out playbackState
            );

        } while (playbackState != PLAYBACK_STATE.STOPPED);

        SetExpression(Expression.Neutral);

        voiceInstance.release();
    }
}