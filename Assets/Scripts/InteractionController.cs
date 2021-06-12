using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InteractionController : MonoBehaviour
{
    public GameObject InteractionIndicator;
    public bool CanInteract;
    public List<InteractionTag> InteractionTags = new List<InteractionTag>();

    public InteractableController InteractionTarget;
    public List<InteractableController> InteractionTargetsInRange = new List<InteractableController>();
    // Start is called before the first frame update
    void Start()
    {
        CanInteract = true;
    }

    // Update is called once per frame
    void Update()
    {
        FindInteractable();
        ManageIndicator();

        ManageInteraction();
    }

    private void ManageInteraction() {
        if (Input.GetButtonDown("Interact") && CanInteract && InteractionTarget) {
            InteractionTarget.DoInteraction();
        }
    }

    private void ManageIndicator() {
        InteractionIndicator.SetActive(InteractionTarget && CanInteract && InteractionTarget.CanBeInteractedWith);
        if (!InteractionTarget) return;

        InteractionIndicator.transform.position = InteractionTarget.transform.position;
    }

    private void FindInteractable() {
        foreach (InteractableController targetInRadius in InteractionTargetsInRange) {
            RaycastHit2D hit = Physics2D.Linecast(transform.position, targetInRadius.transform.position, 3); // Ignore player layer

            if (RaycastHitViableTarget(hit, targetInRadius)) {
                InteractionTarget = targetInRadius;
            }
        }
    }

    public void UpdateInteractionTags(List<InteractionTag> newTags) {
        InteractionTags = newTags;
    }

    public void ClearInteractionTags() {
        InteractionTags = new List<InteractionTag>();
        InteractionTarget = null;
    }

    private bool RaycastHitViableTarget(RaycastHit2D hit, InteractableController interactionTarget) {
        return hit
            && CanInteract
            && InteractionTags.Contains(interactionTarget.InteractionTag)
            && hit.transform == interactionTarget.transform
            && interactionTarget.CanBeInteractedWith;
    }

    private void OnTriggerEnter2D(Collider2D collision) {
        InteractableController interactionTarget = collision.GetComponent<InteractableController>();
        if (interactionTarget) {
            InteractionTargetsInRange.Add(interactionTarget);
        }
    }

    private void OnTriggerExit2D(Collider2D collision) {
        InteractableController interactionTarget = collision.GetComponent<InteractableController>();
        if (interactionTarget) {
            if (interactionTarget == InteractionTarget) {
                InteractionTarget = null;
            }
            InteractionTargetsInRange.Remove(interactionTarget);
        }
    }
}
