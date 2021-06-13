using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class InteractableController : MonoBehaviour
{

    public bool CanBeInteractedWith = true;
    public InteractionTag InteractionTag;
    public UnityEvent OnInteraction;

    public void DoInteraction() {
        if (CanBeInteractedWith) {
            OnInteraction.Invoke();
        }
    }
}
