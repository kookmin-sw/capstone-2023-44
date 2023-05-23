using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class Strategy
{

    public int AITotal { get; set; }
    public int PlayerUpCard { get; set; }

    // <action, prob>
    // stand/hit table, �� ���� �´� �ൿ�� ��¥�� �� �� Ȯ��.
    // �� = �� ī��, �� = ��� ī��
    // ���̵����� �ִ� ���� ��ġ�� �ٸ����ؼ� ������ �����ϰ� ����.
    public abstract bool Thinking();
}

public class BasicStrategy : Strategy
{
    public override bool Thinking()
    {
        return ShouldHit(AITotal, PlayerUpCard);
    }

    public bool ShouldHit(int aiTotal, int playerUpCard)
    {
        // �÷��̾��� ���� ����Ʈ 17�� ���
        if (aiTotal == 17 && HasAce(aiTotal))
        {
            return true;
        }

        // ������ �� ī�尡 7 �̻��� ���
        if (playerUpCard >= 7)
        {
            if (aiTotal <= 11)
            {
                return true;
            }
            if (aiTotal == 12 && (playerUpCard == 2 || playerUpCard == 3))
            {
                return true;
            }
            if (aiTotal <= 15 && playerUpCard == 10)
            {
                return true;
            }
            if (aiTotal <= 16 && playerUpCard == 9)
            {
                return true;
            }
        }

        // ������ �� ī�尡 4, 5, 6�� ���
        if (playerUpCard >= 4 && playerUpCard <= 6)
        {
            if (aiTotal <= 11)
            {
                return true;
            }
            if (aiTotal == 12 && (playerUpCard == 2 || playerUpCard == 3))
            {
                return true;
            }
            if (aiTotal <= 15 && (playerUpCard == 10 || playerUpCard == 11))
            {
                return true;
            }
            if (aiTotal <= 16 && playerUpCard == 9)
            {
                return true;
            }
        }

        // �� ���� ���
        if (aiTotal <= 11)
        {
            return true;
        }
        if (aiTotal == 12 && playerUpCard <= 3)
        {
            return true;
        }
        if (aiTotal <= 16 && playerUpCard >= 7)
        {
            return true;
        }

        return false;
    }

    public bool HasAce(int total)
    {
        foreach (var card in Hand(total))
        {
            if (card == 1)
            {
                return true;
            }
        }
        return false;
    }

    public IEnumerable<int> Hand(int total)
    {
        // �տ� �ִ� �� ī���� ���� ��ȯ�ϴ� �Լ�
        // (A�� 1 �Ǵ� 11�� ����� �� �����Ƿ� ����Ʈ�� ��ȯ)
        if (total <= 11)
        {
            yield return 1;
            yield return total - 1;
        }
        else if (total == 12)
        {
            yield return 1;
            yield return 11;
        }
        else
        {
            yield return total - 10;
        }
    }
}

//�� �ڵ带 ���ӿ� �����ϱ� ���ؼ���, ī�带 ���� ������ UpdateCount �Լ��� ȣ���Ͽ� ī��Ʈ�� ������Ʈ�ϸ� �˴ϴ�.
//ī��Ʈ�� ���� �� �̻��̸� �÷��̾ �߰��� ������ �� �ִ� ��ȸ�� �����ϰ�, ���� �� ���ϸ� �÷��̾ ������ ���̵��� ������ �� �ֽ��ϴ�.

public class BeginnerCardCounting : Strategy
{
    private int count;
    public override bool Thinking()
    {
        return true;
    }

    public void UpdateCount(Card card)
    {
        if (card.CardNumber >= 2 && card.CardNumber <= 6) // 2, 3, 4, 5, 6 ī�尡 ������ ��
            count++;
        else if (card.CardNumber >= 10 || card.CardNumber == 1) // 10, J, Q, K, A ī�尡 ������ ��
            count--;
    }

    public int GetCount()
    {
        return count;
    }

    public void ResetCount()
    {
        count = 0;
    }
}
