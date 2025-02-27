using UnityEngine;
using UnityEngine.InputSystem;

public class RobotController : MonoBehaviour
{
    public float speed = 3f;
    public float rotationSpeed = 100f;
    public float gravityMultiplier = 50f;
    public float groundStickForce = 10f;
    public Transform centerOfMass;
    public float groundCheckDistance = 0.2f; // Dist�ncia do ch�o para considerar que est� aterrissado
    public LayerMask groundLayer; // Camada do ch�o

    private Vector2 move;
    private Rigidbody rb;
    private bool isGrounded;

    public void GetMoveInput(InputAction.CallbackContext callbackContext)
    {
        move = callbackContext.ReadValue<Vector2>();
    }

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

        if (!isGrounded)
        {
            return;
        }

        Vector3 moveDirection = transform.forward * move.y * speed;
        rb.velocity = new Vector3(moveDirection.x, rb.velocity.y * 0.1f, moveDirection.z);

        // Rota��o do rob�
        float rotation = move.x * rotationSpeed * Time.fixedDeltaTime;
        rb.MoveRotation(rb.rotation * Quaternion.Euler(0, rotation, 0));
    }

    private void CheckGrounded()
    {
        // Raycast do centro do rob� para baixo, verificando o ch�o
        isGrounded = Physics.Raycast(transform.position, -transform.up, groundCheckDistance, groundLayer);
    }
}
