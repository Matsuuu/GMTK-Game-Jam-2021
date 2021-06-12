using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PosessionController : MonoBehaviour
{
    public GameObject PosessionIndicator;
    public PosessableController PosessableTarget;
    public PosessableController PosessionTarget;
    public CircleCollider2D PosessionRange;
    public List<PosessableController> TargetsInRadius = new List<PosessableController>();
    public PlayerController PlayerControllerScript;
    public bool IsPosessing = false;
    public bool CanCancelPosession = false;
    public BoxCollider2D PlayerCollider;


    private float OriginalPlayerMovespeed;
    private Vector2 OriginalBoxColliderSize;
    private Vector2 OriginalBoxColliderOffset;
    // Start is called before the first frame update
    void Start() {
        PosessionRange = GetComponent<CircleCollider2D>();
        PlayerControllerScript = GetComponent<PlayerController>();
        PlayerCollider = GetComponent<BoxCollider2D>();

        OriginalPlayerMovespeed = PlayerControllerScript.Speed;
        OriginalBoxColliderOffset = PlayerCollider.offset;
        OriginalBoxColliderSize = PlayerCollider.size;
    }

    // Update is called once per frame
    void Update() {
        FindPosessable();
        ManageIndicator();

        HandlePosess();
        FollowPosessed();
    }

    private void HandlePosess() {
        if (Input.GetButtonDown("Posess") && PosessableTarget && PosessableTarget.CanBePosessed && PlayerControllerScript.CanMove) {
            PosessionTarget = PosessableTarget;
            PosessableTarget.Posess();
            PlayerControllerScript.CanMove = false;
            StartCoroutine(HandlePosessionStart());
        }

        // TODO: Checkaa että onko toinen targetti tähtäimessä
        if (IsPosessing && PosessableTarget && Input.GetButtonDown("Posess") && CanCancelPosession) {
            PosessableTarget.EndPosess();
        }
    }

    private IEnumerator HandlePosessionStart() {
        for (int i = 0; i < 60; i++) {
            yield return new WaitForFixedUpdate();
            float step = 2 * Time.deltaTime;
            transform.position = Vector2.MoveTowards(transform.position, PosessionTarget.transform.position, step);
            if (transform.position == PosessionTarget.transform.position) break;
        }

        CopyPosessionStats();

        IsPosessing = true;
        CanCancelPosession = true;
        PlayerControllerScript.CanMove = true;
    }

    private void CopyPosessionStats() {
        Vector2 scaleModifier = new Vector2(PosessionTarget.transform.localScale.x , PosessionTarget.transform.localScale.y);
        BoxCollider2D boxCollider2D = PosessionTarget.GetComponent<BoxCollider2D>();
        PlayerControllerScript.Speed = PosessionTarget.MovementSpeed;
        PlayerCollider.offset = boxCollider2D.offset * scaleModifier;
        PlayerCollider.size = boxCollider2D.size * scaleModifier;
    }

    private void ReturnOriginalStats() {
        PlayerControllerScript.Speed = OriginalPlayerMovespeed;
        PlayerCollider.offset = OriginalBoxColliderOffset;
        PlayerCollider.size = OriginalBoxColliderSize;
    }

    private void FollowPosessed() {
        if (IsPosessing) {
            PosessionTarget.transform.position = transform.position;
            PosessionTarget.transform.rotation = transform.rotation;
        }
    }

    public void EndPosession() {
        IsPosessing = false;
        PosessionTarget = null;
        ReturnOriginalStats();
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
        PosessionIndicator.SetActive(PosessableTarget != null && PosessableTarget.CanBePosessed && PosessionTarget == null);
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
