using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class WorkManager : MonoBehaviour
{

    private GameObject onTablePaperObj;

    public Timer timer;

    public int wage { get; set; } = 0;

    [SerializeField]
    private float initTime;

    [SerializeField]
    private PaperManager paperManager;

    [SerializeField]
    private XMLManager DataManager;

    [SerializeField]
    private GameObject ThesisZone;

    [SerializeField]
    private GameObject DocumentZone;

    [SerializeField]
    private GameObject TreshZone;

    void Awake()
    {
       
    }
    public void Start0()
    {
        timer = new Timer(initTime);
    }
    public GameObject GetAuthorList()
    {
        return paperManager.authorList;
    }

    public GameObject GetAcademyList()
    {
        return paperManager.academyList;
    }
    public GameObject GetInstitutonList()
    {
        return paperManager.institutonList;
    }

    public void GenPaper(Transform genPos)
    {
        onTablePaperObj = paperManager.GetPaperObj();
        onTablePaperObj.GetComponent<Paper>().MoveTransform(genPos, 0.7f);
    }

    public bool IsGameOver()
    {
        if (timer.GetTime() <= 0)
        {
            DestroyPaper();
            paperManager.End();
            return true;
        }
        else
            return false;
    }

    private void DestroyPaper()
    {
        Destroy(onTablePaperObj);
    }

}
