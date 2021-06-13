using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DoorOpenRemoteController : MonoBehaviour
{
    public DoorController TargetDoor;
    public bool IsUsed;

    public void OnInteract() {
        if (!IsUsed) {
            IsUsed = true;
            TargetDoor.OpenDoor();
        }
    }
}
