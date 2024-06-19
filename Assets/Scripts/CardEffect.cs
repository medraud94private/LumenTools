using System;
using System.Collections.Generic;

[Serializable]
public class Condition
{
    public string description; // 조건 설명
    public Func<Player, Player, bool> evaluate; // 조건을 평가하는 함수
}

[Serializable]
public class Effect
{
    public string description; // 효과 설명
    public Action<Player, Player> apply; // 효과를 적용하는 함수
}
