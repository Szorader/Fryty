using UnityEngine;
using UnityEditor;
using System.Collections.Generic;

public class MissingScriptsFinder : EditorWindow
{
    [MenuItem("Tools/Find Missing Scripts")]
    public static void ShowWindow()
    {
        GetWindow<MissingScriptsFinder>("Find Missing Scripts").ScanProject();
    }

    private void ScanProject()
    {
        string[] guids = AssetDatabase.FindAssets("t:GameObject");
        List<string> objectsWithMissing = new List<string>();

        foreach (string guid in guids)
        {
            string path = AssetDatabase.GUIDToAssetPath(guid);
            GameObject go = AssetDatabase.LoadAssetAtPath<GameObject>(path);

            Component[] components = go.GetComponentsInChildren<Component>(true);

            for (int i = 0; i < components.Length; i++)
            {
                if (components[i] == null)
                {
                    objectsWithMissing.Add(path + " (Missing Component at child: " + go.name + ")");
                    break;
                }
            }
        }

        if (objectsWithMissing.Count == 0)
        {
            EditorUtility.DisplayDialog("Find Missing Scripts", "Brak brakujących skryptów w prefabach i obiektach!", "OK");
        }
        else
        {
            string report = string.Join("\n", objectsWithMissing.ToArray());
            Debug.Log("==== Missing Scripts Found ====\n" + report);
            EditorUtility.DisplayDialog("Find Missing Scripts", $"Znaleziono {objectsWithMissing.Count} obiektów z brakującymi skryptami. Sprawdź konsolę.", "OK");
        }
    }
}