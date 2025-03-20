using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class BaseRobotController : MonoBehaviour
{
    [Header("Robot Settings")]
    [SerializeField] protected RobotSettings settings;
    [SerializeField] protected GameObject gfx;
    [Header("Ground Settings")]
    [SerializeField] protected Transform centerOfMass;
    [SerializeField] protected LayerMask groundLayer;

    protected Rigidbody rb;
    protected bool isGrounded;
    [SerializeField] protected bool isMatchActive = false;

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
        if (!isMatchActive || settings == null) return;

        CheckGrounded();

        // Aplica gravidade extra
        rb.AddForce(Physics.gravity * settings.gravityMultiplier, ForceMode.Acceleration);

        // Mantém o robô preso ao chão
        rb.AddForce(-transform.up * settings.groundStickForce, ForceMode.Acceleration);

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
        isGrounded = Physics.Raycast(transform.position, -transform.up, settings.groundCheckDistance, groundLayer);
    }

    public void SetMatchActive(bool active)
    {
        gfx.SetActive(true);
        rb.velocity = Vector3.zero;
        rb.isKinematic = !active;
        isMatchActive = active;
    }
}
