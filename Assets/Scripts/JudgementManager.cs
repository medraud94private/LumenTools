using System;
using System.Collections.Generic;
using UnityEngine;

public class JudgmentManager : MonoBehaviour
{
    public (int player1Damage, int player2Damage, int player1FP, int player2FP) ProcessJudgment(Player player1, Player player2)
    {
        int player1Damage = 0;
        int player2Damage = 0;
        int player1FP = player1.fp;
        int player2FP = player2.fp;

        // 1. 사용 시 효과 적용
        ApplyEffects(player1, player2, player1.attackCard, "use");
        ApplyEffects(player2, player1, player2.attackCard, "use");

        // 2. 판정 전 효과 적용
        ApplyEffects(player1, player2, player1.attackCard, "beforeJudgment");
        ApplyEffects(player2, player1, player2.attackCard, "beforeJudgment");

        // 3. 회피, 방어 확인 및 속도 비교
        if (player1.attackCard != null && player2.defenseCard != null)
        {
            (player1Damage, player2Damage, player1FP, player2FP) = HandleAttackVsDefense(player1.attackCard, player2.defenseCard, player1FP, player2FP);
        }
        else if (player1.defenseCard != null && player2.attackCard != null)
        {
            (player2Damage, player1Damage, player2FP, player1FP) = HandleAttackVsDefense(player2.attackCard, player1.defenseCard, player2FP, player1FP);
        }
        else if (player1.attackCard != null && player2.attackCard != null)
        {
            (player1Damage, player2Damage, player1FP, player2FP) = HandleAttackVsAttack(player1.attackCard, player2.attackCard, player1FP, player2FP);
        }
        else if (player1.defenseCard != null && player2.defenseCard != null)
        {
            (player1Damage, player2Damage, player1FP, player2FP) = HandleDefenseVsDefense(player1.defenseCard, player2.defenseCard, player1FP, player2FP);
        }

        // 4. 판정 시 효과 적용
        ApplyEffects(player1, player2, player1.attackCard, "duringJudgment");
        ApplyEffects(player2, player1, player2.attackCard, "duringJudgment");

        // 5. 판정 후 효과 적용
        ApplyEffects(player1, player2, player1.attackCard, "afterJudgment");
        ApplyEffects(player2, player1, player2.attackCard, "afterJudgment");

        return (player1Damage, player2Damage, player1FP, player2FP);
    }

    private void ApplyEffects(Player player, Player opponent, Card card, string timing)
    {
        if (card == null) return;

        List<Effect> effects = new List<Effect>();
        switch (timing)
        {
            case "use":
                effects = card.useEffects;
                break;
            case "beforeJudgment":
                effects = card.beforeJudgmentEffects;
                break;
            case "duringJudgment":
                effects = card.duringJudgmentEffects;
                break;
            case "afterJudgment":
                effects = card.afterJudgmentEffects;
                break;
        }

        foreach (var effect in effects)
        {
            if (EvaluateConditions(player, opponent, card.conditions))
            {
                effect.apply(player, opponent);
            }
        }
    }

    private bool EvaluateConditions(Player player, Player opponent, List<Condition> conditions)
    {
        foreach (var condition in conditions)
        {
            if (!condition.evaluate(player, opponent))
            {
                return false;
            }
        }
        return true;
    }

    private (int player1Damage, int player2Damage, int newPlayer1FP, int newPlayer2FP) HandleAttackVsDefense(AttackCard attackCard, DefenseCard defenseCard, int attackerFP, int defenderFP)
    {
        int player1Damage = 0;
        int player2Damage = 0;
        int newPlayer1FP = attackerFP;
        int newPlayer2FP = defenderFP;

        string defenseResult = DetermineDefenseResult(attackCard, defenseCard);

        switch (defenseResult)
        {
            case "hit":
                player2Damage = attackCard.attackPower;
                newPlayer1FP = 0;
                ApplyEffects(attackCard, "hit");
                break;

            case "guard":
                player1Damage = defenseCard.effectDamage;
                newPlayer1FP = 0;
                newPlayer2FP -= attackCard.guardFP;
                ApplyEffects(defenseCard, "guard");
                break;

            case "evade":
                ApplyEffects(defenseCard, "evade");
                break;
        }

        return (player1Damage, player2Damage, newPlayer1FP, newPlayer2FP);
    }

    private (int player1Damage, int player2Damage, int newPlayer1FP, int newPlayer2FP) HandleAttackVsAttack(AttackCard attackCard1, AttackCard attackCard2, int fp1, int fp2)
    {
        int player1Damage = 0;
        int player2Damage = 0;
        int newPlayer1FP = 0;
        int newPlayer2FP = 0;

        int speed1 = attackCard1.speed - fp1;
        int speed2 = attackCard2.speed - fp2;

        if (speed1 > speed2)
        {
            player2Damage = attackCard1.attackPower;
            newPlayer1FP = 0;
            ApplyEffects(attackCard1, "hit");
        }
        else if (speed1 < speed2)
        {
            player1Damage = attackCard2.attackPower;
            newPlayer2FP = 0;
            ApplyEffects(attackCard2, "hit");
        }
        else
        {
            player1Damage = attackCard1.attackPower;
            player2Damage = attackCard2.attackPower;
            newPlayer1FP = 0;
            newPlayer2FP = 0;
            ApplyEffects(attackCard1, "hit");
            ApplyEffects(attackCard2, "hit");
        }

        return (player1Damage, player2Damage, newPlayer1FP, newPlayer2FP);
    }

    private (int player1Damage, int player2Damage, int newPlayer1FP, int newPlayer2FP) HandleDefenseVsDefense(DefenseCard defenseCard1, DefenseCard defenseCard2, int fp1, int fp2)
    {
        // 방어 대 방어의 경우는 특별한 처리가 필요할 수 있습니다.
        Debug.Log("두 방어 카드가 맞붙었습니다. 특별한 처리가 필요합니다.");
        return (0, 0, 0, 0);
    }

    private string DetermineDefenseResult(AttackCard attackCard, DefenseCard defenseCard)
    {
        // 수비 카드의 각 위치 판정에 대해 처리
        if (attackCard.attackLine == AttackLine.High)
        {
            if (defenseCard.highSlot.slotType == SlotType.Defend)
                return "guard";
            if (defenseCard.highSlot.slotType == SlotType.Evade)
                return "evade";
        }
        else if (attackCard.attackLine == AttackLine.Mid)
        {
            if (defenseCard.midSlot.slotType == SlotType.Defend)
                return "guard";
            if (defenseCard.midSlot.slotType == SlotType.Evade)
                return "evade";
        }
        else if (attackCard.attackLine == AttackLine.Low)
        {
            if (defenseCard.lowSlot.slotType == SlotType.Defend)
                return "guard";
            if (defenseCard.lowSlot.slotType == SlotType.Evade)
                return "evade";
        }

        // 수비 카드의 위치 판정이 공격 카드의 위치 판정과 맞지 않으면 히트
        return "hit";
    }

    private void ApplyEffects(Card card, string result)
    {
        // 카드의 효과를 적용하는 로직을 여기에 구현합니다.
        Debug.Log($"Applying {result} effects for card: {card.name}");
    }
}
