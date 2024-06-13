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
    [SerializeField] private ResultPopup resultPopup;
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
        resultPopup.Hide();
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
        string result = "";
        switch (playerChoice)
        {
            case "ROCK" when computerChoice == "SCISSORS":
                result = rockWin;
                --enemyScore;
                enemyScoreTxt.text = $"Enemy score: {enemyScore}";
                break;
            case "PAPER" when computerChoice == "ROCK":
                result = paperWin;
                --enemyScore;
                enemyScoreTxt.text = $"Enemy score: {enemyScore}";
                break;
            case "SCISSORS" when computerChoice == "PAPER":
                result = scissorsWin;
                --enemyScore;
                enemyScoreTxt.text = $"Enemy score: {enemyScore}";
                break;
            case var _ when playerChoice == computerChoice:
                result = draw;
                break;
            default:
                switch (computerChoice)
                {
                    case "ROCK": result = rockWin; break;

                    case "PAPER": result = paperWin; break;

                    case "SCISSORS": result = scissorsWin; break;
                }
                --playerScore;
                playerScoreTxt.text = $"Player score: {playerScore}";
                break;
        }
        resultPopup.Show(result);
        HideOrShowButton();
    }
}
public enum State
{
    Watting,
    Playing,
}
