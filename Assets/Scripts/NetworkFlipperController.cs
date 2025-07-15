using UnityEngine;
using Unity.Netcode;

[RequireComponent(typeof(NetworkObject))]
public class NetworkFlipperController : NetworkBehaviour
{
    private HingeJoint2D hinge;
    private JointMotor2D motor;

    [Header("Physics Settings")]
    public float motorSpeed = -1000f;
    public float motorOffSpeed = 1000f;
    public float motorTorque = 10000f;

    // Public properties for paddle configuration
    public bool IsPlayer1Paddle { get; set; }
    public bool IsLeftPaddle { get; set; }

    private KeyCode myKey;

    private void Awake()
    {
        hinge = GetComponent<HingeJoint2D>();
        motor = hinge.motor;
        motor.maxMotorTorque = motorTorque;

        // Auto-configure based on paddle name
        string paddleName = gameObject.name;
        IsPlayer1Paddle = paddleName.Contains("P1");
        IsLeftPaddle = paddleName.Contains("Up");

        if (IsPlayer1Paddle)
        {
            myKey = IsLeftPaddle ? KeyCode.A : KeyCode.D;
        }
        else // Player 2 paddle
        {
            myKey = IsLeftPaddle ? KeyCode.A : KeyCode.D;
        }
    }

    private void Update()
    {
        
        bool shouldProcessInput = IsOwner &&
                               ((IsPlayer1Paddle && IsHost) || (!IsPlayer1Paddle && !IsHost));

        if (!shouldProcessInput) return;

        bool shouldActivate = Input.GetKey(myKey);
        UpdatePaddleState(shouldActivate);

        if (IsClient)
        {
            UpdatePaddleStateServerRpc(shouldActivate);
        }
    }

    private void UpdatePaddleState(bool activated)
    {
        motor.motorSpeed = activated ? motorSpeed : motorOffSpeed;
        hinge.motor = motor;
    }

    [ServerRpc]
    private void UpdatePaddleStateServerRpc(bool activated)
    {
        UpdatePaddleState(activated);
    }

    [ClientRpc]
    private void UpdatePaddleStateClientRpc(bool activated)
    {
        if (!IsOwner) // Only update on other clients
        {
            UpdatePaddleState(activated);
        }
    }
}