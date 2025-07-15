using UnityEngine;
using Unity.Netcode;

[RequireComponent(typeof(NetworkObject), typeof(Rigidbody2D))]
public class NetworkBall : NetworkBehaviour
{
    private Rigidbody2D rb;
    private NetworkVariable<Vector2> netPosition = new NetworkVariable<Vector2>();
    private NetworkVariable<Vector2> netVelocity = new NetworkVariable<Vector2>();

    private void Awake()
    {
        rb = GetComponent<Rigidbody2D>();
    }

    public override void OnNetworkSpawn()
    {
        if (IsServer)
        {
            netPosition.Value = rb.position;
            netVelocity.Value = rb.linearVelocity;
        }
    }

    private void FixedUpdate()
    {
        if (IsServer)
        {
            netPosition.Value = rb.position;
            netVelocity.Value = rb.linearVelocity;
        }
        else
        {
            rb.position = netPosition.Value;
            rb.linearVelocity = netVelocity.Value;
        }
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (!IsServer) return;

        if (other.CompareTag("LeftGoal") || other.CompareTag("RightGoal"))
        {
            // Notify GameManager
            NetworkGameManager gameManager = FindObjectOfType<NetworkGameManager>();
            if (gameManager != null)
            {
                gameManager.HandleGoal(other.tag);
            }

            // Respawn ball
            NetworkBallSpawner spawner = FindObjectOfType<NetworkBallSpawner>();
            if (spawner != null)
            {
                spawner.RespawnBallServerRpc();
            }

            // Destroy current ball
            GetComponent<NetworkObject>().Despawn(true);
        }
    }
}