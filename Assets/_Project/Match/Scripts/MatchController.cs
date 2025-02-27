using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class MatchController : MonoBehaviour
{
    public BaseRobotController playerRobot; // Refer�ncia ao rob� do jogador
    public BaseRobotController aiRobot; // Refer�ncia ao rob� da IA
    public Transform playerStartPosition; // Posi��o inicial do jogador
    public Transform aiStartPosition; // Posi��o inicial da IA
    public float arenaRadius = 5f; // Raio da arena
    public float countdownTime = 3f; // Tempo da contagem regressiva
    public TextMeshProUGUI countdownText; // UI para exibir a contagem regressiva

    private bool matchInProgress = false;

    private void Start()
    {
        StartCoroutine(StartMatch());
    }

    private IEnumerator StartMatch()
    {
        playerRobot.SetMatchActive(false);
        aiRobot.SetMatchActive(false);

        matchInProgress = false;
        ResetRobots(); // Retorna os rob�s �s posi��es iniciais

        // Contagem regressiva
        for (float i = countdownTime; i > 0; i--)
        {
            countdownText.text = i.ToString();
            yield return new WaitForSeconds(1f);
        }

        countdownText.text = "GO!";
        yield return new WaitForSeconds(1f);
        countdownText.text = "";

        matchInProgress = true;
        
        playerRobot.SetMatchActive(true);
        aiRobot.SetMatchActive(true);

        // Verifica constantemente se algum rob� saiu da arena
        while (matchInProgress)
        {
            yield return new WaitForSeconds(0.5f); // Pequena pausa para otimizar performance
            CheckForWinner();
        }
    }

    private void CheckForWinner()
    {
        float playerDistance = Vector3.Distance(playerRobot.transform.position, Vector3.zero);
        float aiDistance = Vector3.Distance(aiRobot.transform.position, Vector3.zero);

        if (playerDistance > arenaRadius)
        {
            EndMatch("AI Wins!");
        }
        else if (aiDistance > arenaRadius)
        {
            EndMatch("Player Wins!");
        }
    }

    private void EndMatch(string winnerMessage)
    {
        playerRobot.SetMatchActive(false);
        aiRobot.SetMatchActive(false);

        matchInProgress = false;
        countdownText.text = winnerMessage;
        StartCoroutine(RestartMatch());
    }

    private IEnumerator RestartMatch()
    {
        yield return new WaitForSeconds(3f);
        StartCoroutine(StartMatch());
    }

    private void ResetRobots()
    {
        // Retorna os rob�s �s posi��es iniciais e reseta a velocidade
        playerRobot.transform.position = playerStartPosition.position;
        playerRobot.transform.rotation = playerStartPosition.rotation;
        playerRobot.GetComponent<Rigidbody>().velocity = Vector3.zero;
        playerRobot.GetComponent<Rigidbody>().angularVelocity = Vector3.zero;

        aiRobot.transform.position = aiStartPosition.position;
        aiRobot.transform.rotation = aiStartPosition.rotation;
        aiRobot.GetComponent<Rigidbody>().velocity = Vector3.zero;
        aiRobot.GetComponent<Rigidbody>().angularVelocity = Vector3.zero;
    }
}
