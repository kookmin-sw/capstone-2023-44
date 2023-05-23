using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using System.IO;

public class TextFileManager : MonoBehaviour
{
    public GameObject textFilePrefab;
    public RectTransform ContentRect;
    public Scrollbar scrollBar;
    public TextMeshProUGUI textContent;
    public GameObject textContentObject;
    
    // Start is called before the first frame update
    void Start()
    {
        AddTextPrefab();
    }

    // Update is called once per frame
    void Update()
    {
        
    }
    public void AddTextPrefab()
    {
        DestroyPrefab();
        string path = "Assets/TextScript/Daily/Save"+DataManager.saveFile_No.ToString()+"/";
        DirectoryInfo directory = new DirectoryInfo(path);
        foreach(FileInfo File in directory.GetFiles())
        {
            if(Path.GetExtension(File.Name) != ".txt")
            {
                continue; // meta파일을 출력하지 않기 위한 거름망
            }
            bool isBottom = scrollBar.value <= 0.1f;
            GameObject memoIcon = Instantiate(textFilePrefab) as GameObject;
            TextIconScript memoText = memoIcon.GetComponent<TextIconScript>();
            memoText.UserText.text = File.Name;
            memoIcon.transform.SetParent(ContentRect.transform, false); // 실질적으로 출력
        }
    }
    public void DestroyPrefab()
    {
        foreach (Transform child in ContentRect)
        {
            Destroy(child.gameObject);
        }
    }
}
