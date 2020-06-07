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

    public FloatVariable redHeartsCount;
    public TextMeshProUGUI redHeartsText;

    public FloatVariable blueHeartsCount;
    public TextMeshProUGUI blueHeartsText;

    private void Start()
    {
        StartRound();
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

    public void StartRound()
    {
        StartCoroutine(RoundTimer());
    }

    private IEnumerator RoundTimer()
    {
        var timeSeconds = 0f;
        timerText.text = TimeSpan.FromSeconds(timeSeconds).ToString(@"mm\:ss");

        while (timeSeconds < 180)
        {
            timeSeconds += 1;
            Debug.Log("Time seconds = " + timeSeconds);
            yield return new WaitForSeconds(1);
            timerText.text = TimeSpan.FromSeconds(timeSeconds).ToString(@"mm\:ss");
        }

        endOfRoundEvent.Raise();
    }
}
