using System.Collections;
using UnityEngine;

public class JoinMessages : MonoBehaviour
{
    public GameObject redJoinMessage;
    public GameObject blueJoinMessage;

    private IEnumerator redJoinFlash;
    private IEnumerator blueJoinFlash;

    // Start is called before the first frame update
    private void Awake()
    {
        redJoinFlash = FlashRedJoinMessage();
        blueJoinFlash = FlashBlueJoinMessage();

        StartCoroutine(redJoinFlash);
        StartCoroutine(blueJoinFlash);
    }

    public void StopFlashingBlue()
    {
        StopCoroutine(blueJoinFlash);
        blueJoinMessage.SetActive(false);
    }

    public void StopFlashingRed()
    {
        StopCoroutine(redJoinFlash);
        redJoinMessage.SetActive(false);
    }

    private IEnumerator FlashRedJoinMessage()
    {
        while (true)
        {
            redJoinMessage.SetActive(!redJoinMessage.activeSelf);
            yield return new WaitForSeconds(0.5f);
        }
    }

    private IEnumerator FlashBlueJoinMessage()
    {
        while (true)
        {
            blueJoinMessage.SetActive(!blueJoinMessage.activeSelf);
            yield return new WaitForSeconds(0.5f);
        }
    }
}
