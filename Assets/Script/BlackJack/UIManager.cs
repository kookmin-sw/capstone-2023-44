using System;
using System.Threading.Tasks;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;
using TMPro;

using static TURN;

public class UIManager : MonoBehaviour
{ 
    private float playerPoint;
    private float AIPoint;

    private const int C_START_CARD_NUMBER = 2;

    private WaitForSeconds delay = new WaitForSeconds(0.5f);

    public bool isPlayerAction { get; set; } = false;

    public ExtendPRS AIStartPos { get; set; }

    public ExtendPRS playerStartPos { get; set; }

    public ExtendPRS AIEndPos { get; set; }

    public ExtendPRS playerEndPos { get; set; }

    [SerializeField]
    private Button hit;

    [SerializeField]
    private Button Stand;

    [SerializeField]
    private Canvas canvas;

    [SerializeField]
    private CardManager cardManager;

    [SerializeField]
    private TextMeshProUGUI playerSum;

    [SerializeField]
    private TextMeshProUGUI AISum;

    [SerializeField]
    private TextMeshProUGUI systemText;

    [SerializeField]
    private TextMeshProUGUI AIDecisionText;

    [SerializeField]
    private TextMeshProUGUI winnerText;

    [SerializeField]
    private Transform AIStartPosition;

    [SerializeField]
    private Transform playerStartPosition;

    [SerializeField]
    private Transform AIEndPosition;

    [SerializeField]
    private Transform playerEndPosition;

    [SerializeField]
    private BlackJackManager blackJackManager;

    [SerializeField]
    private GameObject GameOver;

    void Awake()
    {
        //button
        hit.onClick.AddListener(() => DoHit());

        systemText.gameObject.SetActive(false);

        //player card position init
        playerStartPos = new ExtendPRS(playerStartPosition.position, playerStartPosition.rotation, playerStartPosition.localScale);
        playerEndPos = new ExtendPRS(playerEndPosition.position, playerEndPosition.rotation, playerEndPosition.localScale);

        playerPoint = playerStartPos.pos.x;

        //ai card position init
        AIStartPos = new ExtendPRS(AIStartPosition.position, AIStartPosition.rotation, AIStartPosition.localScale);
        AIEndPos = new ExtendPRS(AIEndPosition.position, AIEndPosition.rotation, AIEndPosition.localScale);

        AIPoint = AIStartPos.pos.x;
    }

    public IEnumerator AIDoAction()
    {
        if (blackJackManager.AIDecisionAction())
        {
            //yield return delay;
            DoHit();
            //yield return delay;
            AIDecisionText.text = "AI Hit!!";
            //yield return delay;
            isPlayerAction = false;
        }
        else
        {
            //yield return delay;
            DoStand();
            //yield return delay;
            AIDecisionText.text = "AI Stand!!";
            //yield return delay;
            isPlayerAction = false;
        }
        isPlayerAction = false;
        yield return null;
    }

    public void DoHit()
    {
        isPlayerAction = true;

        if (blackJackManager.IsGameOver())
        {
            blackJackManager.EndGame();
            GameOver.gameObject.SetActive(true);
        }

        TURN turn = blackJackManager.GetWhosTurn();

        blackJackManager.OffTargetFlip(turn);
        blackJackManager.Hit();
        StartCoroutine(SortCard(turn));

        //List<GameObject> cards;

        //if (turn == PLAYER)
        //{
        //    cards = cardManager.playerCards;
        //    blackJackManager.whatPlayerDoAction = true;
        //}
        //else
        //{
        //    cards = cardManager.AICards;
        //}

        List<GameObject> cards = turn == PLAYER ? cardManager.playerCards : cardManager.AICards;

        if (cardManager.playerCards.Count == C_START_CARD_NUMBER + 1 || cardManager.AICards.Count == C_START_CARD_NUMBER + 1)
            FlipCard(cards);

    }

    public void DoStand()
    {
        if (blackJackManager.GetWhosTurn() == PLAYER)
        {
            isPlayerAction = true;
        }
        else
        {
            isPlayerAction = false;
        }

        if (cardManager.AICards.Count == 3)
            FlipCard(cardManager.AICards);

        else if (cardManager.playerCards.Count == 3)
            FlipCard(cardManager.playerCards);

        blackJackManager.Stand();
    }

    public void BlackJack()
    {
        var aiCards = cardManager.AICards;
        var playerCards = cardManager.playerCards;

        //draw
        if(blackJackManager.IsBlackJack(playerCards) && blackJackManager.IsBlackJack(aiCards))
        {
            systemText.gameObject.SetActive(true);
            systemText.text = "Draw!!";
            Invoke("OffSystemMessage", 1.5f);
        }
        else if(blackJackManager.IsBlackJack(playerCards))
        {
            systemText.gameObject.SetActive(true);
            systemText.text = "Player is BlackJack!!, Winner winner chicken dinner!!";
            Invoke("OffSystemMessage", 1.5f);
        }
        else if(blackJackManager.IsBlackJack(playerCards))
        {
            systemText.gameObject.SetActive(true);
            systemText.text = "AI is BlackJack!!, Winner winner chicken dinner!!";
            Invoke("OffSystemMessage", 1.5f);
        }

        FlipCard(aiCards);
        FlipCard(playerCards);
    }

    public IEnumerator SetWinner()
    {
        yield return StartCoroutine(WhoWinner());
    }

    public IEnumerator WhoWinner()
    {
        isPlayerAction = false;

        systemText.gameObject.SetActive(true);

        TURN winner = blackJackManager.FindWinner();

        if(winner == PLAYER)
        {
            systemText.text = "Player WIN!!";
        }
        else if(winner == AI)
        {
            systemText.text = "AI WIN!";
        }
        else
        {
            systemText.text = "Draw!";
        }

        Invoke("OffSystemMessage", 1.5f);

        blackJackManager.UpdateStudyPoint(winner);

        blackJackManager.ReGame();

        isPlayerAction = false;

        UIinit();

        yield return null;
    }

    public void OffSystemMessage()
    {
        systemText.gameObject.SetActive(false);
    }

    public void FlipCard(List<GameObject> cards)
    {
        var targetCard = cards[cards.Count - 2].GetComponent<Card>();

        if (targetCard.isFront)
            return;
        else
            targetCard.Rotate();
    }

    public IEnumerator Burst(TURN burstInfo)
    {
        systemText.gameObject.SetActive(true);

        switch (burstInfo)
        {
            case PLAYER:
                systemText.text = "Player Brust!!";
                break;

            case AI:
                systemText.text = "AI Brust!!";
                break;

            default:
                systemText.text = "Draw!!";
                break;
        }

        Invoke("OffSystemMessage", 1.5f);

        blackJackManager.UpdateStudyPoint(burstInfo);
        blackJackManager.ReGame();
        isPlayerAction = false;
        UIinit();

        yield return null;
        //TURN turn = blackJackManager.GetWhosTurn();
        //systemText.gameObject.SetActive(true);
        //systemText.text = turn == PLAYER ? "Player Brust!!" : "AI Brust!!";
        //blackJackManager.UpdateStudyPoint(turn);
        //blackJackManager.ReGame();
        //Invoke("OffSystemMessage", 1.5f);
        //isPlayerAction = false;
        //UIinit();
    }

    public void UIinit()
    {
        blackJackManager.OrderStartCard();
        StartCoroutine(SortStartCard());
        UpdatePlayerSum();
        InitAISum();
        BlackJack();
        isPlayerAction = false;
    }

    public IEnumerator UpdateUI()
    {
        if (blackJackManager.IsBurst())
        {
            yield return Burst(blackJackManager.GetBurstInfo());
        }
        else
        {
            UpdatePlayerSum();
            UpdateAISum();
        }
    }

    public IEnumerator SortStartCard()
    {
        yield return StartCoroutine(SortCard(PLAYER));
        yield return StartCoroutine(SortCard(AI));
    }

    private void UpdatePlayerSum()
    {
        playerSum.text = "Player Sum = " + blackJackManager.GetPlayerSum();
    }

    private void UpdateAISum()
    {
        AISum.text = "AI Sum = " + blackJackManager.GetAICardSum();
    }

    private void InitAISum()
    {
        AISum.text = "AI Sum = " + blackJackManager.GetAIUpCardSum();
    }

    public IEnumerator SortCard(TURN turn)
    {
        var gamerInfo = FindGamerInfo(turn);

        List<GameObject> cards = gamerInfo.Item1;
        Tuple<ExtendPRS, ExtendPRS> posInfo = gamerInfo.Item2;

        for (int idx = 1; idx <= cards.Count; ++idx)
        {
            yield return delay;
            cards[idx - 1].GetComponent<Card>().MoveTransform(GetCardPos(posInfo.Item1, posInfo.Item2, cards.Count + 1, idx), 0.7f);
        }

        //blackJackManager.ChangeTurn();

    }

    public Tuple<List<GameObject>, Tuple<ExtendPRS, ExtendPRS>> FindGamerInfo(TURN turn)
    {
        List<GameObject> cards;
        Tuple<ExtendPRS, ExtendPRS> pos;

        if (turn == PLAYER)
        {
            cards = cardManager.playerCards;
            pos = new Tuple<ExtendPRS, ExtendPRS>(playerStartPos, playerEndPos);
        }
        else
        {
            cards = cardManager.AICards;
            pos = new Tuple<ExtendPRS, ExtendPRS>(AIStartPos, AIEndPos);
        }

        return new Tuple<List<GameObject>, Tuple<ExtendPRS, ExtendPRS>>(cards, pos);
    }

    public ExtendPRS GetCardPos(ExtendPRS start, ExtendPRS end, float cardNum, float count)
    {
        var x = LinearInterpolation(start.pos.x, end.pos.x, (float)(count / cardNum));
        return new ExtendPRS(new Vector3(x, start.pos.y, start.pos.z), start.rot, start.scale);
    }

    float LinearInterpolation(float p1, float p2, float d1)
    {
        return (1 - d1) * p1 + d1 * p2;
    }

}
