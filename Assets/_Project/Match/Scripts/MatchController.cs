using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class MatchController : MonoBehaviour
{
    public BaseRobotController playerARobot; // Referência ao robô do jogador
    public BaseRobotController playerBRobot; // Referência ao robô da IA
    public Transform playerStartPosition; // Posição inicial do jogador
    public Transform aiStartPosition; // Posição inicial da IA
    public float arenaRadius = 5f; // Raio da arena
    public float countdownTime = 3f; // Tempo da contagem regressiva
    public TextMeshProUGUI countdownText; // UI para exibir a contagem regressiva

    private bool matchInProgress = false;

    public static MatchController instance;

    private void Awake()
    {
        instance = this;
    }

    public void StartMatch()
    {
        StartCoroutine(StartMatchCoroutine());
    }

    private IEnumerator StartMatchCoroutine()
    {
        yield return null;

        playerARobot.SetMatchActive(false);
        playerBRobot.SetMatchActive(false);

        matchInProgress = false;
        ResetRobots(); // Retorna os robôs às posições iniciais

        // Contagem regressiva
        for (float i = countdownTime; i > 0; i--)
        {
            countdownText.text = i.ToString();
            yield return new WaitForSeconds(1f);
        }

        countdownText.text = "VAI!";
        yield return new WaitForSeconds(1f);
        countdownText.text = "";

        matchInProgress = true;
        
        playerARobot.SetMatchActive(true);
        playerBRobot.SetMatchActive(true);

        // Verifica constantemente se algum robô saiu da arena
        while (matchInProgress)
        {
            yield return new WaitForSeconds(0.5f); // Pequena pausa para otimizar performance
            CheckForWinner();
        }
    }

    private void CheckForWinner()
    {
        float playerDistance = Vector3.Distance(playerARobot.transform.position, Vector3.zero);
        float aiDistance = Vector3.Distance(playerBRobot.transform.position, Vector3.zero);

        // Se ambos saírem, vence o que estiver mais próximo do centro
        if (playerDistance > arenaRadius && aiDistance > arenaRadius)
        {
            if (playerDistance > aiDistance)
            {
                EndMatch("VERMELHO ganhou!");
            }
            else
            {
                EndMatch("AZUL ganhou!");
            }
        }
        else if (playerDistance > arenaRadius)
        {
            EndMatch("VERMELHO ganhou!");
        }
        else if (aiDistance > arenaRadius)
        {
            EndMatch("AZUL ganhou!");
        }
    }

    private void EndMatch(string winnerMessage)
    {
        playerARobot.SetMatchActive(false);
        playerBRobot.SetMatchActive(false);

        matchInProgress = false;
        countdownText.text = winnerMessage;
        StartCoroutine(RestartMatch());
    }

    private IEnumerator RestartMatch()
    {
        yield return new WaitForSeconds(3f);
        StartCoroutine(StartMatchCoroutine());
    }

    private void ResetRobots()
    {
        // Retorna os robôs às posições iniciais e reseta a velocidade
        playerARobot.transform.position = playerStartPosition.position;
        playerARobot.transform.rotation = playerStartPosition.rotation;
        playerARobot.GetComponent<Rigidbody>().velocity = Vector3.zero;
        playerARobot.GetComponent<Rigidbody>().angularVelocity = Vector3.zero;

        playerBRobot.transform.position = aiStartPosition.position;
        playerBRobot.transform.rotation = aiStartPosition.rotation;
        playerBRobot.GetComponent<Rigidbody>().velocity = Vector3.zero;
        playerBRobot.GetComponent<Rigidbody>().angularVelocity = Vector3.zero;
    }
}
