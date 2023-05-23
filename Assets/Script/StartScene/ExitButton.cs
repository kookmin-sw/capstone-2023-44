using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ExitButton : MonoBehaviour
{
    public GameObject exitObject;
    public TextFileManager textFileManager;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }
    public void Exit()
    {
        textFileManager.AddTextPrefab();
        exitObject.SetActive(false);

    }
}
