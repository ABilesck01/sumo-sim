using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerController : MonoBehaviour
{
    public static Action<GameObject> OnPlayerIsReady;

    [SerializeField] private float moveSpeed;
    [SerializeField] private Vector2 position2D;
    [SerializeField] private float distanceFromCenter;

    private bool isReady = false;
    private bool positionSelectionEnabled = false;
    private int areaDirection = 0; // -1 para esquerda, 1 para direita

    public void OnAnyButtonPressed(InputAction.CallbackContext context)
    {
        if (isReady)
            return;

        isReady = true;
        OnPlayerIsReady?.Invoke(this.gameObject);
    }

    public void EnablePositionSelection(int direction)
    {
        positionSelectionEnabled = true;
        areaDirection = direction;
    }

    private void Update()
    {
        if (!positionSelectionEnabled)
            return;

        Vector2 movement = new Vector2(Input.GetAxis("Horizontal"), Input.GetAxis("Vertical"));
        Vector3 newPosition = transform.position + new Vector3(movement.x, 0, movement.y) * moveSpeed * Time.deltaTime;

        if (IsWithinHalfArena(newPosition))
        {
            transform.position = newPosition;
        }

        // Confirmar posição com Enter
        if (Keyboard.current.enterKey.wasPressedThisFrame)
        {
            positionSelectionEnabled = false; // Desativa a movimentação após confirmação
        }
    }

    bool IsWithinHalfArena(Vector3 position)
    {
        position2D = new Vector2(position.x, position.z);
        distanceFromCenter = Vector2.Distance(position2D, GameUtils.instance.arena.transform.position);

        // Verificar se está dentro do raio da arena
        if (distanceFromCenter > GameUtils.instance.arenaRadius)
            return false;

        // Verificar se está na metade correta da arena
        if (areaDirection == -1 && position2D.x > 0) // Jogador na metade esquerda (x <= 0)
            return false;
        if (areaDirection == 1 && position2D.x < 0)  // Jogador na metade direita (x >= 0)
            return false;

        return true;
    }
}
