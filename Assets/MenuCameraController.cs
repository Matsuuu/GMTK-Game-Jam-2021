using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MenuCameraController : MonoBehaviour
{
    public Transform StartingPoint;
    public Transform EndingPoint;

    public Transform CurrentTarget;
    private float ZAxis;
    private float PreviousY;
 
    // Start is called before the first frame update
    void Start()
    {
        CurrentTarget = EndingPoint.transform;
        ZAxis = transform.position.z;
    }

    // Update is called once per frame
    void Update()
    {
        Vector2 newPos = Vector2.MoveTowards(transform.position, CurrentTarget.position, Time.deltaTime * 1.25f);
        transform.position = new Vector3(newPos.x, newPos.y, ZAxis);
        Debug.Log(transform.position.y);
        if (transform.position.y == PreviousY) {
            PreviousY = -9999;
            CurrentTarget = CurrentTarget == EndingPoint ? StartingPoint : EndingPoint;
        }
        else {
            PreviousY = transform.position.y;
        }
    }
}
