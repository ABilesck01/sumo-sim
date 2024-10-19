using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerController : MonoBehaviour
{
    public static Action<GameObject> OnPlayerIsReady;

    private bool isReady = false;

    public void OnAnyButtonPressed(InputAction.CallbackContext context)
    {
        if (isReady)
            return;

        isReady = true;
        OnPlayerIsReady?.Invoke(this.gameObject);
    }
}
