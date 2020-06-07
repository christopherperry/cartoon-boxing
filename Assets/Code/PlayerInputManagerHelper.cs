using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerInputManagerHelper : MonoBehaviour
{
    public GameObject redBoxerPrefab;
    public GameObject blueBoxerPrefab;

    public GameEvent onRedPlayerJoin;
    public GameEvent onBluePlayerJoin;

    private void OnPlayerJoined(PlayerInput playerInput)
    {
        if (PlayerInputManager.instance.playerPrefab == redBoxerPrefab)
        {
            onRedPlayerJoin.Raise();
        }
        else if (PlayerInputManager.instance.playerPrefab == blueBoxerPrefab)
        {
            onBluePlayerJoin.Raise();
        }

        PlayerInputManager.instance.playerPrefab = blueBoxerPrefab;
    }
}
