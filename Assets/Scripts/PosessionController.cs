using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PosessionController : MonoBehaviour
{
    public GameObject PosessionIndicator;
    public PosessableController PosessableTarget;
    public CircleCollider2D PosessionRange;
    public List<PosessableController> TargetsInRadius = new List<PosessableController>();
    public PlayerController PlayerControllerScript;
    public bool IsPosessing = false;
    // Start is called before the first frame update
    void Start() {
        PosessionRange = GetComponent<CircleCollider2D>();
        PlayerControllerScript = GetComponent<PlayerController>();
    }

    // Update is called once per frame
    void Update() {
        FindPosessable();
        ManageIndicator();

        HandlePosess();
    }

    private void HandlePosess() {
        if (Input.GetKeyDown("Posess") && PosessableTarget) {
            PosessableTarget.Posess();
            PlayerControllerScript.CanMove = false;
            IsPosessing = true;
        }

        if (IsPosessing && Input.GetKeyDown("Posess")) {
            PosessableTarget.EndPosess();
        }
    }

    public void EndPosession() {
        PlayerControllerScript.CanMove = true;
    }

    private void FindPosessable() {
        foreach(PosessableController targetInRadius in TargetsInRadius) {
            RaycastHit2D hit = Physics2D.Linecast(transform.position, targetInRadius.transform.position, 3); // Ignore player layer
            if (RaycastHitViableTarget(hit, targetInRadius)) {
                PosessableTarget = targetInRadius;
            }
        }
    }

    private bool RaycastHitViableTarget(RaycastHit2D hit, PosessableController targetInRadius) {
        return hit 
            && hit.transform == targetInRadius.transform 
            && targetInRadius.CanBePosessed;
    }

    private void ManageIndicator() {
        PosessionIndicator.SetActive(PosessableTarget != null);
        if (!PosessableTarget) return;

        PosessionIndicator.transform.position = PosessableTarget.transform.position;
    }

    private void OnTriggerEnter2D(Collider2D collision) {
        PosessableController posessableController = collision.GetComponent<PosessableController>();
        if (posessableController) {
            TargetsInRadius.Add(posessableController);
        }
    }

    private void OnTriggerExit2D(Collider2D collision) {
        PosessableController posessableController = collision.GetComponent<PosessableController>();
        if (posessableController) {
            if (PosessableTarget & posessableController == PosessableTarget) {
                PosessableTarget = null;
            }
            TargetsInRadius.Remove(posessableController);
        }
    }
}
