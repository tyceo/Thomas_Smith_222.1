using UnityEngine;
using Unity.Netcode;

public class NetworkPaddleInitializer : NetworkBehaviour
{
    [Header("Scene Paddles")]
    [SerializeField] private GameObject PaddleP1Up;
    [SerializeField] private GameObject PaddleP1Down;
    [SerializeField] private GameObject PaddleP2Up;
    [SerializeField] private GameObject PaddleP2Down;

    public override void OnNetworkSpawn()
    {
        if (!IsServer) return;

        // Host owns P1 paddles
        AssignOwnership(PaddleP1Up, NetworkManager.ServerClientId);
        AssignOwnership(PaddleP1Down, NetworkManager.ServerClientId);

        // When client connects, assign P2 paddles
        NetworkManager.OnClientConnectedCallback += OnClientConnected;
    }

    private void OnClientConnected(ulong clientId)
    {
        if (clientId != NetworkManager.ServerClientId)
        {
            AssignOwnership(PaddleP2Up, clientId);
            AssignOwnership(PaddleP2Down, clientId);
        }
    }

    private void AssignOwnership(GameObject paddle, ulong clientId)
    {
        if (paddle != null && paddle.TryGetComponent<NetworkObject>(out var netObj))
        {
            netObj.ChangeOwnership(clientId);
        }
    }

    private void OnDestroy()
    {
        if (NetworkManager.Singleton != null)
        {
            NetworkManager.Singleton.OnClientConnectedCallback -= OnClientConnected;
        }
    }
}