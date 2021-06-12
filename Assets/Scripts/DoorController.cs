using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DoorController : MonoBehaviour
{
    public Transform DoorOne;
    public Transform DoorTwo;

    public int PixelMovementCount;



    private void Start() {
    }
    public void OnInteract() {
        GetComponent<InteractableController>().CanBeInteractedWith = false;
        StartCoroutine(OpenDoors());
    }

    private IEnumerator OpenDoors() {
        Vector3 DoorOnePosition = DoorOne.transform.localPosition;
        Vector3 DoorTwoPosition = DoorTwo.transform.localPosition;
        for (int i = 0; i < PixelMovementCount; i++) {
            DoorOne.transform.localPosition = new Vector2(DoorOnePosition.x, DoorOnePosition.y += 0.01f);
            DoorTwo.transform.localPosition= new Vector2(DoorTwoPosition.x, DoorTwoPosition.y -= 0.01f);
            yield return new WaitForFixedUpdate();
        }
    }
}
