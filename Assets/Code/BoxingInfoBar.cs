using System;
using System.Collections;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class BoxingInfoBar : MonoBehaviour
{
    public GameEvent endOfRoundEvent;

    public FloatVariable maxHealth;

    public FloatVariable redHealth;
    public Slider redSlider;

    public FloatVariable blueHealth;
    public Slider blueSlider;

    public TextMeshProUGUI timerText;
    public TextMeshProUGUI roundText;

    public FloatVariable redHeartsCount;
    public TextMeshProUGUI redHeartsText;

    public FloatVariable blueHeartsCount;
    public TextMeshProUGUI blueHeartsText;

    public GameObject redWinsMessage;
    public GameObject blueWinsMessage;

    private IEnumerator roundTimer;
    private int roundNumber = 1;
    private int roundTimeSeconds = 180;

    private void Awake()
    {
        timerText.text = TimeSpan.FromSeconds(0f).ToString(@"mm\:ss");
        roundText.text = $"Round {roundNumber}";

        redWinsMessage.SetActive(false);
        blueWinsMessage.SetActive(false);
    }

    public void UpdateHealthBars()
    {
        blueSlider.value = blueHealth.Value / maxHealth.Value;
        redSlider.value = redHealth.Value / maxHealth.Value;
    }

    public void UpdateHeartCounts()
    {
        redHeartsText.text = $"{redHeartsCount.Value}";
        blueHeartsText.text = $"{blueHeartsCount.Value}";
    }

    public void StartNewRound()
    {
        roundTimer = RoundTimer();
        StartCoroutine(roundTimer);
    }

    public void OnRedWins()
    {
        redWinsMessage.SetActive(true);
        StopCoroutine(roundTimer);
    }

    public void OnBlueWins()
    {
        blueWinsMessage.SetActive(true);
        StopCoroutine(roundTimer);
    }

    private IEnumerator RoundTimer()
    {
        var timeSeconds = 0f;
        timerText.text = TimeSpan.FromSeconds(timeSeconds).ToString(@"mm\:ss");

        while (timeSeconds < roundTimeSeconds)
        {
            timeSeconds += 1;
            Debug.Log("Time seconds = " + timeSeconds);
            yield return new WaitForSeconds(1);
            timerText.text = TimeSpan.FromSeconds(timeSeconds).ToString(@"mm\:ss");
        }

        roundNumber++;
        roundText.text = $"Round {roundNumber}";
        timerText.text = TimeSpan.FromSeconds(0).ToString(@"mm\:ss");
        endOfRoundEvent.Raise();
    }
}
