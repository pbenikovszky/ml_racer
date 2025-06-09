using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TrackCheckpoints : MonoBehaviour
{

    public class CarCheckpointsEventArgs : EventArgs
    {
        public Transform CarTransform { get; }

        public CarCheckpointsEventArgs(Transform carTransform)
        {
            CarTransform = carTransform;
        }
    }

    public event EventHandler<CarCheckpointsEventArgs> OnCarCorrectCheckpoint;
    public event EventHandler<CarCheckpointsEventArgs> OnCarWrongCheckpoint;
    public event EventHandler<CarCheckpointsEventArgs> OnCarFinalCheckpoint;

    [SerializeField] private List<Transform> carTransforms = new();

    private readonly List<Checkpoint> checkpointList = new();
    private readonly List<int> nextCheckpointIndexList = new();

    private void Awake()
    {
        Transform checkPointTransforms = transform.Find("Checkpoints");
        if (checkPointTransforms == null)
        {
            Debug.LogError("No Checkpoints found in TrackCheckpoints script.");
            return;
        }

        foreach (Transform checkpoint in checkPointTransforms)
        {
            if (checkpoint == null)
            {
                Debug.LogError("Checkpoint transform is null.");
                continue;
            }

            if (!checkpoint.TryGetComponent<Checkpoint>(out Checkpoint checkpointComponent))
            {
                Debug.LogError("Checkpoint component not found on " + checkpoint.name);
                continue;
            }

            checkpointList.Add(checkpointComponent);
            checkpointComponent.SetTrackCheckpoints(this);

            foreach (Transform carTransform in carTransforms)
            {
                if (carTransform == null)
                {
                    Debug.LogError("Car transform is null.");
                    continue;
                }

                if (!carTransform.TryGetComponent<CarController>(out CarController carController))
                {
                    Debug.LogError("CarController component not found on " + carTransform.name);
                    continue;
                }

                nextCheckpointIndexList.Add(0);
            }
        }
    }

    public void ResetCheckpointsForCar(Transform carTransform)
    {
        int carTransformIndex = carTransforms.IndexOf(carTransform);
        if (carTransformIndex < 0 || carTransformIndex >= nextCheckpointIndexList.Count)
        {
            Debug.LogError("Car transform not found in the list of car transforms.");
            return;
        }

        nextCheckpointIndexList[carTransformIndex] = 0; // Reset to the first checkpoint
    }

    public void ResetAllCheckpoints()
    {
        nextCheckpointIndexList.Clear();
        foreach (Transform carTransform in carTransforms)
        {
            if (carTransform == null)
            {
                Debug.LogError("Car transform is null.");
                continue;
            }

            nextCheckpointIndexList.Add(0);
        }
    }

    public void CheckpointReached(Checkpoint checkpoint, Transform carTransform)
    {
        int carTransformIndex = carTransforms.IndexOf(carTransform);
        int nextCheckpointIndex = nextCheckpointIndexList[carTransformIndex];
        int checkpointIndex = checkpointList.IndexOf(checkpoint);
        if (checkpointIndex == nextCheckpointIndex)
        {
            nextCheckpointIndexList[carTransformIndex] = (nextCheckpointIndex + 1) % checkpointList.Count; // Move to the next checkpoint
            OnCarCorrectCheckpoint?.Invoke(this, new CarCheckpointsEventArgs(carTransform));
            if (nextCheckpointIndex + 1 >= checkpointList.Count)
            {
                OnCarFinalCheckpoint?.Invoke(this, new CarCheckpointsEventArgs(carTransform));
            }
        }
        else
        {
            OnCarWrongCheckpoint?.Invoke(this, new CarCheckpointsEventArgs(carTransform));
        }
    }

    public Checkpoint GetNextCheckpoint(Transform carTransform)
    {
        int carTransformIndex = carTransforms.IndexOf(carTransform);
        int nextCheckpointIndex = nextCheckpointIndexList[carTransformIndex];

        return checkpointList[nextCheckpointIndex];
    }

}
