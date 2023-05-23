using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum TURN { PLAYER, AI, NONE }

public enum ACTION { HIT, STAND, NOT }

public class BlackJackState
{

    public int playerSum { get; set; }
    public int playerUpCardSum { get; set; }
    public int AISum { get; set; }
    public int AIUpCardSum { get; set; }
    public bool isGameOver { get; set; }
    public TURN whosTurn { get; set; }
    public int round { get; set; }
    public bool brust { get; set; }
    public ACTION playerAction { get; set; } = ACTION.NOT;
    public ACTION AIAction { get; set; } = ACTION.NOT;
    public TURN whoBurst { get; set; }

    public BlackJackState()
    {
        playerSum = 0;

        AISum = 0;

        isGameOver = false;

        whosTurn = TURN.PLAYER;
    }

    public void NextRound()
    {
        this.playerSum = 0;

        this.AISum = 0;

        this.AIUpCardSum = 0;

        this.playerUpCardSum = 0;

        this.whosTurn = TURN.PLAYER;

        this.round++;
    }

}
