using UnityEngine;
using Unity.Netcode;

public class NetworkBallSpawner : NetworkBehaviour
{
    [SerializeField] private GameObject ballPrefab;
    [SerializeField] private Vector3 spawnPosition = Vector3.zero;

    public override void OnNetworkSpawn()
    {
        if (IsServer)
        {
            SpawnBall();
        }
    }

    [ServerRpc]
    private void SpawnBallServerRpc()
    {
        SpawnBall();
    }

    private void SpawnBall()
    {
        GameObject ball = Instantiate(ballPrefab, spawnPosition, Quaternion.identity);
        NetworkObject netObj = ball.GetComponent<NetworkObject>();
        netObj.Spawn(true); 

        
        Rigidbody2D rb = ball.GetComponent<Rigidbody2D>();
        if (rb != null)
        {
            rb.position = spawnPosition;
            rb.linearVelocity = Vector2.zero;
            rb.angularVelocity = 0f;
        }
    }

    // maybe use if want ball to respawn after goal
    [ServerRpc]
    public void RespawnBallServerRpc()
    {
        SpawnBall();
    }
}