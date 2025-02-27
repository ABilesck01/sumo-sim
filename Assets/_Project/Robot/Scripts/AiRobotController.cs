using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AiRobotController : MonoBehaviour
{
    public Transform target; // O jogador que será perseguido
    public float speed = 3f;
    public float rotationSpeed = 100f;
    public float gravityMultiplier = 50f;
    public float groundStickForce = 10f;
    public float groundCheckDistance = 0.2f;
    public LayerMask groundLayer;
    public Transform centerOfMass;

    private Rigidbody rb;
    private bool isGrounded;

    private void Awake()
    {
        rb = GetComponent<Rigidbody>();
    }

    private void Start()
    {
        rb.centerOfMass = centerOfMass.localPosition;
    }

    private void FixedUpdate()
    {
        CheckGrounded();

        rb.AddForce(Physics.gravity * gravityMultiplier, ForceMode.Acceleration);

        rb.AddForce(-transform.up * groundStickForce, ForceMode.Acceleration);

        if (!isGrounded) return; // Se não está no chão, não se move

        // Seguir o jogador
        if (target != null)
        {
            Vector3 direction = (target.position - transform.position).normalized;
            direction.y = 0; // Mantém o movimento no plano horizontal

            // Movimentação
            Vector3 moveForce = direction * speed;
            rb.velocity = new Vector3(moveForce.x, rb.velocity.y, moveForce.z);

            // Rotação para mirar no jogador
            Quaternion targetRotation = Quaternion.LookRotation(direction);
            rb.MoveRotation(Quaternion.RotateTowards(rb.rotation, targetRotation, rotationSpeed * Time.fixedDeltaTime));
        }
    }

    private void CheckGrounded()
    {
        // Raycast do centro do robô para baixo, verificando o chão
        isGrounded = Physics.Raycast(transform.position, -transform.up, groundCheckDistance, groundLayer);
    }
}
