using DG.Tweening;
using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class Hand : MonoBehaviour
{
    private readonly string[] CHOICE = { "ROCK", "PAPER", "SCISSORS" };
    private const float ROTATION = 30f;
    private const float ROTATION_DURATION = 0.2f;

    [SerializeField] private Sprite[] handSprites;
    [SerializeField] private Transform playerHand;
    [SerializeField] private Transform enemyHand;
    [SerializeField] private GameObject groupButton;
    [SerializeField] private GameObject resultPopup;
    [SerializeField] private TextMeshProUGUI resultTxt;
    [SerializeField] private TextMeshProUGUI playerScoreTxt;
    [SerializeField] private TextMeshProUGUI enemyScoreTxt;

    private State state;
    private string playerChoice;
    private int playerScore = 5;
    private int enemyScore = 5;

    private void Start()
    {
        playerScoreTxt.text = $"Player score: {playerScore}";
        enemyScoreTxt.text = $"Enemy score: {enemyScore}";
        HideOrShowButton();
    }

    public void PlayerChoice(int index)
    {
        if (state == State.Playing) return;

        state = State.Playing;
        playerHand.GetComponentInChildren<Image>().sprite = handSprites[0];
        enemyHand.GetComponentInChildren<Image>().sprite = handSprites[0];

        HandAnimation(index);
        HideOrShowButton();
        ShowOrHideResultPopup();
    }

    private void HandAnimation(int index)
    {
        var indexRandom = UnityEngine.Random.Range(0, CHOICE.Length);
        playerChoice = CHOICE[index];

        playerHand.transform.DORotate(new Vector3(0, 0, ROTATION), ROTATION_DURATION)
             .SetLoops(5, LoopType.Yoyo)
             .OnComplete(() =>
             {
                 playerHand.transform.DORotate(Vector3.zero, ROTATION_DURATION / 2f)
                     .OnComplete(() =>
                     {
                         playerHand.GetComponentInChildren<Image>().sprite = handSprites[index];
                         enemyHand.GetComponentInChildren<Image>().sprite = handSprites[indexRandom];
                         CheckResult(indexRandom);
                     });
             });

        enemyHand.transform.DORotate(new Vector3(0, 0, -ROTATION), ROTATION_DURATION)
          .SetLoops(5, LoopType.Yoyo)
          .OnComplete(() =>
          {
              enemyHand.transform.DORotate(Vector3.zero, ROTATION_DURATION / 2f);
          });
    }
    private void ShowOrHideResultPopup()
    {
        var resultPopupRect = resultPopup.GetComponent<RectTransform>();
        if (resultPopupRect.sizeDelta.y == 0)
        {
            resultPopupRect.DOSizeDelta(new Vector2(resultPopupRect.sizeDelta.x, 200), 1f).
                OnComplete(() =>
                {
                    resultTxt.transform.DOScale(Vector3.one, 0.2f).SetEase(Ease.OutBack);
                });
        }
        else
        {
            resultPopupRect.DOSizeDelta(new Vector2(resultPopupRect.sizeDelta.x, 0), 1f).
                OnComplete(() =>
                {
                    resultTxt.transform.DOScale(Vector3.zero, 0.2f).SetEase(Ease.InBack);

                }); 
        }
    }
    private void HideOrShowButton()
    {
        state = State.Playing;
        groupButton.transform.DOMoveY(-groupButton.transform.position.y, 1f).SetEase(Ease.Linear)
            .OnComplete(() =>
            {
                state = State.Watting;
            });
    }

    public void CheckResult(int indexRandom)
    {
        var computerChoice = CHOICE[indexRandom];
        string rockWin = "ROCK\nBREAKS\nSCISSORS";
        string paperWin = "PAPER\nWRAPS\nROCK";
        string scissorsWin = "SCISSORS\nCUT PAPER";
        string draw = $"DRAW\n{playerChoice} VS {computerChoice}";

        switch (playerChoice)
        {
            case "ROCK" when computerChoice == "SCISSORS":
                resultTxt.text = rockWin;
                --enemyScore;
                enemyScoreTxt.text = $"Enemy score: {enemyScore}";
                break;
            case "PAPER" when computerChoice == "ROCK":
                resultTxt.text = paperWin;
                --enemyScore;
                enemyScoreTxt.text = $"Enemy score: {enemyScore}";
                break;
            case "SCISSORS" when computerChoice == "PAPER":
                resultTxt.text = scissorsWin;
                --enemyScore;
                enemyScoreTxt.text = $"Enemy score: {enemyScore}";
                break;
            case var _ when playerChoice == computerChoice:
                resultTxt.text = draw;
                break;
            default:
                switch (computerChoice)
                {
                    case "ROCK": resultTxt.text = rockWin; break;

                    case "PAPER": resultTxt.text = paperWin; break;

                    case "SCISSORS": resultTxt.text = scissorsWin; break;
                }
                --playerScore;
                playerScoreTxt.text = $"Player score: {playerScore}";
                break;
        }
        ShowOrHideResultPopup();
        HideOrShowButton();
    }
}
public enum State
{
    Watting,
    Playing,
}
