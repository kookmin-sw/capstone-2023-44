using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.UI;

using static TYPE;

public class PaperManager : MonoBehaviour
{

    private Stack<Paper> papers = new();

    public GameObject authorList { get; private set; }
    public GameObject academyList { get; private set; }
    public GameObject institutonList { get; private set; }

    public int fordebugStackCount;

    public List<Paper> theses { get; private set; }

    public List<Paper> documents { get; private set; }

    [SerializeField]
    private Canvas desk;

    [SerializeField]
    private XMLManager DataManager;

    [SerializeField]
    private GameObject ThesisPrefabs;

    [SerializeField]
    private GameObject DocumentPrefabs;

    [SerializeField]
    private GameObject ListPaperPrefabs;

    public GameObject WorkGameMain;
    public GameObject WorkGameStartScene;
    /*void Awake()
    {
        LoadThesisData();
        LoadDocuData();
        MakePaperStack(theses);
        MakePaperStack(documents);
        Shuffle();
        MakeAuthorList();
        MakeAcademyList();
        MakeInstitutonList();
    }*/
    public void Setup()
    {
        WorkGameStartScene.SetActive(false);
        WorkGameMain.SetActive(true);
    }
    public void End()
    {
        WorkGameStartScene.SetActive(true);
        WorkGameMain.SetActive(false);
    }
    public void Start11()
    {
        LoadThesisData();
        LoadDocuData();
        MakePaperStack(theses);
        MakePaperStack(documents);
        Shuffle();
        MakeAuthorList();
        MakeAcademyList();
        MakeInstitutonList();
    }
    private void LoadThesisData()
    {
        theses = new List<Paper>(DataManager.LoadThesisXML());
    }

    private void LoadDocuData()
    {
        documents = new List<Paper>(DataManager.LoadDocuXML());
    }

    private void MakePaperStack(List<Paper> paperList)
    {
        foreach(Paper paper in paperList)
        {
            papers.Push(paper);
        }
    }

    private void MakeAuthorList()
    {
        AuthorList authorL = new AuthorList();

        GameObject authors = Instantiate(ListPaperPrefabs, this.transform.position, Quaternion.identity);

        authors.transform.GetChild(1).GetComponent<TextMeshProUGUI>().text = authorL.MakePaper();

        authors.transform.SetParent(desk.transform);
        authors.transform.localScale = new Vector3(6f, 6f, 6f);

        authorList = authors;
        authorList.AddComponent<AuthorList>();
    }

    private void MakeAcademyList()
    {
        AcademyList academyL = new AcademyList();

        GameObject academys = Instantiate(ListPaperPrefabs, this.transform.position, Quaternion.identity);

        academys.transform.GetChild(1).GetComponent<TextMeshProUGUI>().text = academyL.MakePaper();

        academys.transform.SetParent(desk.transform);
        academys.transform.localScale = new Vector3(6f, 6f, 6f);

        academyList = academys;
        academyList.AddComponent<AcademyList>();
    }

    private void MakeInstitutonList()
    {
        InstitutonList institutonL = new InstitutonList();

        GameObject institutons = Instantiate(ListPaperPrefabs, this.transform.position, Quaternion.identity);

        institutons.transform.GetChild(1).GetComponent<TextMeshProUGUI>().text = institutonL.MakePaper();

        institutons.transform.SetParent(desk.transform);
        institutons.transform.localScale = new Vector3(6f, 6f, 6f);

        institutonList = institutons;
        institutons.AddComponent<InstitutonList>();
    }

    private void Shuffle()
    {

        List<Paper> shuffleSpace = new List<Paper>(papers);

        System.Random random = new System.Random();

        for (int i = shuffleSpace.Count - 1; i > 0; i--)
        {

            int j = random.Next(i + 1);

            Paper tmp = shuffleSpace[i];

            shuffleSpace[i] = shuffleSpace[j];

            shuffleSpace[j] = tmp;

        }

        papers = new Stack<Paper>(shuffleSpace);

    }

    private void MakeThesis()
    {

    }

    private void MakeDocument()
    {

    }

    public GameObject GetPaperObj()
    {
        Paper paper = GetPaper();
        GameObject paperObj = new();

        if (paper is Thesis)
        {
            Thesis convPaper = paper as Thesis;
            paperObj = Instantiate(ThesisPrefabs);
            paperObj.GetComponent<Paper>().isValidPaper = paper.isValidPaper;
            paperObj.GetComponent<Paper>().paperSprite = paper.paperSprite;
            paperObj.GetComponent<Paper>().paperType = THESIS;
            paperObj.GetComponent<Thesis>().author = convPaper.author;
            paperObj.GetComponent<Thesis>().academy = convPaper.academy;
        }
        else if (paper is Document)
        {
            Document convPaper = paper as Document;
            paperObj = Instantiate(DocumentPrefabs);
            paperObj.GetComponent<Paper>().isValidPaper = paper.isValidPaper;
            paperObj.GetComponent<Paper>().paperSprite = paper.paperSprite;
            paperObj.GetComponent<Paper>().paperType = DOCUMENT;
            paperObj.GetComponent<Document>().institution = convPaper.institution;
            paperObj.GetComponent<Document>().stampFilePath = convPaper.stampFilePath;
        }

        paperObj.transform.SetParent(desk.transform);
        paperObj.transform.localScale = new Vector3(6f, 6f, 6f);

        return paperObj;
    }

    private Paper GetPaper()
    {
        fordebugStackCount = papers.Count;
        return papers.Pop();
    }

}
