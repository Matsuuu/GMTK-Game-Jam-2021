using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController : MonoBehaviour
{
    public float Speed;
    public float Acceleration;
    public Vector2 Velocity;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        HandleMovement();
    }

    void HandleMovement()
    {
        float xAxis = Input.GetAxis("Horizontal");
        float yAxis = Input.GetAxis("Vertical");

        Vector3 oldPos = transform.position;
        transform.position = new Vector3(oldPos.x + xAxis * Time.deltaTime, oldPos.y + yAxis * Time.deltaTime, 1);
    }
}
