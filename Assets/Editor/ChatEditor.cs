using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;

[CustomEditor(typeof(ChatManager))]
public class ChatEditor : Editor
{
    ChatManager chatManager;
    string text;

    void OnEnable()
    {
       // chatManager = target as ChatManager;
    }
    /*public override void OnInspectorGUI()
    {
        base.OnInspectorGUI();

        EditorGUILayout.BeginHorizontal();
        text = EditorGUILayout.TextArea(text);

        if(GUILayout.Button("보내기", GUILayout.Width(60)) && text.Trim() != "")
        {
            chatManager.Chat(true, text, "me", null);
            text = "";
            GUI.FocusControl(null);
        }
        if (GUILayout.Button("받기", GUILayout.Width(60)) && text.Trim() != "")
        {
            chatManager.Chat(false, text, "AI", null);
            text = "";
            GUI.FocusControl(null);
        }

        EditorGUILayout.EndHorizontal();
    }*/
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
