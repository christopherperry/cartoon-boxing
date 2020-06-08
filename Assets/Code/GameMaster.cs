using System.Collections;
using UnityEngine;

public class GameMaster : MonoBehaviour
{
    public GameEvent startOfRoundEvent;

    public AudioClip playerJoinClip;
    public AudioClip gameStartClip;
    public AudioClip roundStartClip;
    public AudioClip roundEndClip;

    public AudioSource musicAudioSource;
    public AudioClip menuClip;
    public AudioClip roundMusicClip;

    public GameObject gameStartOverlay;

    private AudioSource audioSource;
    private int playerJoinCount;

    private void Awake()
    {
        audioSource = GetComponent<AudioSource>();
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

    private IEnumerator StartGameCoroutine()
    {
        StopMenuMusic();
        // yield return new WaitForSeconds(playerJoinClip.length);
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
