using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PosessionController : MonoBehaviour
{
    public GameObject PosessionIndicator;
    public PosessableController PosessableTarget;
    public CircleCollider2D PosessionRange;
    public List<PosessableController> TargetsInRadius = new List<PosessableController>();
    // Start is called before the first frame update
    void Start()
    {
        PosessionRange = GetComponent<CircleCollider2D>();
    }

    // Update is called once per frame
    void Update()
    {
        FindPosessable();
    }

    private void FindPosessable()
    {
        foreach(PosessableController targetInRadius in TargetsInRadius)
        {
            Debug.Log("Checking against " + targetInRadius.name);
            RaycastHit2D hit = Physics2D.Linecast(transform.position, targetInRadius.transform.position);
            Debug.Log(hit);
        }

    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        PosessableController posessableController = collision.GetComponent<PosessableController>();
        if (posessableController)
        {
            TargetsInRadius.Add(posessableController);
        }
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        PosessableController posessableController = collision.GetComponent<PosessableController>();
        if (posessableController)
        {
            TargetsInRadius.Remove(posessableController);
        }
    }
}
