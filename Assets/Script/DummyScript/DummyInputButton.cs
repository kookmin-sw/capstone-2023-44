using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DummyInputButton : MonoBehaviour
{
    [SerializeField]
    GameObject aI_PicManager;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }
    public void executeImgInput()
    {
        aI_PicManager.SetActive(true);
    }
}
