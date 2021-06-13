using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DecontaminationController : MonoBehaviour
{
    private Coroutine KillingTimer;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    private void OnTriggerEnter2D(Collider2D collision) {
        if (collision.CompareTag("Player") && collision.GetType() == typeof(BoxCollider2D)) {
            InteractionController interactionCont = collision.GetComponent<InteractionController>();
            if (!interactionCont.HasTag(InteractionTag.DECONTAMINATION)) {
                KillingTimer = StartCoroutine(KillingTimerStart(collision));
            }
        }
    }

    private void OnTriggerExit2D(Collider2D collision) {
        if (collision.CompareTag("Player") && collision.GetType() == typeof(BoxCollider2D)) {
            StopCoroutine(KillingTimer);
            KillingTimer = null;
        }
    }

    IEnumerator KillingTimerStart(Collider2D collision) {
        yield return new WaitForSeconds(0.5f);
        PlayerController player = collision.GetComponent<PlayerController>();
        player.KillPlayer("You cannot survive in water");
    }
}
