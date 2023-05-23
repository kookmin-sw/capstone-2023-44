using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using static TURN;
using static ACTION;

public class BlackJackManager : MonoBehaviour
{

    private BlackJackState GameState;

    public bool whatPlayerDoAction { get; set; }

    private const int C_START_CARD_NUMBER = 2;

    public static int studyPoint { get; set; }

    private ExtendPRS origin = new ExtendPRS(Vector3.zero, Quaternion.identity, new Vector3(3, 3, 3));

    private WaitForSeconds delay = new WaitForSeconds(0.5f);

    [SerializeField]
    private CardManager cardManager;

    [SerializeField]
    private AIController ai;
    [SerializeField]
    private RenderingManager RenderManager;

    public GameObject BlackJack_GameObject;
    public GameObject BlackJack_BG;
    public GameObject BlackJack_Popup;
    public ScriptManager SManager;
    void Awake()
    {
       
    }
    public void StartGame() // Awake에있던거 Button화 시키기 위해 함수로 묶음
    {
        string activeSceneName = SceneManager.GetActiveScene().name;
        if (activeSceneName == "AIScene")
        {
            SManager.AITutorialPanel.SetActive(false);

        }
        BlackJack_BG.SetActive(false);
        BlackJack_GameObject.SetActive(true);
        cardManager.MakeDeck();

        cardManager.Shuffle();

        GameState = new BlackJackState();

        RenderManager.UISetting();

    }
    public void EndGame()
    {
        ReGame();
        BlackJack_GameObject.SetActive(false);
        BlackJack_BG.SetActive(true);
        if(SManager.script_Status == "story")
        {

        }
        else if(SManager.script_Status == "move")
        {
            SManager.text.text = "";
            SManager.moveIndex++;
            BlackJack_Popup.SetActive(false);
            SManager.ScriptPanel.SetActive(true);
        }


    }
    public void ReGame()
    {
        //GameState.NextRound();
        GameState = null;
        GC.Collect();
        GameState = new BlackJackState();

        cardManager.DestroyAllCard();

        cardManager.AICards.Clear();
        cardManager.playerCards.Clear();
    }

    public void UpdateStudyPoint(TURN turn)
    {
        //플레이어 버스트일 시 3 증가.
        studyPoint += turn == PLAYER ? 3 : 1;
    }

    //에이아이용 메서드.
    public bool AIDecisionAction()
    {
        UpdatePlayerUpCardSum();

        return ai.Thinking(GameState.AISum, GameState.playerUpCardSum);
    }

    public void Hit()
    {
        var card = cardManager.GetCard();
        TURN turn = GetWhosTurn();

        cardManager.OrderCard(card, true, turn);

        switch (turn)
        {

            case PLAYER:
                GameState.playerSum += card.CardNumber;
                GameState.playerAction = HIT;
                break;

            case AI:
                GameState.AISum += card.CardNumber;
                GameState.AIAction = HIT;
                break;

            default:
                break;

        }
        ChangeTurn();
    }

    //overloading for start card
    public void Hit(bool isFornt)
    {
        var card = cardManager.GetCard();
        TURN turn = GetWhosTurn();

        cardManager.OrderCard(card, isFornt, turn);

        switch (turn)
        {
            case PLAYER:
                GameState.playerSum += card.CardNumber;
                GameState.playerAction = HIT;
                break;

            case AI:
                GameState.AISum += card.CardNumber;
                GameState.AIAction = HIT;
                break;

            default:
                break;

            }
    }

    public void Stand()
    {
        TURN turn = GetWhosTurn();

        if(turn == PLAYER)
        {
            GameState.playerAction = STAND;
        }
        else
        {
            GameState.AIAction = STAND;
        }

        ChangeTurn();
    }

    public void OrderStartCard()
    {
        for (int count = 0; count < C_START_CARD_NUMBER; ++count)
        {

            if (count == C_START_CARD_NUMBER - 1)
            {
                Hit(false);
                OnTargetFlip(PLAYER);
                ChangeTurn();
                Hit(false);
                ChangeTurn();
            }
            else
            {
                Hit(true);
                ChangeTurn();
                Hit(true);
                ChangeTurn();
            }
        }
    }

    public TURN GetWhosTurn()
    {
        return GameState.whosTurn;
    }

    // AI가 오픈된 플레이어 카드를 보고 판단하기 위해 오픈된 카드를 계산
    public void UpdatePlayerUpCardSum()
    {
        GameState.playerUpCardSum = 0;

        for (int idx = 0; idx < cardManager.playerCards.Count; ++idx)
        {
            var card = cardManager.playerCards[idx].GetComponent<Card>();

            if (card.isFront)
                GameState.playerUpCardSum += card.CardNumber;
            else
                continue;
        }
    }

    //UI에 업데이트 하기 위한 모든 플레이어 카드를 계산.
    public void UpdatePlayerCardSum()
    {
        GameState.playerUpCardSum = 0;

        for (int idx = 0; idx < cardManager.playerCards.Count; ++idx)
        {
            GameObject card = cardManager.playerCards[idx];
            GameState.playerUpCardSum += card.GetComponent<Card>().CardNumber;
        }
    }

    ////UI 업데이트를 위한 오픈된 AI의 카드를 계산.
    public void UpdateAIUpCardSum()
    {
        GameState.AIUpCardSum = 0;

        for (int idx = 0; idx < cardManager.AICards.Count; ++idx)
        {
            var card = cardManager.AICards[idx].GetComponent<Card>();

            if (card.isFront)
                GameState.AIUpCardSum += card.CardNumber;
            else
                continue;
        }
    }

    //AI가 판단하기 위한 AI의 모든 카드를 계산.
    public void UpdateAICardSum()
    {
        GameState.AISum = 0;

        for (int idx = 0; idx < cardManager.AICards.Count; ++idx)
        {
            GameObject card = cardManager.AICards[idx];
            GameState.AISum += card.GetComponent<Card>().CardNumber;
        }
    }

    public void OnTargetFlip(TURN turn)
    {
        if (turn == AI)
            return;

        int endIdx = cardManager.playerCards.Count - 1;
        cardManager.playerCards[endIdx].GetComponent<Card>().isTargetFlip = true;
    }

    //player가 hit해서 직전에 받은 카드를 뒤집지 못하게 막음.
    public void OffTargetFlip(TURN turn)
    {
        if (turn == AI)
            return;

        int endIdx = cardManager.playerCards.Count - 1;
        cardManager.playerCards[endIdx].GetComponent<Card>().isTargetFlip = false;
    }

    public bool IsBlackJack(List<GameObject> cards)
    {
        int sum = 0;
        
        foreach(GameObject card in cards)
        {
            sum += card.GetComponent<Card>().CardNumber;
        }

        return (sum == 11);
    }

    public int GetPlayerSum()
    {
        UpdatePlayerCardSum();
        return GameState.playerSum;
    }

    public int GetAICardSum()
    {
        UpdateAICardSum();
        return GameState.AISum;
    }

    public int GetAIUpCardSum()
    {
        UpdateAIUpCardSum();
        return GameState.AIUpCardSum;
    }

    public void ChangeTurn()
    {
       GameState.whosTurn = (GameState.whosTurn == PLAYER) ?  AI : PLAYER;
    }

    public bool IsBurst()
    {
        UpdateAICardSum();
        UpdatePlayerCardSum();

        if (GameState.playerSum > 21 || GameState.AISum > 21)
        {
            if (GameState.playerSum > 21 && GameState.AISum > 21)
            {
                GameState.whoBurst = NONE;
            }
            else if (GameState.playerSum > 21)
            {
                GameState.whoBurst = PLAYER;
            }
            else
            {
                GameState.whoBurst = AI;
            }

            return true;
        }
        else
            return false;
    }

    public ACTION GetPlayerAction()
    {
        return GameState.playerAction;
    }

    public ACTION GetAIAction()
    {
        return GameState.AIAction;
    }

    public TURN GetBurstInfo()
    {
        return GameState.whoBurst;
    }

    public bool IsGameOver()
    {
        return cardManager.GetDeckNum() == 0;
    }

    public TURN FindWinner()
    {
        UpdateAICardSum();
        UpdatePlayerCardSum();

        if (GameState.playerSum > GameState.AISum)
            return PLAYER;
        else if (GameState.playerSum < GameState.AISum)
            return AI;
        else
            return NONE;
    }

}
