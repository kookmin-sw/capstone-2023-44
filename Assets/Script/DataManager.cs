using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Xml;
using System;

public class DataManager : MonoBehaviour
{ 
    public static int day_No = 0;
    public static int script_No = 0; // 전체적으로 저장해야할 Day와 Script번호 << 메인스토리
    public static int saveFile_No = 0; // 몇번째 save파일인지 저장하는 함수 save에 필수적
    public static int day_Turn = 0;
    // Start is called before the first frame update
    void Start()
    {
        //camera = GameObject.Find("Main Camera").GetComponent<Transform>;
    }

    // Update is called once per frame
    void Update()
    {
       
    }
    public static void save()
    {
        string save_Path = Application.dataPath + "/TextScript/DummySaveFile/Save" + saveFile_No.ToString() + ".xml";
        XmlDocument saveFile = new XmlDocument(); //xml파일 생성
        XmlElement xmlRoot = saveFile.CreateElement("Save"); //루트 요소 생성

        XmlElement day = saveFile.CreateElement("Day"); //Child 요소 생성
        XmlElement scriptNo = saveFile.CreateElement("ScriptNo"); 
        XmlElement stats = saveFile.CreateElement("Stats");
        XmlElement turn = saveFile.CreateElement("Turn");
        //Child 요소들에 값을 배부
        day.InnerText = day_No.ToString();
        scriptNo.InnerText = script_No.ToString();
        turn.InnerText = day_Turn.ToString();
        stats.InnerText = "Not Yet";
        //XML Save구조에 맞게 루트에 Child값을 Append
        xmlRoot.AppendChild(day);
        xmlRoot.AppendChild(turn);
        xmlRoot.AppendChild(scriptNo);
        xmlRoot.AppendChild(stats);
        //SaveFile에 Append후 Save
        saveFile.AppendChild(xmlRoot);
        saveFile.Save(save_Path);
    }
    
}
