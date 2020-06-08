using System.Collections;
using UnityEngine;
using static Boxer;

public class GameMaster : MonoBehaviour
{
    public GameEvent startOfRoundEvent;
    public GameEvent redWinsEvent;
    public GameEvent blueWinsEvent;

    public AudioClip playerJoinClip;
    public AudioClip gameStartClip;
    public AudioClip roundStartClip;
    public AudioClip roundEndClip;
    public AudioClip knockoutClip;

    public AudioSource musicAudioSource;
    public AudioClip menuClip;
    public AudioClip roundMusicClip;

    public GameObject gameStartOverlay;

    private AudioSource audioSource;
    private int playerJoinCount;

    private void Awake()
    {
        audioSource = GetComponent<AudioSource>();
        gameStartOverlay.SetActive(true);
    }

    private void Start()
    {
        PlayMenuMusic();
    }

    public void OnPlayerJoin()
    {
        playerJoinCount++;
        audioSource.PlayOneShot(playerJoinClip);

        if (playerJoinCount == 2)
        {
            StartCoroutine(StartGameCoroutine());
        }
    }

    public void OnRoundEnd()
    {
        EnableBoxerControls(false);
        StopRoundMusic();
        StartCoroutine(StartRoundCoroutine());
    }

    public void OnKnockout()
    {
        StartCoroutine(KnockoutCoroutine());

        var winnerName = GetWinnerName();
        if (winnerName == BoxerName.Red)
        {
            redWinsEvent.Raise();
        }

        if (winnerName == BoxerName.Blue)
        {
            blueWinsEvent.Raise();
        }
    }

    private IEnumerator KnockoutCoroutine()
    {
        StopRoundMusic();
        audioSource.PlayOneShot(knockoutClip);
        yield return new WaitForSeconds(knockoutClip.length);
    }

    private IEnumerator StartGameCoroutine()
    {
        StopMenuMusic();
        audioSource.PlayOneShot(gameStartClip);
        yield return new WaitForSeconds(gameStartClip.length);
        audioSource.PlayOneShot(roundStartClip);
        gameStartOverlay.SetActive(false);
        yield return new WaitForSeconds(roundStartClip.length);
        EnableBoxerControls(true);
        PlayRoundMusic();
        startOfRoundEvent.Raise();
    }

    private IEnumerator StartRoundCoroutine()
    {
        MoveBoxersToStartingPositions();
        audioSource.PlayOneShot(roundStartClip);
        yield return new WaitForSeconds(roundStartClip.length);
        EnableBoxerControls(true);
        PlayRoundMusic();
        startOfRoundEvent.Raise();
    }

    private void EnableBoxerControls(bool isEnabled)
    {
        var boxers = GameObject.FindObjectsOfType<Boxer>();
        foreach (var boxer in boxers)
        {
            boxer.enabled = isEnabled;
        }
    }

    private void MoveBoxersToStartingPositions()
    {
        var boxers = GameObject.FindObjectsOfType<Boxer>();
        foreach (var boxer in boxers)
        {
            boxer.MoveToStartingPosition();
        }
    }

    private BoxerName GetWinnerName()
    {
        var boxers = GameObject.FindObjectsOfType<Boxer>();
        foreach (var boxer in boxers)
        {
            if (!boxer.IsKnockedOut()) return boxer.GetName();
        }

        // Double KO?!
        return BoxerName.None;
    }

    private void PlayRoundMusic()
    {
        musicAudioSource.clip = roundMusicClip;
        musicAudioSource.loop = true;
        musicAudioSource.Play();
    }

    private void StopRoundMusic()
    {
        musicAudioSource.Stop();
    }

    private void PlayMenuMusic()
    {
        musicAudioSource.clip = menuClip;
        musicAudioSource.loop = true;
        musicAudioSource.Play();
    }

    private void StopMenuMusic()
    {
        musicAudioSource.Stop();
    }
}
