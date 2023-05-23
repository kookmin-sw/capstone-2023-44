using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using DG.Tweening;

public enum LISTTYPE { AUTHOR, ACADEMY, INSTITUTION, STAMP };

public abstract class ListPaper : MonoBehaviour
{
    public LISTTYPE ListType;

    public GameObject listPaper;

    public PaperManager paperManager;

    public void MoveTransform(Transform pos, float dotweenTime = 0f)
    {
        transform.DOMove(pos.position, dotweenTime);
        transform.DORotateQuaternion(pos.rotation, dotweenTime);
    }

    public abstract string MakePaper();

    public abstract void LoadData();

}

public class AuthorList : ListPaper
{
    List<string> authorList = new();
    public AuthorList()
    {
        this.paperManager = GameObject.Find("PaperManager").GetComponent<PaperManager>();
        LoadData();
        this.ListType = LISTTYPE.AUTHOR;
    }

    public override string MakePaper()
    {
        string authors = "";

        foreach (string author in authorList)
        {
            authors += author + "\n";
        }

        return authors;
    }

    public override void LoadData()
    {
        List<Paper> thesisDatas = paperManager.GetComponent<PaperManager>().theses;

        foreach(Thesis thesisData in thesisDatas)
        {
            authorList.Add(thesisData.author);
        }
    }

}
public class AcademyList : ListPaper
{
    List<string> academyList = new();

    public AcademyList()
    {
        this.paperManager = GameObject.Find("PaperManager").GetComponent<PaperManager>();
        LoadData();
        this.ListType = LISTTYPE.ACADEMY;
    }

    public override string MakePaper()
    {
        string academies = "";

        foreach (string academy in academyList)
        {
            academies += academy + "\n";
        }

        return academies;
    }

    public override void LoadData()
    {
        List<Paper> thesisDatas = paperManager.GetComponent<PaperManager>().theses;

        foreach (Thesis thesisData in thesisDatas)
        {
            academyList.Add(thesisData.academy);
        }
    }

}
public class InstitutonList : ListPaper
{
    List<string> institutonList = new();

    public InstitutonList()
    {
        this.paperManager = GameObject.Find("PaperManager").GetComponent<PaperManager>();
        LoadData();
        this.ListType = LISTTYPE.INSTITUTION;
    }

    public override string MakePaper()
    {
        string institutons = "";

        foreach (string instituton in institutonList)
        {
            institutons += instituton + "\n";
        }

        return institutons;
    }

    public override void LoadData()
    {
        List<Paper> documentDatas = paperManager.GetComponent<PaperManager>().documents;

        foreach (Document documentData in documentDatas)
        {
            institutonList.Add(documentData.institution);
        }
    }

}

//public class StampList : ListPaper
//{
//    List<Sprite> stampList;

//    public StampList()
//    {
//        this.ListType = LISTTYPE.AUTHOR;
//    }

//    public override GameObject MakePaper()
//    {
//        return new GameObject();
//    }

//}