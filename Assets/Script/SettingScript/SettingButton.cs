using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SettingButton : MonoBehaviour
{
    [SerializeField]
    GameObject settingPanel;
    // Start is called before the first frame update
    void Start()
    {
        //settingCanvas = GameObject.Find("SettingCanvas").GameObject;
    }

    // Update is called once per frame
    void Update()
    {
        //임시함수 
        if (Input.GetKeyDown(KeyCode.A))

        {

            LoadingSceneManager.LoadScene("AIScene");

        }
        
    }
    public void ToMap()
    {
        LoadingSceneManager.LoadScene("MapScene");

    }
    public void openSetting()
    {
        settingPanel.SetActive(true);
    }
}
