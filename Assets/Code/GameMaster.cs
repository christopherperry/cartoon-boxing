using System.Collections;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.SceneManagement;
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
    public AudioClip knockoutCheerClip;
    public AudioClip winnerClip;

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

    public void OnRedPlayerJoin()
    {
        EnableBoxerControls(BoxerName.Red, false);
        OnPlayerJoin();
    }

    public void OnBluePlayerJoin()
    {
        EnableBoxerControls(BoxerName.Blue, false);
        OnPlayerJoin();
    }

    private void OnPlayerJoin()
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
        DisableAllBoxersControls();
        StopRoundMusic();
        StartCoroutine(StartRoundCoroutine());
    }

    public void OnKnockout()
    {
        StartCoroutine(KnockoutCoroutine());
    }

    private IEnumerator KnockoutCoroutine()
    {
        StopRoundMusic();
        audioSource.PlayOneShot(knockoutClip);
        audioSource.PlayOneShot(knockoutCheerClip);
        yield return new WaitForSeconds(knockoutClip.length / 3f);
        audioSource.Stop();

        var winnerName = GetWinnerName();
        if (winnerName == BoxerName.Red)
        {
            redWinsEvent.Raise();
        }

        if (winnerName == BoxerName.Blue)
        {
            blueWinsEvent.Raise();
        }
        audioSource.PlayOneShot(winnerClip);
        yield return new WaitForSeconds(winnerClip.length);
        audioSource.Stop();

        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
    }

    private IEnumerator StartGameCoroutine()
    {
        StopMenuMusic();
        audioSource.PlayOneShot(gameStartClip);
        yield return new WaitForSeconds(gameStartClip.length);
        audioSource.PlayOneShot(roundStartClip);
        gameStartOverlay.SetActive(false);
        yield return new WaitForSeconds(roundStartClip.length);
        EnableAllBoxersControls();
        PlayRoundMusic();
        startOfRoundEvent.Raise();
    }

    private IEnumerator StartRoundCoroutine()
    {
        MoveBoxersToStartingPositions();
        audioSource.PlayOneShot(roundStartClip);
        yield return new WaitForSeconds(roundStartClip.length);
        EnableAllBoxersControls();
        PlayRoundMusic();
        startOfRoundEvent.Raise();
    }

    private void EnableAllBoxersControls()
    {
        EnableBoxerControls(BoxerName.Red, true);
        EnableBoxerControls(BoxerName.Blue, true);
    }

    private void DisableAllBoxersControls()
    {
        EnableBoxerControls(BoxerName.Red, false);
        EnableBoxerControls(BoxerName.Blue, false);
    }

    private void EnableBoxerControls(BoxerName name, bool isEnabled)
    {
        var boxers = GameObject.FindObjectsOfType<Boxer>();
        foreach (var boxer in boxers)
        {
            if (boxer.GetName() == name)
            {
                var actions = boxer.GetComponent<PlayerInput>().currentActionMap;
                if (isEnabled)
                {
                    actions.Enable();
                }
                else
                {
                    actions.Disable();
                }
            }
        }
    }

    private void MoveBoxersToStartingPositions()
    {
        var boxers = GameObject.FindObjectsOfType<Boxer>();
        foreach (var boxer in boxers)
        {
            boxer.MoveToStartingPosition();
            boxer.ResetHealth();
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
