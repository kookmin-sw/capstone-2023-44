using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using System.IO;
public class MessageCellPhone : MonoBehaviour
{
    public GameObject messagePrefab;
    public RectTransform ContentRect;
    public Scrollbar scrollBar;
    public TextMeshProUGUI textContent;
    public GameObject messageImage_GameObject;
    // Start is called before the first frame update
    void Start()
    {
        AddTextPrefab();
    }

    // Update is called once per frame
    void Update()
    {
        
    }
    public void MessageOn()
    {
        messageImage_GameObject.SetActive(true);
    }
    public void AddTextPrefab()
    {
        DestroyPrefab();
        string path = "Assets/TextScript/Message/Save" + DataManager.saveFile_No.ToString() + "/";
        DirectoryInfo directory = new DirectoryInfo(path);
        foreach (FileInfo File in directory.GetFiles())
        {
            if (Path.GetExtension(File.Name) != ".txt")
            {
                continue; // meta������ ������� �ʱ� ���� �Ÿ���
            }
            bool isBottom = scrollBar.value <= 0.1f;
            GameObject messageIcon = Instantiate(messagePrefab) as GameObject;
            MessageContent messageText = messageIcon.GetComponent<MessageContent>();
            messageText.nameText.text = Path.GetFileNameWithoutExtension(File.Name);

            string filePath = "Assets/TextScript/Message/Save" + DataManager.saveFile_No.ToString() + "/" + File.Name;
            FileStream read = new FileStream(filePath, FileMode.Open);
            StreamReader reader = new StreamReader(read);
            string textContent = reader.ReadToEnd();
            messageText.contentText.text = textContent;
            messageText.transform.SetParent(ContentRect.transform, false); // ���������� ���
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
