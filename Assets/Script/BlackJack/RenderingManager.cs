using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

using static TURN;
using static ACTION;

public class RenderingManager : MonoBehaviour
{

    [SerializeField]
    private BlackJackManager blackJackManager;

    [SerializeField]
    private UIManager UIManager;

    [SerializeField]
    private AIController ai;

    void Start()
    {
        
    }
    public void UISetting() // Start에 있던 함수들을 옮겨왔음 버튼을 눌러서 시작하고 제어하기 위해서
    {
        UIManager.UIinit();
        UIManager.BlackJack();

    }
    void Update()
    {
        //Debug.Log(blackJackManager.GetWhosTurn());

        if (Input.GetMouseButtonDown(0))
        {
            Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);

            if (Physics.Raycast(ray, out RaycastHit hitInfo))
            {
                Card targetCard = hitInfo.transform.gameObject.GetComponent<Card>();

                if (targetCard.isTargetFlip && !targetCard.isRotating)
                {
                    Utill.ToggleCollider(hitInfo.transform.gameObject);
                    //targetCard.GetComponent<Card>().Rotate();
                    targetCard.GetComponent<Card>().Rotate();
                }
                else
                {
                    Utill.ToggleCollider(hitInfo.transform.gameObject);
                }
            }

        }

        if (UIManager.isPlayerAction && blackJackManager.GetWhosTurn() == AI)
        {
            StartCoroutine(UIManager.AIDoAction());

            //if (blackJackManager.IsBurst())
            //{
            //    UIManager.Burst();
            //}

            StartCoroutine(UIManager.UpdateUI());

            if (blackJackManager.GetPlayerAction() == STAND && blackJackManager.GetAIAction() == STAND)
            {
                //UIManager.WhoWinner();
                StartCoroutine(UIManager.SetWinner());
            }

            UIManager.isPlayerAction = false;
        }
    }
}
