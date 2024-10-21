using System;
using UnityEngine;
using UnityEngine.InputSystem;

public class MatchController : MonoBehaviour
{
    public Transform player1StartPosition;
    public Transform player2StartPosition;

    private PlayerController player1;
    private PlayerController player2;
    private bool player1Ready = false;
    private bool player2Ready = false;

    private bool gameStarted = false;

    public static Action OnBothPlayersReady;

    private void OnEnable()
    {
        PlayerController.OnPlayerIsReady += OnPlayerIsReady;
    }

    private void OnPlayerIsReady(GameObject player)
    {
        if (player == player1)
        {
            player1Ready = true;
            Debug.Log("Player 1 is ready! Waiting for Player 2...");
        }
        else if (player == player2)
        {
            player2Ready = true;
            Debug.Log("Player 2 is ready! Waiting for Player 1...");
        }

        if (player1Ready && player2Ready)
        {
            Debug.Log("Both players are ready! Choose your starting positions.");
            EnablePositionSelection();
        }
    }

    void EnablePositionSelection()
    {
        player1.EnablePositionSelection(-1);
        player2.EnablePositionSelection(1);
    }

    public void OnPlayerJoined(PlayerInput playerInput)
    {
        if (player1 == null)
        {
            player1 = playerInput.gameObject.GetComponent<PlayerController>();
            player1.transform.position = player1StartPosition.position;
            player1.EnablePositionSelection(-1);
            Debug.Log("Player 1 joined. Waiting for Player 2...");
        }
        else if (player2 == null)
        {
            player2 = playerInput.gameObject.GetComponent<PlayerController>();
            player2.transform.position = player2StartPosition.position;
            player1.EnablePositionSelection(1);
            Debug.Log("Player 2 joined. Waiting for both players to be ready...");
        }
    }
}
