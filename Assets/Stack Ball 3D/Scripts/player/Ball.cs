using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Ball : MonoBehaviour
{
    [Header("Elements")]
    private new Rigidbody rigidbody;
    [Header("Setting")]
    private bool smash;

    void Awake()
    {
        rigidbody = GetComponent<Rigidbody>();
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetMouseButtonDown(0))
            smash = true;

        if (Input.GetMouseButtonUp(0))
            smash = false;
    }

    void FixedUpdate()
    {
        if (Input.GetMouseButton(0))
        {
            smash = true;
            rigidbody.velocity = new Vector3(0, -100 * Time.fixedDeltaTime * 7, 0);
        }

        if (rigidbody.velocity.y > 5)
            rigidbody.velocity = new Vector3(rigidbody.velocity.x, 5, rigidbody.velocity.z);
    }

    void OnCollisionEnter(Collision other)
    {
        if (!smash)
            rigidbody.velocity = new Vector3(0, 50 * Time.deltaTime * 5, 0);
    }

    void OnCollisionStay(Collision other)
    {
        if (!smash || other.gameObject.CompareTag(Tags.FINISH))
            rigidbody.velocity = new Vector3(0, 50 * Time.deltaTime * 5, 0);
    }
}
