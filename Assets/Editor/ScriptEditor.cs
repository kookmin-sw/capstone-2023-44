using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;
using System;

[CustomEditor(typeof(ScriptManager))]
public class ScriptEditor : Editor
{
    ScriptManager SManager;
    string text;

    void OnEnable()
    {
        SManager = target as ScriptManager;
    }
    public override void OnInspectorGUI()
    {
        base.OnInspectorGUI();

        EditorGUILayout.BeginHorizontal();
        text = EditorGUILayout.TextArea(text);

        if(GUILayout.Button("Day-DayTurn", GUILayout.Width(60)) && text.Trim() != "")
        {
            string[] splited = text.Split('-');
            DataManager.day_No = Convert.ToInt32(splited[0]);
            DataManager.day_Turn = Convert.ToInt32(splited[1]);
            SManager.storyIndex = 0;
            SManager.LoadSources();
            text = "";
            GUI.FocusControl(null);
        }

        EditorGUILayout.EndHorizontal();
    }
    // Start is called before the first frame update
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {

    }
}