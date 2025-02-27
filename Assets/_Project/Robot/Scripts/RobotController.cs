using UnityEngine;
using UnityEngine.InputSystem;

public class RobotController : BaseRobotController
{

    private Vector2 move;

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

        Vector3 moveDirection = transform.forward * move.y * speed;
        rb.velocity = new Vector3(moveDirection.x, rb.velocity.y * 0.1f, moveDirection.z);
    }

    protected override void RotateRobot()
    {
        if (!isGrounded)
        {
            return;
        }

        float rotation = move.x * rotationSpeed * Time.fixedDeltaTime;
        rb.MoveRotation(rb.rotation * Quaternion.Euler(0, rotation, 0));
    }
}
