using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WorkGameRenderer : MonoBehaviour
{

    [SerializeField]
    private WorkUIManager uiManager;

    void Start()
    {
        
    }
    public void Start3()
    {
        uiManager.SetNextPaper();
    }

    void Update()
    {
        uiManager.UpdateTimer();
        uiManager.GameOver();
    }

}
