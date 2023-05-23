using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SettingClose : MonoBehaviour
{
    [SerializeField]
    GameObject settingCanvas;
    // Start is called before the first frame update
    void Start()
    {
        //settingCanvas = transform.Find("SettingCanvas").gameObject;
    }

    // Update is called once per frame
    void Update()
    {
        
    }
    public void closeSetting()
    {
        settingCanvas.SetActive(false);
    }
}
