using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Xml;
using System;

public class DataManager : MonoBehaviour
{ 
    public static int day_No = 0;
    public static int script_No = 0; // ��ü������ �����ؾ��� Day�� Script��ȣ << ���ν��丮
    public static int saveFile_No = 0; // ���° save�������� �����ϴ� �Լ� save�� �ʼ���
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
        XmlDocument saveFile = new XmlDocument(); //xml���� ����
        XmlElement xmlRoot = saveFile.CreateElement("Save"); //��Ʈ ��� ����

        XmlElement day = saveFile.CreateElement("Day"); //Child ��� ����
        XmlElement scriptNo = saveFile.CreateElement("ScriptNo"); 
        XmlElement stats = saveFile.CreateElement("Stats");
        XmlElement turn = saveFile.CreateElement("Turn");
        //Child ��ҵ鿡 ���� ���
        day.InnerText = day_No.ToString();
        scriptNo.InnerText = script_No.ToString();
        turn.InnerText = day_Turn.ToString();
        stats.InnerText = "Not Yet";
        //XML Save������ �°� ��Ʈ�� Child���� Append
        xmlRoot.AppendChild(day);
        xmlRoot.AppendChild(turn);
        xmlRoot.AppendChild(scriptNo);
        xmlRoot.AppendChild(stats);
        //SaveFile�� Append�� Save
        saveFile.AppendChild(xmlRoot);
        saveFile.Save(save_Path);
    }
    
}
