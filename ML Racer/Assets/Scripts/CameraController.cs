using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraController : MonoBehaviour
{

    [SerializeField] private CarController target;

    private Vector3 offsetDir;
    [SerializeField] private float smoothSpeed = 7f;
    [SerializeField] private float minHeight = 12f;
    [SerializeField] private float maxHeight = 15f;

    public bool isSmoothing = true;

    // Start is called before the first frame update
    void Start()
    {
        offsetDir = (transform.position - target.transform.position).normalized;
    }

    void FixedUpdate()
    {
        if (target == null) return;

        Vector3 desiredPosition = target.transform.position + (offsetDir * minHeight);

        transform.position = isSmoothing
            ? Vector3.Lerp(transform.position, desiredPosition, smoothSpeed * Time.deltaTime)
            : desiredPosition;

        transform.LookAt(target.transform);
    }

    // Update is called once per frame
    void Update()
    {


    }
}
