using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class WorkUIManager : MonoBehaviour
{
    public int money { get; set; } = 0;

    private int C_INIT_MONEY = 0;

    private bool isStampBookCalled = false;
    private bool isAuthorListCalled = false;
    private bool isAcademyListCalled = false;
    private bool isInstitutonListCalled = false;

    [SerializeField]
    private TMP_Text timer;

    [SerializeField]
    private TextMeshProUGUI makeMoney;

    [SerializeField]
    private GameObject GameOverUI;

    [SerializeField]
    private Transform paperGenPosition;

    [SerializeField]
    private WorkManager workManager;

    [SerializeField]
    private Canvas canvas;

    [SerializeField]
    private TextMeshProUGUI wageText;

    //List Button들과 해당되는 ListPaper들이 불러지면 이동될 위치.
    [SerializeField]
    private Button AuthorButton;

    [SerializeField]
    private GameObject AuthorPos;

    [SerializeField]
    private Button AcademyButton;

    [SerializeField]
    private GameObject AcademyPos;

    [SerializeField]
    private Button InstitutonButton;

    [SerializeField]
    private GameObject InstitutonPos;

    [SerializeField]
    private GameObject stampBook;

    [SerializeField]
    private Button StampBookButton;

    [SerializeField]
    private Transform StampBookPos;

    [SerializeField]
    private Transform StampBookReturnPos;

    [SerializeField]
    private Transform ListReturnPos;

    void Awake()
    {
        
    }
    public void Start2()
    {
        UpdateMoney(C_INIT_MONEY);
        UpdateTimer();
    }
    public void SetNextPaper()
    {
        workManager.GenPaper(paperGenPosition);
    }

    public void CallStampBook()
    {
        isStampBookCalled = !isStampBookCalled;

        if (isStampBookCalled)
        {
            stampBook.GetComponent<Book>().MoveTransform(StampBookPos, 0.7f);
        }
        else
        {
            stampBook.GetComponent<Book>().MoveTransform(StampBookReturnPos, 0.7f);
        }
    }
    public void CallAuthorList()
    {
        isAuthorListCalled = !isAuthorListCalled;

        if (isAuthorListCalled)
        {
            workManager.GetAuthorList().GetComponent<ListPaper>().MoveTransform(paperGenPosition, 0.7f);
        }
        else
        {
            workManager.GetAuthorList().GetComponent<ListPaper>().MoveTransform(ListReturnPos, 0.7f);
        }
    }
    public void CallAcademyList()
    {
        isAcademyListCalled = !isAcademyListCalled;

        if (isAcademyListCalled)
        {
            workManager.GetAcademyList().GetComponent<ListPaper>().MoveTransform(paperGenPosition, 0.7f);
        }
        else
        {
            workManager.GetAcademyList().GetComponent<ListPaper>().MoveTransform(ListReturnPos, 0.7f);
        }
    }
    public void CallInstitutonList()
    {
        isInstitutonListCalled = !isInstitutonListCalled;

        if (isInstitutonListCalled)
        {
            workManager.GetInstitutonList().GetComponent<ListPaper>().MoveTransform(paperGenPosition, 0.7f);
        }
        else
        {
            workManager.GetInstitutonList().GetComponent<ListPaper>().MoveTransform(ListReturnPos, 0.7f);
        }
    }

    public void GameOver()
    {
        if(workManager.IsGameOver())
        {
            ShowGameOverUI();
        }
    }

    private void ShowGameOverUI()
    {
        timer.gameObject.SetActive(false);
        GameOverUI.SetActive(true);

        wageText.text = "make money : " + workManager.wage;
    }
    
    public void UpdateMoney(int pay)
    {
        money += pay;
        wageText.text = "Wage : " + pay;
    }

    public void UpdateTimer()
    {
        timer.text = "Remaining time : " + workManager.timer.GetlimitTime();
    }

}
