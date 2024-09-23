using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StackPartController : MonoBehaviour
{
    private new Rigidbody rigidbody;
    private MeshRenderer meshRenderer;
    private StackController stackController;
    private new Collider collider;

    void Awake()
    {
        rigidbody = GetComponent<Rigidbody>();
        meshRenderer = GetComponent<MeshRenderer>();
        stackController = transform.parent.GetComponent<StackController>();
        collider = GetComponent<Collider>();
    }

    public void Shatter()
    {
        rigidbody.isKinematic = false;
        collider.enabled = false;

        Vector3 forcePoint = transform.parent.position;
        float paretXpos = transform.position.x;
        // returns the center point of the bounding box of the mesh attached to the MeshRenderer component.
        // This is useful for various calculations, such as positioning objects relative to the mesh or determining the mesh’s spatial properties.
        float xPos = meshRenderer.bounds.center.x;

        Vector3 subDir = (paretXpos - xPos < 0) ? Vector3.right : Vector3.left;
        Vector3 dir = (Vector3.up * 1.5f + subDir).normalized;

        float force = Random.Range(20, 35);
        float torque = Random.Range(110, 180);

        // rigidbody.AddForceAtPosition: applies a force to a Rigidbody at a specific position in world coordinates.
        // This can result in both a force and a torque being applied to the object, which is useful for simulating effects like explosions or impacts.
        // ForceMode.Impulse: resulting in an immediate change in velocity. This is particularly useful for simulating sudden impacts or explosions.
        rigidbody.AddForceAtPosition(dir * force, forcePoint, ForceMode.Impulse);
        // rigidbody.AddTorque: applies a torque to a Rigidbody, causing it to rotate.
        // This can be useful for simulating rotational forces, such as spinning objects or applying rotational impulses.
        rigidbody.AddTorque(Vector3.left * torque);
        rigidbody.velocity = Vector3.down;
    }

    public void RemoveAllChilds()
    {
        for (int i = 0; i < transform.childCount; i++)
        {
            transform.GetChild(i).SetParent(null);
            i--;
        }
    }
}
