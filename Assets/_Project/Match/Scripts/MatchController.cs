using System;
using UnityEngine;
using UnityEngine.InputSystem;

public class MatchController : MonoBehaviour
{
    private GameObject player1;
    private GameObject player2;
    private bool player1Ready = false;
    private bool player2Ready = false;

    private bool gameStarted = false;

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

    }

    public void OnPlayerJoined(PlayerInput playerInput)
    {
        if (player1 == null)
        {
            player1 = playerInput.gameObject;
            Debug.Log("Player 1 joined. Waiting for Player 2...");
        }
        else if (player2 == null)
        {
            player2 = playerInput.gameObject;
            Debug.Log("Player 2 joined. Waiting for both players to be ready...");
        }
    }
}
