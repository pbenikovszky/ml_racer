using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Downforce : MonoBehaviour
{
    [SerializeField] private float downforceAmount = 100f;
    private Rigidbody rb;

    void Start()
    {
        rb = GetComponent<Rigidbody>();
        rb.centerOfMass = new Vector3(0, -0.5f, 0);
    }

    void FixedUpdate()
    {
        rb.AddForce(downforceAmount * rb.velocity.magnitude * -transform.up);
    }
}
