using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Ball : MonoBehaviour
{
    [Header("Elements")]
    private new Rigidbody rigidbody;
    [Header("Setting")]
    private bool smash, invincible;
    private float currentTime;

    public enum BallState
    {
        Prepare, Playing, Died, Finish
    }
    [HideInInspector]
    public BallState ballState = BallState.Prepare;

    void Awake()
    {
        rigidbody = GetComponent<Rigidbody>();
    }

    // Update is called once per frame
    void Update()
    {
        if (ballState == BallState.Playing)
        {
            if (Input.GetMouseButtonDown(0))
                smash = true;

            if (Input.GetMouseButtonUp(0))
                smash = false;

            EventInvincible();
        }

        if (ballState == BallState.Prepare)
        {
            if (Input.GetMouseButtonDown(0))
                ballState = BallState.Playing;
        }

        if (ballState == BallState.Finish)
        {
            if (Input.GetMouseButtonDown(0))
                FindAnyObjectByType<LevelSpawner>().NextLevel();
        }
    }

    void FixedUpdate()
    {
        if (ballState == BallState.Playing)
        {
            if (Input.GetMouseButton(0))
            {
                smash = true;
                rigidbody.velocity = new Vector3(0, -100 * Time.fixedDeltaTime * 7, 0);
            }
        }

        if (rigidbody.velocity.y > 5)
            rigidbody.velocity = new Vector3(rigidbody.velocity.x, 5, rigidbody.velocity.z);
    }

    void EventInvincible()
    {
        if (invincible)
            currentTime -= Time.deltaTime * .35f;
        else
        {
            if (smash)
                currentTime += Time.deltaTime * .8f;
            else
                currentTime -= Time.deltaTime * .5f;
        }

        if (currentTime >= 1)
        {
            currentTime = 1;
            invincible = true;
        }
        else
        {
            currentTime = 0;
            invincible = false;
        }
    }

    void OnCollisionEnter(Collision other)
    {
        if (!smash)
            rigidbody.velocity = new Vector3(0, 50 * Time.deltaTime * 5, 0);
        else
        {
            if (invincible)
            {
                if (other.gameObject.CompareTag(Tags.ENEMY) || other.gameObject.CompareTag(Tags.PLANE))
                    other.transform.parent.GetComponent<StackController>().ShatterAllParts();
            }
            else
            {
                if (other.gameObject.CompareTag(Tags.ENEMY))
                    other.transform.parent.GetComponent<StackController>().ShatterAllParts();

                if (other.gameObject.CompareTag(Tags.PLANE))
                    Debug.Log("Over");
            }
        }

        if (other.gameObject.CompareTag(Tags.FINISH) && ballState == BallState.Playing)
            ballState = BallState.Finish;
    }

    void OnCollisionStay(Collision other)
    {
        if (!smash || other.gameObject.CompareTag(Tags.FINISH))
            rigidbody.velocity = new Vector3(0, 50 * Time.deltaTime * 5, 0);
    }
}
