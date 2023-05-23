using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using B83.Win32;

public class AI_PicManager : MonoBehaviour
{
    [SerializeField]
    RectTransform fileImg;
    [SerializeField]
    GameObject fileImg_Active;
    [SerializeField]
    RectTransform dropZone;
    [SerializeField]
    FileDragAndDrop fileDrag;
    // Start is called before the first frame update
    void Start()
    {
        gameObject.SetActive(false);
        fileImg_Active.SetActive(false);
    }

    // Update is called once per frame
    void Update()
    {
        Vector3 mousePoint;
        mousePoint = Camera.main.ScreenToWorldPoint(new Vector3(Input.mousePosition.x, Input.mousePosition.y, -Camera.main.transform.position.z));
        if (gameObject.activeSelf == true && Input.GetMouseButton(0))
        {
            fileImg_Active.SetActive(true);
            fileImg.position = mousePoint;
        }
        if(Input.GetMouseButtonUp(0) == true)
        {
            fileImg_Active.SetActive(false);
        }

    }

}
