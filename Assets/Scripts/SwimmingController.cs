using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Rigidbody))]
public class SwimmingController : MonoBehaviour
{
    public float waterLevel = 0f;
    public float buoyancyForce = 10f;
    public float underwaterDrag = 3f;

    private Rigidbody rb;
    private float defaultDrag;

    void Start()
    {
        rb = GetComponent<Rigidbody>();
        defaultDrag = rb.drag;
    }

    void FixedUpdate()
    {
        if (transform.position.y < waterLevel)
        {
            rb.drag = underwaterDrag;

            if (Input.GetButton("Fire3"))
            {
                rb.AddForce(Vector3.up * buoyancyForce, ForceMode.Acceleration);
            }
            if (Input.GetButton("Fire1"))
            {
                rb.AddForce(Vector3.down * buoyancyForce, ForceMode.Acceleration);
            }
        }
        else
        {
            rb.drag = defaultDrag;
        }
    }
}
