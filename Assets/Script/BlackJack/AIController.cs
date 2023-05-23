using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum INTELLIGENCE{ HIGH, MEDIUM, LOW }

public class AIController : MonoBehaviour
{
    // hit / stand�� �Ǵ��ϰ�
    // hit, stand�� string���� �����Ͽ�
    // �Ŵ����� �Ѱ�����.
    private INTELLIGENCE intelligence;

    private Strategy strategy;

    public bool whatAIDoAction { get; private set; }

    [SerializeField]
    private CardManager cardManager;

    // ������ �ν����� �Է�. ���� ���� ���.
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

    //HIT���� STAND���� ����
    public bool Thinking(int aiTotal, int playerUpCard)
    {
        strategy.AITotal = aiTotal;
        strategy.PlayerUpCard = playerUpCard;
        
        //hit�ϸ� true stand�ϸ� false
        whatAIDoAction = strategy.Thinking();

        return whatAIDoAction;
    }

}
