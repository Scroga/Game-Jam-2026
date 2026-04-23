using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using TMPro;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;

public enum SlotType { HEART = 0, SPADE, SEVEN, DOLLAR, LEMON, BAR, UNKNOWN };

public class SlotMachineController : MonoBehaviour
{
    [SerializeField]
    private TextMeshProUGUI prizeText;
    [SerializeField]
    private TextMeshProUGUI betText;

    [SerializeField]
    private TextMeshProUGUI scoreText;
    [SerializeField]
    private TextMeshProUGUI multiplierText;

    [SerializeField]
    private TextMeshProUGUI combinationsText;

    private string combinations;

    [SerializeField]
    private SlotMachineBotton botton;

    private const string SCORE_TEXT = "SCORE: ";
    private const string MULTIPLIER_TEXT = "BET MULTIPLIER: ";
    private int score = 10000;

    private const int MAX_MULTIPLIER = 1000;
    private const int MIN_MULTIPLIER = 1;

    private int betMultiplier = 1;
    private int prizeValue = 0;

    private bool canSetBet = true;
    private int betValue = 0;

    public static UnityEvent OnStart;

    [SerializeField]
    private SlotController slot1;

    [SerializeField]
    private SlotController slot2;

    [SerializeField]
    private SlotController slot3;
    void Start()
    {
        RefreshUI();
        botton.Activate();
        prizeValue = 0;
    }

    private void RefreshUI()
    {
        scoreText.text = SCORE_TEXT + score.ToString();
        multiplierText.text = MULTIPLIER_TEXT + betMultiplier.ToString();
        betText.text = betValue.ToString();
        prizeText.text = prizeValue.ToString();
        combinationsText.text = combinations;
    }

    private void Update()
    {
        if (canSetBet)
        {
            if (Input.GetKeyUp(KeyCode.UpArrow))
            {
                if (score >= betMultiplier)
                {
                    betValue += betMultiplier;
                    score -= betMultiplier;
                    RefreshUI();
                }
            }
            else if (Input.GetKeyUp(KeyCode.DownArrow))
            {
                if (betValue >= betMultiplier)
                {
                    betValue -= betMultiplier;
                    score += betMultiplier;
                    RefreshUI();
                }
            }
        }

        if (Input.GetKeyUp(KeyCode.RightArrow))
        {
            if (betMultiplier * 10 <= MAX_MULTIPLIER)
            {
                betMultiplier *= 10;
                RefreshUI();
            }
        }
        else if (Input.GetKeyUp(KeyCode.LeftArrow))
        {
            if (betMultiplier / 10 >= MIN_MULTIPLIER)
            {
                betMultiplier /= 10;
                RefreshUI();
            }
        }
    }

    private bool AreSlotsStopped()
    {
        return slot1.rowStopped && slot2.rowStopped && slot3.rowStopped;
    }

    public void StartGame()
    {
        if (AreSlotsStopped())
        {
            canSetBet = false;
            prizeValue = 0;
            RefreshUI();
            botton.Deactivate();
            StartCoroutine(GameHandle());
        }
    }

    public void SetInitialState()
    {
        slot1.SetInitialPosition();
        slot2.SetInitialPosition();
        slot3.SetInitialPosition();
    }

    private IEnumerator GameHandle()
    {
        yield return new WaitForSeconds(0.5f);
        OnStart?.Invoke();
        slot1.StartRotating();
        slot2.StartRotating();
        slot3.StartRotating();

        yield return new WaitUntil(AreSlotsStopped);
        botton.Activate();
        CheckResults();
    }

    private bool CheckSlots(SlotType value1, SlotType value2, SlotType value3)
    {
        return
            slot1.stoppedSlot == value1 &&
            slot2.stoppedSlot == value2 &&
            slot3.stoppedSlot == value3;
    }
    private void CheckResults()
    {
        canSetBet = true;
        string s1 = slot1.stoppedSlot.ToString();
        string s2 = slot2.stoppedSlot.ToString();
        string s3 = slot3.stoppedSlot.ToString();

        int prize = EvaluatePrize(slot1.stoppedSlot, slot2.stoppedSlot, slot3.stoppedSlot);
        prizeValue = prize > 0 ? prize : 0;

        if (prizeValue > 0) {
            SoundManager.Instance.PlaySound2D("Money");
        }

        Debug.Log($"{s1} {s2} {s3}");
        betValue = 0;
        score += prizeValue;

        RefreshUI();
    }

    public int EvaluatePrize(SlotType s1, SlotType s2, SlotType s3)
    {
        var slots = new[] { s1, s2, s3 };

        var counts = slots.GroupBy(x => x).ToDictionary(g => g.Key, g => g.Count());

        int spades = counts.TryGetValue(SlotType.SPADE, out var sp) ? sp : 0;

        combinations = $"1. {s1.ToString()}\n2.{s2.ToString()}\n3.{s3.ToString()}";

        if (counts.Count == 1)
        {
            var sym = slots[0];
            return sym switch
            {
                SlotType.HEART => betValue + 300,
                SlotType.DOLLAR => betValue + 600,
                SlotType.LEMON => betValue + 150,
                SlotType.SEVEN => betValue * 3,
                SlotType.BAR => betValue * 10,
                SlotType.SPADE => betValue - 600,
                _ => 0
            };
        }

        var pair = counts.FirstOrDefault(kv => kv.Value == 2).Key;
        bool hasPair = counts.Values.Any(v => v == 2);

        if (hasPair)
        {
            var kicker = counts.First(kv => kv.Value == 1).Key;

            if (spades == 1 && pair != SlotType.SPADE)
            {
                return pair switch
                {
                    SlotType.HEART => betValue - 10,
                    SlotType.DOLLAR => betValue - 50,
                    SlotType.LEMON => betValue + 50,
                    SlotType.BAR => betValue - 100,
                    SlotType.SEVEN => betValue + 100,
                    _ => betValue
                };
            }

            return pair switch
            {
                SlotType.SEVEN => betValue * 2, // SEVEN, SEVEN, (anything but spade) => x2
                SlotType.BAR => betValue * 3, // BAR, BAR, (anything but spade) => x3

                SlotType.DOLLAR when kicker == SlotType.SEVEN => betValue + 300,   // DOLLAR,DOLLAR,SEVEN
                SlotType.HEART when kicker == SlotType.SEVEN => betValue + 150,   // HEART,HEART,SEVEN
                SlotType.HEART when kicker == SlotType.DOLLAR => betValue + 100,  // HEART,HEART,DOLLAR
                SlotType.LEMON when kicker == SlotType.DOLLAR => betValue + 80,   // LEMON,LEMON,DOLLAR

                _ => betValue
            };
        }

        if (spades == 2)
        {
            var other = slots.First(x => x != SlotType.SPADE);
            return other switch
            {
                SlotType.HEART => betValue - 150,
                SlotType.DOLLAR => betValue - 50,
                SlotType.LEMON => betValue,
                SlotType.BAR => betValue - 250,
                SlotType.SEVEN => betValue,
                _ => betValue
            };
        }
        combinations = "No combinations";
        return 0;
    }
}
