using UnityEngine;
using Unity.Netcode;

public class PlayerSpawner : NetworkBehaviour
{
    [Header("Paddle Prefabs")]
    [SerializeField] private GameObject PaddleP1Up;
    [SerializeField] private GameObject PaddleP1Down;
    [SerializeField] private GameObject PaddleP2Up;
    [SerializeField] private GameObject PaddleP2Down;

    public override void OnNetworkSpawn()
    {
        if (!IsServer) return;

        // Spawn Player 1 (Host) paddles
        SpawnPaddle(PaddleP1Up, NetworkManager.ServerClientId, true, true);
        SpawnPaddle(PaddleP1Down, NetworkManager.ServerClientId, true, false);

        // Spawn Player 2 (Client) paddles when connected
        NetworkManager.OnClientConnectedCallback += clientId =>
        {
            if (clientId != NetworkManager.ServerClientId)
            {
                SpawnPaddle(PaddleP2Up, clientId, false, true);
                SpawnPaddle(PaddleP2Down, clientId, false, false);
            }
        };
    }

    private void SpawnPaddle(GameObject prefab, ulong ownerId, bool isPlayer1, bool isLeftPaddle)
    {
        GameObject paddle = Instantiate(prefab);
        NetworkObject netObj = paddle.GetComponent<NetworkObject>();
        NetworkFlipperController controller = paddle.GetComponent<NetworkFlipperController>();

        // Use properties instead of fields
        controller.IsPlayer1Paddle = isPlayer1;
        controller.IsLeftPaddle = isLeftPaddle;
        netObj.SpawnWithOwnership(ownerId);
    }
}