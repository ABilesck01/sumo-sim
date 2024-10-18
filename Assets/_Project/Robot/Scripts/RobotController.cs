using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.InputSystem;

public class RobotController : MonoBehaviour
{
    public WheelCollider[] leftWheel;
    public WheelCollider[] rightWheel;
    public Transform centerOfMass;

    public float motorForce = 1500f;
    public float brakeForce = 3000f;
    public float downforce = 500f;

    private float motorInput;
    private float steerInput;
    private bool isTurning;

    private Vector2 moveMag;
    private Vector2 move;
    private Rigidbody rb;

    public void GetMoveInput(InputAction.CallbackContext callbackContext)
    {
        move = callbackContext.ReadValue<Vector2>();
        steerInput = move.x;
        motorInput = move.y;
    }

    private void Awake()
    {
        rb = GetComponent<Rigidbody>();
    }

    private void Start()
    {
        if(centerOfMass != null)
            rb.centerOfMass = centerOfMass.position;
    }

    private void Update()
    {
        //motorInput = Input.GetAxis("Vertical");
        //steerInput = Input.GetAxis("Horizontal");

        if (Mathf.Abs(steerInput) > 0.1f && !isTurning)
        {
            ApplyBraking();
            isTurning = true;
        }
        else if (Mathf.Abs(steerInput) <= 0.1f)
        {
            isTurning = false;
        }

        if (!isTurning)
        {
            moveMag = new Vector2(motorInput, steerInput).normalized;
            if (moveMag.magnitude > 0.1f)
            {
                HandleMotor();
            }
            else
            {
                ApplyBraking();
            }
        }
        else
        {
            HandleTurning();
        }

    }

    private void HandleMotor()
    {
        for (int i = 0; i < leftWheel.Length; i++)
        {
            leftWheel[i].brakeTorque = 0;
        }
        for (int i = 0; i < rightWheel.Length; i++)
        {
            rightWheel[i].brakeTorque = 0;
        }

        float leftMotor = motorInput * motorForce;
        float rightMotor = motorInput * motorForce;

        for (int i = 0; i < leftWheel.Length; i++)
        {
            leftWheel[i].motorTorque = leftMotor;
        }
        for (int i = 0; i < rightWheel.Length; i++)
        {
            rightWheel[i].motorTorque = rightMotor;
        }
    }

    private void ApplyBraking()
    {
        for (int i = 0; i < leftWheel.Length; i++)
        {
            leftWheel[i].brakeTorque = brakeForce;
            leftWheel[i].motorTorque = 0;
        }
        for (int i = 0; i < rightWheel.Length; i++)
        {
            rightWheel[i].brakeTorque = brakeForce;
            rightWheel[i].motorTorque = 0;
        }
    }

    private void HandleTurning()
    {
        for (int i = 0; i < leftWheel.Length; i++)
        {
            leftWheel[i].brakeTorque = 0;
        }
        for (int i = 0; i < rightWheel.Length; i++)
        {
            rightWheel[i].brakeTorque = 0;
        }

        float leftMotor = steerInput * motorForce;
        float rightMotor = -steerInput * motorForce;

        for (int i = 0; i < leftWheel.Length; i++)
        {
            leftWheel[i].motorTorque = leftMotor;
        }
        for (int i = 0; i < rightWheel.Length; i++)
        {
            rightWheel[i].motorTorque = rightMotor;
        }
    }
}
