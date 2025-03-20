using System.Linq;
using UnityEngine;
using UnityEngine.InputSystem;

public class RobotController : BaseRobotController
{
    private Vector2 move;

    private PlayerInput playerInput;

    protected override void Awake()
    {
        base.Awake();

        playerInput = GetComponent<PlayerInput>();
    }

    public void GetMoveInput(InputAction.CallbackContext callbackContext)
    {
        move = callbackContext.ReadValue<Vector2>();
    }

    protected override void MoveRobot()
    {
        if (!isGrounded)
        {
            return;
        }

        Vector3 moveDirection = transform.forward * move.y * settings.speed;
        rb.velocity = new Vector3(moveDirection.x, rb.velocity.y * 0.1f, moveDirection.z);
    }

    protected override void RotateRobot()
    {
        if (!isGrounded)
        {
            return;
        }

        float rotation = move.x * settings.rotationSpeed * Time.fixedDeltaTime;
        rb.MoveRotation(rb.rotation * Quaternion.Euler(0, rotation, 0));
    }

    private void Update()
    {
        Debug.Log(playerInput.devices.First().name);
    }
}
