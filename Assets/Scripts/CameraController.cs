using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraController : MonoBehaviour
{
    private Transform Target;
    private Vector3 Velocity = Vector3.zero;
    private Camera CameraComponent;

    public float Damp = 0.15f;
    // Start is called before the first frame update
    void Start()
    {
        Target = GameObject.FindGameObjectWithTag("Player").transform;
        CameraComponent = GetComponent<Camera>();
    }

    // Update is called once per frame
    void Update()
    {
        FollowPlayer();
    }

    private void FollowPlayer()
    { 
        if (!Target) return;

        Vector3 targetPoint = CameraComponent.WorldToViewportPoint(Target.position);
        Vector3 delta = Target.position - CameraComponent.ViewportToWorldPoint(new Vector3(0.5f, 0.5f, targetPoint.z));

        Vector3 destination = transform.position + delta;
        transform.position = Vector3.SmoothDamp(transform.position, destination, ref Velocity, Damp);
    }
}
