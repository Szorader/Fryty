using UnityEngine;
using TMPro;
using UnityEngine.UI;
using System;
using System.Collections.Generic;

public class PaperButton : MonoBehaviour
{
    [Serializable]
    public class Option
    {
        public string label;
        public UnityEngine.Events.UnityEvent action;
    }

    [SerializeField] private TMP_Text text;
    [SerializeField] private Button button;
    [SerializeField] private List<Option> options = new List<Option>();

    private int index;

    private void Start()
    {
        button.onClick.AddListener(Next);

        if (options.Count > 0)
        {
            Apply(0);
        }
    }

    private void Next()
    {
        if (options.Count == 0) return;

        index = (index + 1) % options.Count;
        Apply(index);
    }

    private void Apply(int i)
    {
        text.text = options[i].label;
        options[i].action?.Invoke();
    }
}