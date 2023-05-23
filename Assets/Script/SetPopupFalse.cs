using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SetPopupFalse : MonoBehaviour
{
    public GameObject ScriptPanel;
    public GameObject BlockCanvas;
    public ScriptManager SManager;
    // Start is called before the first frame update
    void Start()
    {
        gameObject.SetActive(false);
    }

    // Update is called once per frame
    void Update()
    {
        
    }
    public void OffAI()
    {
        gameObject.SetActive(false);
        ScriptPanel.SetActive(true);
        BlockCanvas.SetActive(false);
        SManager.printScript();
        
    }
}
