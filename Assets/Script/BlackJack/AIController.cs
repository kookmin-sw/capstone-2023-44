using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum INTELLIGENCE{ HIGH, MEDIUM, LOW }

public class AIController : MonoBehaviour
{
    // hit / stand를 판단하고
    // hit, stand를 string으로 리턴하여
    // 매니저로 넘겨주자.
    private INTELLIGENCE intelligence;

    private Strategy strategy;

    public bool whatAIDoAction { get; private set; }

    [SerializeField]
    private CardManager cardManager;

    // 디버깅용 인스펙터 입력. 삭제 예정 멤버.
    [SerializeField]
    private int intelligencePoint;

    private void Awake()
    {
        SettingIntelligence();
    }

    public void SettingIntelligence()//(int intelligencePoint)
    {
        if(intelligencePoint > 70)
        {
            intelligence = INTELLIGENCE.HIGH;
        }
        else if(intelligencePoint <= 70 && intelligencePoint > 30)
        {
            intelligence = INTELLIGENCE.MEDIUM;
            //strategy = (BeginnerCardCounting);
        }
        else if(intelligencePoint <= 30)
        {
            intelligence = INTELLIGENCE.LOW;
            strategy = new BasicStrategy();
        }
    }

    //HIT할지 STAND할지 생각
    public bool Thinking(int aiTotal, int playerUpCard)
    {
        strategy.AITotal = aiTotal;
        strategy.PlayerUpCard = playerUpCard;
        
        //hit하면 true stand하면 false
        whatAIDoAction = strategy.Thinking();

        return whatAIDoAction;
    }

}
