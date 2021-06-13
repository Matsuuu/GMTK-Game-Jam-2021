using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DoorOpenRemoteController : MonoBehaviour
{
    public List<DoorController> TargetDoors;
    public bool IsUsed;

    public void OnInteract() {
        if (!IsUsed) {
            IsUsed = true;
            foreach (DoorController TargetDoor in TargetDoors) {
                TargetDoor.OpenDoor();
            }
        }
    }
}
