using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CellPhone : MonoBehaviour
{
    public GameObject CellPhone_GameObject;
    public ScriptManager SManager;
    public GameObject CellPhone_Message_GameObject;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }
    public void CellPhoneOn()
    {
        //SManager.ScriptPanel.SetActive(false);
        CellPhone_GameObject.SetActive(true);
    }
    public void CellPhoneOff()
    {
        //SManager.ScriptPanel.SetActive(true);
        CellPhone_GameObject.SetActive(false);
    }
    public void BackButton()
    {
        CellPhone_Message_GameObject.SetActive(false);
    }
}
