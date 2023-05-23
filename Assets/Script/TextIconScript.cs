using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using System.IO;

public class TextIconScript : MonoBehaviour
{
    public TextMeshProUGUI UserText; // 게임상에서 보여질 text파일의 이름
    public GameObject textReadObject;
    public TextMeshProUGUI textRead; // 게임상에서 출력될 text파일의 내용
    public TextFileManager textIconManager;
    public string textContent; // 저장될 텍스트파일 내용
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
        textIconManager.DestroyPrefab(); // 내용을 보게되면 기존에 보여졌던 아이콘 삭제하는 부분
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
