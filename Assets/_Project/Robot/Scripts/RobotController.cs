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
    public float maxSteerAngle = 30f;

    private float motorInput;
    private float steerInput;
    private float brakeInput;

    private void Update()
    {
        motorInput = Input.GetAxis("Vertical");
        steerInput = Input.GetAxis("Horizontal");
        brakeInput = Input.GetKey(KeyCode.Space) ? 1f : 0f;

        HandleMotor();
        ApplyBraking();
    }

    private void HandleMotor()
    {
        float leftMotor = motorInput * motorForce + steerInput * motorForce;
        float rightMotor = motorInput * motorForce - steerInput * motorForce;

        leftWheel.motorTorque = leftMotor;
        rightWheel.motorTorque = rightMotor;
    }

    private void ApplyBraking()
    {
        if (brakeInput > 0f)
        {
            leftWheel.brakeTorque = brakeInput * brakeForce;
            rightWheel.brakeTorque = brakeInput * brakeForce;
        }
        else
        {
            leftWheel.brakeTorque = 0f;
            rightWheel.brakeTorque = 0f;
        }
    }
}
