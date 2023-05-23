using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using System.IO;
using TMPro;

public class ResearchInput : MonoBehaviour
{
    public TMP_InputField research_Contents;
    public GameObject research_GameObject;
    public GameObject write_Button;
    public TextFileManager textFileManager;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }
    public void InputResearchText()
    {
        string path = "Assets/TextScript/Daily/Save"+ DataManager.saveFile_No.ToString()+"/" + "Day"+DataManager.day_No.ToString();
        string contents = research_Contents.text;
        StreamWriter sw;
        if (false == File.Exists(path))
        {
            sw = new StreamWriter(path + ".txt");
            sw.WriteLine(contents);
            sw.Flush();
            sw.Close();
        }

        research_GameObject.SetActive(false);
        write_Button.SetActive(true);
        Debug.Log(contents);
    }
    public void openWrite()
    {
        textFileManager.DestroyPrefab();
        write_Button.SetActive(false);
        research_GameObject.SetActive(true);
    }
}
