using UnityEngine;

public class CarControllerOld : MonoBehaviour
{

    [SerializeField] private WheelCollider frontLeft;
    [SerializeField] private WheelCollider frontRight;
    [SerializeField] private WheelCollider rearLeft;
    [SerializeField] private WheelCollider rearRight;

    public float Acceleration = 500f;
    public float brakingForce = 300f;
    public float maxTurnAngle = 15f;

    private float currentAccelaration = 0f;
    private float currentBrakingForce = 0f;
    private float currentTurnAngle = 0f;

    void Start()
    {
    }

    // Update is called once per frame

    void Update()
    {

        // Get input for acceleration, braking, and steering
        currentAccelaration = Input.GetAxis("Vertical") * Acceleration;
        currentBrakingForce = Input.GetKey(KeyCode.Space) ? brakingForce : 0f;
        currentTurnAngle = Input.GetAxis("Horizontal") * maxTurnAngle;
    }

    public void SetInput()
    {

    }

    private void FixedUpdate()
    {
        // Apply steering
        frontLeft.steerAngle = currentTurnAngle;
        frontRight.steerAngle = currentTurnAngle;

        // Apply acceleration and braking
        frontLeft.motorTorque = currentAccelaration;
        frontRight.motorTorque = currentAccelaration;

        frontLeft.brakeTorque = currentBrakingForce;
        frontRight.brakeTorque = currentBrakingForce;
        rearLeft.brakeTorque = currentBrakingForce;
        rearRight.brakeTorque = currentBrakingForce;


    }
}
