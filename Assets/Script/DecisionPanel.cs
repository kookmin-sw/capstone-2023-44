using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
public class DecisionPanel : MonoBehaviour
{
    public TextMeshProUGUI firstText;
    public TextMeshProUGUI secondText;
    public string choice_Result = "";
    public int choice_Index = 0;
    bool isClicked = false;
    public ScriptManager SManager;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }
    public void ClickFirstDecision() // ������ �׳� Ŭ���� ���������� ���⿡ ���� ���Ͻ��Ѽ� ���ÿ� ���� stat������ �����Ϸ��� ��.
    {
        choice_Result = "you choose 1";
        choice_Index = 0;
        isClicked = true;
        SManager.ChangeScript();
        SManager.moveList(SManager.storyIndex - 2);
        SManager.printScript();

    }
    public void ClickSecondDecision()
    {
        choice_Result = "you choose 2";
        choice_Index = 1;
        isClicked = true;
        SManager.ChangeScript();
        SManager.moveList(SManager.storyIndex - 1);
        SManager.printScript();

    }
    public void WaitForChoice()
    {
        StartCoroutine(WaitForChoice_IEnumerator());
    }

    IEnumerator WaitForChoice_IEnumerator()
    {
        while (!isClicked)
        {
            
            yield return new WaitForSeconds(0.04f);
        }
        isClicked = false;
        SManager.ScriptPanel.SetActive(true);
        gameObject.SetActive(false);
    }
    public void PrintDecisionResult()
    {
        //SManager.text.text = choice_Result;
        choice_Result = "";
    }
}
