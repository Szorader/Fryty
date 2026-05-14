using System;
using UnityEngine;

public class FaceController : MonoBehaviour
{
    
    public enum Expression
    {
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

    private Material faceMat;
    private Renderer rend;

    private readonly Vector2[] offsets =
    {
        new Vector2(0f, 0.666f),       // Neutral
        new Vector2(0.333f, 0.666f),   // Talk
        new Vector2(0.666f, 0.666f),   // Talk2

        new Vector2(0f, 0.333f),       // Surprised
        new Vector2(0.333f, -0.333f),  // Happy
        new Vector2(0.666f, -0.333f),  // Sad

        new Vector2(0f, 0f),           // Angry
        new Vector2(0.333f, 0f),       // TalkAngry
        new Vector2(0.666f, 0f)        // Extra
    };

    private void Awake()
    {
        rend = GetComponent<Renderer>();

        if (rend == null)
        {
            Debug.LogError("Renderer not found!");
            return;
        }

        Material[] mats = rend.materials;

        if (faceMaterialIndex >= mats.Length)
        {
            Debug.LogError("Face material index out of range!");
            return;
        }

        faceMat = mats[faceMaterialIndex];

        Debug.Log("Face material found: " + faceMat.name);
    }

    public void SetExpression(Expression expression)
    {
        Vector2 offset = offsets[(int)expression];

        Debug.Log("Changing to: " + expression);
        Debug.Log("Offset: " + offset);

        faceMat.SetVector("_ExpressionOffset", offset);
    }

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.P))
        {
            Debug.Log("Pressed P");
            SetExpression(Expression.Happy);
        }
    }
}