using System;
using System.Collections.Generic;
using UnityEngine;

public class CardDatabase : MonoBehaviour
{
    public TextAsset jsonFile;
    public List<Card> cards = new List<Card>();

    void Start()
    {
        LoadCardsFromJson();
    }

    void LoadCardsFromJson()
    {
        if (jsonFile != null)
        {
            var jsonString = jsonFile.text;
            List<CardData> cardDataList = JsonUtility.FromJson<CardDataList>(jsonString).cards;
            foreach (var cardData in cardDataList)
            {
                if (cardData.cardType == "공격")
                {
                    AttackCard attackCard = new AttackCard
                    {
                        name = cardData.name,
                        cardType = CardType.Attack,
                        character = cardData.character,
                        speed = cardData.speed ?? 0,
                        attackLine = ParseAttackLine(cardData.attackLine),
                        attackPower = cardData.attackPower ?? 0,
                        limbType = cardData.limbType,
                        specialCondition = cardData.specialCondition,
                        hitFP = int.TryParse(cardData.hitFP, out int hitFpVal) ? hitFpVal : 0,
                        counterFP = int.TryParse(cardData.counterFP, out int counterFpVal) ? counterFpVal : 0,
                        guardFP = int.TryParse(cardData.guardFP, out int guardFpVal) ? guardFpVal : 0
                    };
                    cards.Add(attackCard);
                }
                else if (cardData.cardType == "수비")
                {
                    DefenseCard defenseCard = new DefenseCard
                    {
                        name = cardData.name,
                        cardType = CardType.Defense,
                        character = cardData.character,
                        highSlot = new DefenseSlot
                        {
                            slotType = ParseSlotType(cardData.highSlot),
                            condition = ""
                        },
                        midSlot = new DefenseSlot
                        {
                            slotType = ParseSlotType(cardData.midSlot),
                            condition = ""
                        },
                        lowSlot = new DefenseSlot
                        {
                            slotType = ParseSlotType(cardData.lowSlot),
                            condition = ""
                        },
                        effectDamage = 0,
                        specialCondition = cardData.specialCondition
                    };
                    cards.Add(defenseCard);
                }
                else
                {
                    Card specialCard = new Card
                    {
                        name = cardData.name,
                        cardType = CardType.Special,
                        character = cardData.character
                    };
                    cards.Add(specialCard);
                }
            }
        }
    }

    private AttackLine ParseAttackLine(string line)
    {
        switch (line)
        {
            case "상단":
                return AttackLine.High;
            case "중단":
                return AttackLine.Mid;
            case "하단":
                return AttackLine.Low;
            default:
                return AttackLine.High;
        }
    }

    private SlotType ParseSlotType(string type)
    {
        switch (type)
        {
            case "회피":
                return SlotType.Evade;
            case "방어":
                return SlotType.Defend;
            case "상쇄":
                return SlotType.Neutralize;
            default:
                return SlotType.None;
        }
    }
}

[System.Serializable]
public class CardData
{
    public string number;
    public string name;
    public string character;
    public string cardType;
    public int? speed;
    public string attackLine;
    public int? attackPower;
    public string limbType;
    public string specialCondition;
    public string hitFP;
    public string counterFP;
    public string guardFP;
    public string effects;
    public string highSlot;
    public string midSlot;
    public string lowSlot;
}

[System.Serializable]
public class CardDataList
{
    public List<CardData> cards;
}
