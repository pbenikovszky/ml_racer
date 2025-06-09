using UnityEngine;
using Unity.MLAgents;
using Unity.MLAgents.Actuators;
using Unity.MLAgents.Sensors;

public class CarDriverAgent : Agent
{

    [SerializeField] private TrackCheckpoints trackCheckpoints;
    [SerializeField] private Transform spawnPosition;

    private CarController _carController;

    private void Awake()
    {
        if (TryGetComponent<CarController>(out var carController))
        {
            _carController = carController;
        }
        else
        {
            Debug.LogError("CarController component not found on this GameObject.");
        }
    }

    private void Start()
    {
        trackCheckpoints.OnCarCorrectCheckpoint += TrackCheckpoints_OnCarCorrectCheckpoint;
        trackCheckpoints.OnCarWrongCheckpoint += TrackCheckpoints_OnCarWrongCheckpoint;
        trackCheckpoints.OnCarFinalCheckpoint += TrackCheckpoints_OnCarFinalCheckpoint;
    }

    private void TrackCheckpoints_OnCarCorrectCheckpoint(object sender, TrackCheckpoints.CarCheckpointsEventArgs e)
    {
        if (e.CarTransform != transform)
        {
            return;
        }

        Debug.Log("Correct checkpoint reached!");
        AddReward(1f);
    }

    private void TrackCheckpoints_OnCarWrongCheckpoint(object sender, TrackCheckpoints.CarCheckpointsEventArgs e)
    {
        if (e.CarTransform != transform)
        {
            return;
        }

        Debug.Log("Wrong checkpoint reached!");
        AddReward(-1f);
    }

    private void TrackCheckpoints_OnCarFinalCheckpoint(object sender, TrackCheckpoints.CarCheckpointsEventArgs e)
    {
        if (e.CarTransform != transform)
        {
            return;
        }

        Debug.Log("Final checkpoint reached!");
        AddReward(5f);
        EndEpisode();
    }


    public override void OnEpisodeBegin()
    {
        transform.position = spawnPosition.position + new Vector3(Random.Range(-1.5f, 1.5f), 0, Random.Range(-1.5f, 1.5f));
        transform.forward = spawnPosition.forward;
        _carController.ResetCar();
        trackCheckpoints.ResetCheckpointsForCar(transform);
    }

    public override void CollectObservations(VectorSensor sensor)
    {
        // Vector3 checkPointForward = trackCheckpoints.GetNextCheckpoint(transform).transform.forward;
        // float directDot = Vector3.Dot(transform.forward, checkPointForward);
        // sensor.AddObservation(directDot);
    }

    public override void OnActionReceived(ActionBuffers actions)
    {
        float horizontalInput = actions.ContinuousActions[0];
        float verticalInput = actions.ContinuousActions[1];
        bool isBraking = actions.DiscreteActions[0] == 1;

        _carController.SetInput(horizontalInput, verticalInput, isBraking);
    }

    public override void Heuristic(in ActionBuffers actionsOut)
    {
        ActionSegment<float> continuousActions = actionsOut.ContinuousActions;
        ActionSegment<int> discreteActions = actionsOut.DiscreteActions;
        continuousActions[0] = Input.GetAxis("Horizontal");
        continuousActions[1] = Input.GetAxis("Vertical");
        discreteActions[0] = Input.GetKey(KeyCode.Space) ? 1 : 0;

    }

    private void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.CompareTag("Wall"))
        {
            AddReward(-0.5f);
            // EndEpisode();
        }
    }

    private void OnCollisionStay(Collision collision)
    {
        if (collision.gameObject.CompareTag("Wall"))
        {
            AddReward(-0.1f);
        }
    }
}
