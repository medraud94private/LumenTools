using System.Collections.Generic;
using UnityEngine;

public class CardManager : MonoBehaviour
{
    public List<AttackCard> attackCards;
    public List<DefenseCard> defenseCards;

    void Start()
    {
        // 예시 카드 데이터를 초기화합니다.
        InitializeCards();
    }

    void InitializeCards()
    {
        attackCards = new List<AttackCard>
        {
            new AttackCard
            {
                name = "Cutting",
                speed = 5,
                attackLine = AttackLine.High,
                attackPower = 400,
                limbType = "hand",
                specialCondition = "neutralize",
                hitFP = 2,
                guardFP = -3,
                counterFP = 2
            },
            // 다른 공격 카드를 추가합니다.
        };

        defenseCards = new List<DefenseCard>
        {
            new DefenseCard
            {
                name = "Standing Guard",
                highSlot = new DefenseSlot { slotType = SlotType.Defend, condition = "true" },
                midSlot = new DefenseSlot { slotType = SlotType.None, condition = "false" },
                lowSlot = new DefenseSlot { slotType = SlotType.None, condition = "false" },
                specialCondition = "Takes 200 damage when guarding."
            },
            // 다른 방어 카드를 추가합니다.
        };
    }

    public AttackCard GetAttackCard(string cardName)
    {
        return attackCards.Find(card => card.name == cardName);
    }

    public DefenseCard GetDefenseCard(string cardName)
    {
        return defenseCards.Find(card => card.name == cardName);
    }
}
