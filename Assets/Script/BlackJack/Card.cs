using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;

using static TURN;

public enum SYMBOL { CLOVER, SPADE, HEART, DIAMOND }
public class Card : MonoBehaviour
{
    public TURN whosCard { get; set; }
    public Sprite CardFront { get; set; }
    public Sprite CardBack { get; set; }
    public int CardNumber { get; set; }
    public SYMBOL CardSymbol { get; set; }
    public bool isFront { get; set; } = false;
    public bool isTargetFlip { get; set; } = false;
    public bool isRotating { get; set; } = false;

    public Card(Sprite frt, Sprite bck, int num, SYMBOL sym)
    {

        this.CardFront = frt;
        this.CardBack = bck;
        this.CardNumber = num;
        this.CardSymbol = sym;

    }

    public void MoveTransform(ExtendPRS ExtendPRS, float dotweenTime = 0f)
    {
        transform.DOMove(ExtendPRS.pos, dotweenTime);
        transform.DORotateQuaternion(ExtendPRS.rot, dotweenTime);
    }

    public void Rotate()
    {
        isFront = !isFront;

        var seq = DOTween.Sequence();
        seq.Append(this.transform.DORotate(this.transform.eulerAngles + new Vector3(0, 90, 0), 0.25f));
        seq.AppendCallback(() => { this.GetComponent<SpriteRenderer>().sprite = (this.transform.eulerAngles.y < 180) ? this.GetComponent<Card>().CardFront : this.GetComponent<Card>().CardBack; });
        seq.Append(this.transform.DORotate(this.transform.eulerAngles + new Vector3(0, 180, 0), 0.25f));
    }

}