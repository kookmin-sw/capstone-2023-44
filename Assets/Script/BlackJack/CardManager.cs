using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

using static SYMBOL;
using static TURN;

public class CardManager : MonoBehaviour
{

    private const int C_TOTALCARDS = 52;
    private const int C_JQK = 10;

    public int ca;

    private Stack<Card> cards;
    [SerializeField]
    public List<GameObject> playerCards { get; private set; } = new List<GameObject>();
    [SerializeField]
    public List<GameObject> AICards { get; private set; } = new List<GameObject>();

    [SerializeField]
    private List<Sprite> frontSprites;

    [SerializeField]
    private List<Sprite> backSprites;

    [SerializeField]
    private GameObject cardPrefabs;

    [SerializeField]
    private Transform cardSpawnPoint;

    [SerializeField]
    private Transform playerPlace;

    [SerializeField]
    private Transform AIPlace;

    public void MakeDeck()
    {

        cards = new Stack<Card>();

        int backColor = 0;

        for(int sym = 0; sym < 4; ++sym)
        {
            //sym이 0~1이면 0. 2~4면 1로 만들어 앞 뒷면 index로 사용
            backColor /= 2;

            for(int num = 0; num < 13; ++num)
            {

                Card card;

                if (num >= 10)
                    card = new Card(frontSprites[num + (13 * sym)], backSprites[backColor], C_JQK, (SYMBOL)sym);
                else
                    card = new Card(frontSprites[num + (13 * sym)], backSprites[backColor], num + 1, (SYMBOL)sym);

                cards.Push(card);

            }

        }

        //for debug
        ca = cards.Count;

    }

    public void Shuffle()
    {

        List<Card> shuffleSpace = new List<Card>(cards);

        System.Random random = new System.Random();

        for (int i = shuffleSpace.Count - 1; i > 0; i--)
        {

            int j = random.Next(i + 1);

            Card tmp = shuffleSpace[i];

            shuffleSpace[i] = shuffleSpace[j];

            shuffleSpace[j] = tmp;

        }

        cards = new Stack<Card>(shuffleSpace);

    }

    public void OrderCard(Card card, bool isFront, TURN turn)
    {
        MakeCardObject(card, isFront, turn);
    }

    public void MakeCardObject(Card card, bool isFront, TURN turn)
    {
        var cardObject = Instantiate(cardPrefabs, cardSpawnPoint.position, Quaternion.identity);

        if (isFront)
        {
            cardObject.GetComponent<SpriteRenderer>().sprite = card.CardFront;
            cardObject.GetComponent<Card>().CardFront = card.CardFront;
            cardObject.GetComponent<Card>().CardBack = card.CardBack;
        }
        else
        {
            cardObject.GetComponent<SpriteRenderer>().sprite = card.CardBack;
            cardObject.GetComponent<Card>().CardBack = card.CardBack;
            cardObject.GetComponent<Card>().CardFront = card.CardFront;
        }

        cardObject.GetComponent<Card>().CardNumber = card.CardNumber;
        cardObject.GetComponent<Card>().CardSymbol = card.CardSymbol;
        cardObject.GetComponent<Card>().isFront = isFront;
        cardObject.GetComponent<Card>().whosCard = turn;
        cardObject.GetComponent<Card>().isTargetFlip = false;

        switch (turn)
        {
            case PLAYER:
                playerCards.Add(cardObject);
                cardObject.transform.SetParent(playerPlace);
                break;

            case AI:
                AICards.Add(cardObject);
                cardObject.transform.SetParent(AIPlace);
                break;

            default:
                break;
        }

    }

    public void DestroyAllCard()
    {
        for(int idx = 0; idx < AICards.Count; ++idx)
        {
            Destroy(AICards[idx]);
        }

        for (int idx = 0; idx < playerCards.Count; ++idx)
        {
            Destroy(playerCards[idx]);
        }
    }

    public int GetDeckNum()
    {
        return cards.Count;
    }

    public Card GetCard()
    {
        ca -= 1;
        return cards.Pop();
    }

}
