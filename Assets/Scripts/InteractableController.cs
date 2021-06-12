using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class InteractableController : MonoBehaviour
{

    public bool CanBeInteractedWith = true;
    public InteractionTag InteractionTag;
    public UnityEvent OnInteraction;
    // TODO: Make ionteraction callback
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void DoInteraction() {
        OnInteraction.Invoke();
    }
}
