using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraFollow : MonoBehaviour
{
    private Vector3 cameraFollow;
    private Transform ball, win;

    void Awake()
    {
        ball = FindObjectOfType<Ball>().transform;
    }

    // Update is called once per frame
    void Update()
    {
        if (win == null)
            win = GameObject.Find("Win(Clone)").GetComponent<Transform>();

        if (transform.position.y > ball.position.y && transform.position.y > win.position.y + 4f)
            cameraFollow = new Vector3(transform.position.x, ball.position.y, transform.position.z);

        transform.position = new Vector3(transform.position.x, cameraFollow.y, -5f);
    }
}
