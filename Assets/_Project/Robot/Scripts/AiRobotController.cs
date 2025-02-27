using UnityEngine;

public class AiRobotController : BaseRobotController
{
    [Header("AI Settings")]
    public Transform target;

    protected override void MoveRobot()
    {
        if (!isGrounded) return;

        if (target == null) return;

        Vector3 direction = (target.position - transform.position).normalized;

        Vector3 moveForce = direction * settings.speed;
        rb.velocity = new Vector3(moveForce.x, rb.velocity.y, moveForce.z);
    }

    protected override void RotateRobot()
    {
        if (!isGrounded) return;

        if (target == null) return;

        Vector3 direction = (target.position - transform.position).normalized;

        Quaternion targetRotation = Quaternion.LookRotation(direction);
        rb.MoveRotation(Quaternion.RotateTowards(rb.rotation, targetRotation, settings.rotationSpeed * Time.fixedDeltaTime));
    }
}
