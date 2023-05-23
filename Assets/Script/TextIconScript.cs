using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using System.IO;

public class TextIconScript : MonoBehaviour
{
    public TextMeshProUGUI UserText; // ���ӻ󿡼� ������ text������ �̸�
    public GameObject textReadObject;
    public TextMeshProUGUI textRead; // ���ӻ󿡼� ��µ� text������ ����
    public TextFileManager textIconManager;
    public string textContent; // ����� �ؽ�Ʈ���� ����
    // Start is called before the first frame update
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {
        
    }
    public void getText()
    {
        textIconManager = GameObject.Find("TextFileManager").GetComponent<TextFileManager>();
        textIconManager.DestroyPrefab(); // ������ ���ԵǸ� ������ �������� ������ �����ϴ� �κ�
        string path = "Assets/TextScript/Daily/Save" + DataManager.saveFile_No.ToString() + "/" + UserText.text;
        FileStream read = new FileStream(path, FileMode.Open);
        StreamReader reader = new StreamReader(read);
        textContent = reader.ReadToEnd();
        textReadObject = GameObject.Find("TextFileManager").GetComponent<TextFileManager>().textContentObject;
        textRead = GameObject.Find("TextFileManager").GetComponent<TextFileManager>().textContent;
        textRead.text = textContent;
        textReadObject.SetActive(true);
        read.Close();
        reader.Close();
    }
    void saveTextContent()
    {
        string path = "Assets/TextScript/Daily/Save" + DataManager.saveFile_No.ToString() + "/";
    }
}
