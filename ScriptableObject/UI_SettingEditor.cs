using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;

[CustomEditor(typeof(UI_Setting))]

public class UI_SettingEditor : Editor
{
    [MenuItem("Assets/Open UI Setting")]

    public static void OpenInspector()
    {
        Selection.activeObject = UI_Setting.Instance;
    }

    public override void OnInspectorGUI()
    {
        base.OnInspectorGUI();

        if(GUI.changed)
        {
            EditorUtility.SetDirty(target);
            AssetDatabase.SaveAssets();
        }
    }
}
