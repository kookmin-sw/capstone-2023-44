using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    ImageManager IManager;
    ScriptManager SManager;

    // Start is called before the first frame update
    void Start()
    {
        IManager = GameObject.Find("ImageManager").GetComponent<ImageManager>();
        SManager = GameObject.Find("ScriptManager").GetComponent<ScriptManager>();

    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Space))
        {

            //IManager.LoadImage("left", "flipping", "", 0.4f, 0,0);

        }
        if (Input.GetKeyDown(KeyCode.S))

        {

            SManager.printScript();

        }
        if (Input.GetKeyDown(KeyCode.D))

        {

            //IManager.LoadImage("left", "change", "Assets/Design/UI_BG.png", 0.4f, 10, 0);

        }
        if (Input.GetKeyDown(KeyCode.M))//M�� ������ ������ ��ŸƮ������ ���ư��Բ�. ���� SAVEUI�� �����ϱ�

        {

            DataManager.save();
            LoadingSceneManager.LoadScene("StartScene");

        }
    }
}
