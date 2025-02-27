using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class BaseRobotController : MonoBehaviour
{
    public float speed = 3f;
    public float rotationSpeed = 100f;
    public float gravityMultiplier = 50f;
    public float groundStickForce = 10f;
    public Transform centerOfMass;
    public float groundCheckDistance = 0.2f;
    public LayerMask groundLayer;

    protected Rigidbody rb;
    protected bool isGrounded;

    protected virtual void Awake()
    {
        rb = GetComponent<Rigidbody>();
    }

    protected virtual void Start()
    {
        if (centerOfMass != null)
        {
            rb.centerOfMass = centerOfMass.localPosition;
        }
    }

    protected virtual void FixedUpdate()
    {
        CheckGrounded();

        // Aplica gravidade extra
        rb.AddForce(Physics.gravity * gravityMultiplier, ForceMode.Acceleration);

        // Mantém o robô preso ao chão
        rb.AddForce(-transform.up * groundStickForce, ForceMode.Acceleration);

        if (!isGrounded)
        {
            return;
        }

        MoveRobot();
        RotateRobot();
    }

    protected abstract void MoveRobot();
    protected abstract void RotateRobot();

    protected void CheckGrounded()
    {
        // Raycast do centro do robô para baixo, verificando o chão
        isGrounded = Physics.Raycast(transform.position, -transform.up, groundCheckDistance, groundLayer);
    }
}
