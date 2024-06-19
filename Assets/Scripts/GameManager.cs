using UnityEngine;

public class GameManager : MonoBehaviour
{
    public Player player1;
    public Player player2;
    public JudgmentManager judgmentManager;

    void Start()
    {
        InitializePlayers();
    }

    void InitializePlayers()
    {
        if (player1 == null)
            player1 = new Player();
        if (player2 == null)
            player2 = new Player();

        player1.life = 5000;
        player2.life = 5000;
        player1.fp = 0;
        player2.fp = 0;
    }

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Space))
        {
            ResolveTurn();
        }
    }

    void ResolveTurn()
    {
        var (player1Damage, player2Damage, newPlayer1FP, newPlayer2FP) = judgmentManager.ProcessJudgment(player1, player2);

        player1.life -= player1Damage;
        player2.life -= player2Damage;
        player1.fp = newPlayer1FP;
        player2.fp = newPlayer2FP;

        Debug.Log($"Player1: {player1.life} HP, {player1.fp} FP");
        Debug.Log($"Player2: {player2.life} HP, {player2.fp} FP");
    }
}
