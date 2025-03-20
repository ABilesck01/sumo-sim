using System.Collections;
using TMPro;
using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerManager : MonoBehaviour
{
    [Header("Player")]
    public Transform pointA;
    public Transform pointB;
    [Header("Materials")]
    public Material playerAMaterial;
    public Material playerBMaterial;
    [Header("AI")]
    public AiRobotController aiRobotController;
    public int timerToAI = 5;
    [Header("UI")]
    public GameObject screen;
    public TextMeshProUGUI txtMessage;
    public TextMeshProUGUI txtTimer;

    private bool firstPlayer = false;

    public void OnPlayerJoin(PlayerInput playerInput)
    {
        if (firstPlayer)
        {
            playerInput.transform.position = pointB.position;
            playerInput.transform.rotation = pointB.rotation;
            MatchController.instance.playerBRobot = playerInput.GetComponent<BaseRobotController>();
            MatchController.instance.playerBRobot.SetMaterial(playerBMaterial);
            firstPlayer = false;
            StopAllCoroutines();
            StartMatch();
        }
        else
        {
            playerInput.transform.position = pointA.position;
            playerInput.transform.rotation = pointA.rotation;
            MatchController.instance.playerARobot = playerInput.GetComponent<BaseRobotController>();
            MatchController.instance.playerARobot.SetMaterial(playerAMaterial);
            firstPlayer = true;

            txtMessage.text = "Jogador 1 conectado. Aguardando jogador 2...";
            StartCoroutine(WaitForPlayer());
        }
    }

    private IEnumerator WaitForPlayer()
    {
        int currentSeconds = timerToAI;
        while (currentSeconds > 0)
        {
            txtTimer.text = $"Jogar contra IA em {currentSeconds.ToString()} segundos...";
            yield return new WaitForSeconds(1f);
            currentSeconds--;
        }
        PlayerInputManager.instance.DisableJoining();
        AiRobotController aiRobot = Instantiate(aiRobotController, pointB.position, pointB.rotation);
        MatchController.instance.playerBRobot = aiRobot;
        StartMatch();
    }

    private void StartMatch()
    {
        

        screen.SetActive(false);
        MatchController.instance.StartMatch();
        txtMessage.text = "";
        txtTimer.text = "";
    }
}
