using System;

[Serializable]
public class Player
{
    public string name;
    public int fp;
    public int life;
    public string[] hand;
    public AttackCard attackCard;
    public DefenseCard defenseCard;
}
