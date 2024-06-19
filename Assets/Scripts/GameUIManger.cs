using UnityEngine;
using TMPro;
using UnityEngine.UI;
using System.Collections.Generic;

public class GameUIManager : MonoBehaviour
{
    public TMP_Text player1FPText; // 플레이어 1의 FP 텍스트
    public TMP_Text player1LifeText; // 플레이어 1의 생명력 텍스트
    public TMP_Text player2FPText; // 플레이어 2의 FP 텍스트
    public TMP_Text player2LifeText; // 플레이어 2의 생명력 텍스트

    public TMP_Dropdown player1CardDropdown; // 플레이어 1의 카드 선택 드롭다운
    public TMP_Dropdown player2CardDropdown; // 플레이어 2의 카드 선택 드롭다운

    public Button player1FPPlusButton; // 플레이어 1의 FP 증가 버튼
    public Button player1FPMinusButton; // 플레이어 1의 FP 감소 버튼
    public Button player2FPPlusButton; // 플레이어 2의 FP 증가 버튼
    public Button player2FPMinusButton; // 플레이어 2의 FP 감소 버튼

    public Button executeButton; // 턴 실행 버튼
    public Button resetFPButton; // FP 리셋 버튼

    public TMP_Text player1DamageText; // 플레이어 1의 데미지 텍스트
    public TMP_Text player2DamageText; // 플레이어 2의 데미지 텍스트

    public GameManager gameManager; // 게임 매니저 참조
    public CardDatabase cardDatabase; // 카드 데이터베이스 참조
    public TMP_FontAsset customFont; // Inspector에서 설정할 폰트 에셋

    private void Start()
    {
        // 드롭다운에 카드 목록 채우기
        PopulateDropdowns();
        // 드롭다운의 폰트를 커스텀 폰트로 설정
        SetDropdownFont(player1CardDropdown);
        SetDropdownFont(player2CardDropdown);

        // 버튼 클릭 이벤트 리스너 추가
        if (player1FPPlusButton != null)
            player1FPPlusButton.onClick.AddListener(() => AdjustFP(gameManager.player1, 1));
        if (player1FPMinusButton != null)
            player1FPMinusButton.onClick.AddListener(() => AdjustFP(gameManager.player1, -1));
        if (player2FPPlusButton != null)
            player2FPPlusButton.onClick.AddListener(() => AdjustFP(gameManager.player2, 1));
        if (player2FPMinusButton != null)
            player2FPMinusButton.onClick.AddListener(() => AdjustFP(gameManager.player2, -1));
        if (executeButton != null)
            executeButton.onClick.AddListener(ExecuteTurn);
        if (resetFPButton != null)
            resetFPButton.onClick.AddListener(ResetFP);

        // 드롭다운 값 변경 이벤트 리스너 추가
        player1CardDropdown.onValueChanged.AddListener(delegate { DropdownValueChanged(player1CardDropdown, gameManager.player1); });
        player2CardDropdown.onValueChanged.AddListener(delegate { DropdownValueChanged(player2CardDropdown, gameManager.player2); });

        // UI 업데이트
        UpdateUI();
    }

    // 드롭다운에 카드 목록을 채우는 메서드
    private void PopulateDropdowns()
    {
        List<string> cardNames = new List<string>();
        foreach (var card in cardDatabase.cards)
        {
            cardNames.Add(card.name);
        }

        player1CardDropdown.ClearOptions(); // 기존 항목 제거
        player2CardDropdown.ClearOptions(); // 기존 항목 제거

        player1CardDropdown.AddOptions(cardNames);
        player2CardDropdown.AddOptions(cardNames);
    }

    private void SetDropdownFont(TMP_Dropdown dropdown)
    {
        if (customFont != null)
        {
            dropdown.captionText.font = customFont;
            dropdown.itemText.font = customFont;
        }
    }

    // 드롭다운 값이 변경될 때 호출되는 메서드
    private void DropdownValueChanged(TMP_Dropdown dropdown, Player player)
    {
        string selectedCardName = dropdown.options[dropdown.value].text;
        var selectedCard = cardDatabase.cards.Find(card => card.name == selectedCardName);

        if (selectedCard is AttackCard)
        {
            player.attackCard = (AttackCard)selectedCard;
            player.defenseCard = null; // 공격 카드를 선택한 경우 방어 카드를 초기화
        }
        else if (selectedCard is DefenseCard)
        {
            player.defenseCard = (DefenseCard)selectedCard;
            player.attackCard = null; // 방어 카드를 선택한 경우 공격 카드를 초기화
        }

        // UI 업데이트
        UpdateUI();
    }

    // FP를 조정하는 메서드
    private void AdjustFP(Player player, int amount)
    {
        if (player != null)
        {
            player.fp += amount;
            UpdateUI();
        }
    }

    // 턴을 실행하는 메서드
    private void ExecuteTurn()
    {
        if (gameManager != null)
        {
            // 초기 데미지 텍스트 설정
            player1DamageText.text = "0";
            player2DamageText.text = "0";

            // 판정을 수행하고 결과를 받음
            var (player1Damage, player2Damage, newPlayer1FP, newPlayer2FP) = gameManager.judgmentManager.ProcessJudgment(gameManager.player1, gameManager.player2);

            // 데미지 및 FP 업데이트
            gameManager.player1.life -= player1Damage;
            gameManager.player2.life -= player2Damage;
            gameManager.player1.fp = newPlayer1FP;
            gameManager.player2.fp = newPlayer2FP;

            // 데미지 텍스트 업데이트
            player1DamageText.text = player1Damage.ToString();
            player2DamageText.text = player2Damage.ToString();

            // UI 업데이트
            UpdateUI();
        }
    }

    // FP를 리셋하는 메서드
    private void ResetFP()
    {
        if (gameManager != null)
        {
            gameManager.player1.fp = 0;
            gameManager.player2.fp = 0;
            UpdateUI();
        }
    }

    // UI를 업데이트하는 메서드
    private void UpdateUI()
    {
        if (gameManager != null && gameManager.player1 != null && gameManager.player2 != null)
        {
            player1FPText.text = $"FP: {gameManager.player1.fp}";
            player1LifeText.text = $"Life: {gameManager.player1.life}";
            player2FPText.text = $"FP: {gameManager.player2.fp}";
            player2LifeText.text = $"Life: {gameManager.player2.life}";
        }
    }
}
