using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HazmatController : MonoBehaviour
{
    public PosessableController PosessableScript;
    public Animator HazmatAnimator;
    // Start is called before the first frame update
    void Start()
    {
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void OnInteract() {
        PosessableScript.CanBePosessed = true;
        gameObject.SetActive(false);
        HazmatAnimator.SetTrigger("Action");
    }
}
