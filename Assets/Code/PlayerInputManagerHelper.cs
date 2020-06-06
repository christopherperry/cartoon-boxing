using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerInputManagerHelper : MonoBehaviour
{
    public GameObject redBoxerPrefab;
    public GameObject blueBoxerPrefab;

    private void Awake()
    {

    }

    private void OnPlayerJoined(PlayerInput playerInput)
    {
        PlayerInputManager.instance.playerPrefab = blueBoxerPrefab;
    }

    // Start is called before the first frame update
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {

    }
}
