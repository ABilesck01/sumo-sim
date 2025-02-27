using UnityEngine;

[CreateAssetMenu(fileName = "NewRobotSettings", menuName = "Robot/Settings")]
public class RobotSettings : ScriptableObject
{
    public float speed = 3f;
    public float rotationSpeed = 100f;
    public float gravityMultiplier = 50f;
    public float groundStickForce = 10f;
    public float groundCheckDistance = 0.2f;
}
