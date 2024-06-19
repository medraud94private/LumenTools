using System;
using System.Collections.Generic;

public enum CardType { Attack, Defense, Special }
public enum SlotType { None, Evade, Defend, Neutralize }
public enum AttackLine { High, Mid, Low }

[Serializable]
public class Card
{
    public string name;
    public CardType cardType;
    public string character;
    public List<Condition> conditions = new List<Condition>();
    public List<Effect> useEffects = new List<Effect>();
    public List<Effect> beforeJudgmentEffects = new List<Effect>();
    public List<Effect> duringJudgmentEffects = new List<Effect>();
    public List<Effect> afterJudgmentEffects = new List<Effect>();
}

[Serializable]
public class AttackCard : Card
{
    public int speed;
    public AttackLine attackLine;
    public int attackPower;
    public string limbType; // "hand" or "foot"
    public string specialCondition;
    public int hitFP;
    public int counterFP;
    public int guardFP;
}

[Serializable]
public class DefenseSlot
{
    public SlotType slotType;
    public string condition;
}

[Serializable]
public class DefenseCard : Card
{
    public DefenseSlot highSlot;
    public DefenseSlot midSlot;
    public DefenseSlot lowSlot;
    public int effectDamage; // 추가
    public string specialCondition;
}
