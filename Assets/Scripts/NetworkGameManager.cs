using UnityEngine;
using Unity.Netcode;
using TMPro;
using UnityEngine.SceneManagement; // Add this namespace

public class NetworkGameManager : NetworkBehaviour
{
    [SerializeField] private TMP_Text scoreText;

    private NetworkVariable<int> player1Score = new NetworkVariable<int>(0);
    private NetworkVariable<int> player2Score = new NetworkVariable<int>(0);
    private const int winningScore = 3;

    public override void OnNetworkSpawn()
    {
        player1Score.OnValueChanged += OnScoreChanged;
        player2Score.OnValueChanged += OnScoreChanged;
        UpdateScoreDisplay();
    }

    public void HandleGoal(string goalTag)
    {
        if (!IsServer) return;

        if (goalTag == "LeftGoal")
        {
            player2Score.Value++;
        }
        else if (goalTag == "RightGoal")
        {
            player1Score.Value++;
        }

        CheckGameOver();
    }

    private void OnScoreChanged(int previous, int current)
    {
        UpdateScoreDisplay();
    }

    private void UpdateScoreDisplay()
    {
        scoreText.text = $"{player1Score.Value} : {player2Score.Value}";
    }

    private void CheckGameOver()
    {
        if (player1Score.Value >= winningScore)
        {
            NetworkManager.SceneManager.LoadScene("Player1Wins", UnityEngine.SceneManagement.LoadSceneMode.Single);
        }
        else if (player2Score.Value >= winningScore)
        {
            NetworkManager.SceneManager.LoadScene("Player2Wins", UnityEngine.SceneManagement.LoadSceneMode.Single);
        }
    }

    [ServerRpc]
    public void ResetScoresServerRpc()
    {
        player1Score.Value = 0;
        player2Score.Value = 0;
    }
}