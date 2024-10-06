using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class RobotController : MonoBehaviour
{
    public WheelCollider leftWheel;
    public WheelCollider rightWheel;

    public float motorForce = 1500f;
    public float brakeForce = 3000f;
    public float downforce = 500f;

    private float motorInput;
    private float steerInput;
    private bool isTurning;

    private Vector2 moveMag;
    private Rigidbody rb;

    private void Awake()
    {
        rb = GetComponent<Rigidbody>();
    }

    private void Update()
    {
        motorInput = Input.GetAxis("Vertical");
        steerInput = Input.GetAxis("Horizontal");

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

    private void FixedUpdate()
    {
        rb.AddForce(-transform.up * downforce, ForceMode.Acceleration);
    }

    private void HandleMotor()
    {
        leftWheel.brakeTorque = 0;
        rightWheel.brakeTorque = 0;

        float leftMotor = motorInput * motorForce;
        float rightMotor = motorInput * motorForce;

        leftWheel.motorTorque = leftMotor;
        rightWheel.motorTorque = rightMotor;
    }

    private void ApplyBraking()
    {
        leftWheel.brakeTorque = brakeForce;
        rightWheel.brakeTorque = brakeForce;
        leftWheel.motorTorque = 0;
        rightWheel.motorTorque = 0;
    }

    private void HandleTurning()
    {
        float leftMotor = steerInput * motorForce;
        float rightMotor = -steerInput * motorForce;

        leftWheel.motorTorque = leftMotor;
        rightWheel.motorTorque = rightMotor;

        leftWheel.brakeTorque = 0;
        rightWheel.brakeTorque = 0;
    }
}
