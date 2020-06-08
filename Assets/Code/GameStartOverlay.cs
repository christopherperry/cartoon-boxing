using UnityEngine;

public class GameStartOverlay : MonoBehaviour
{
    public GameObject redIdleImage;
    public GameObject redPunchImage;

    public GameObject blueIdleImage;
    public GameObject bluePunchImage;

    private void Awake()
    {
        redIdleImage.SetActive(true);
        redPunchImage.SetActive(false);

        blueIdleImage.SetActive(true);
        bluePunchImage.SetActive(false);
    }

    public void OnBlueJoined()
    {
        blueIdleImage.SetActive(false);
        bluePunchImage.SetActive(true);
    }

    public void OnRedJoined()
    {
        redIdleImage.SetActive(false);
        redPunchImage.SetActive(true);
    }
}
