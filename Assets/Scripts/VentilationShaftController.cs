using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class VentilationShaftController : MonoBehaviour
{

    public PosessionController PlayerObject;

    public VentilationShaftController TargetVent;
    private Animator AnimatorController;
    // Start is called before the first frame update
    void Start()
    {
        AnimatorController = GetComponent<Animator>();
        PlayerObject = GameObject.FindGameObjectWithTag("Player").GetComponent<PosessionController>();
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void OnInteraction() {
        StartCoroutine(DoTransition());
    }

    private IEnumerator DoTransition() {
        PlayerObject.gameObject.SetActive(false);
        PlayerObject.PosessionTarget.gameObject.SetActive(false);
        yield return new WaitForSeconds(0.4f);
        PlayerObject.transform.position = TargetVent.transform.position;
        yield return new WaitForSeconds(0.3f);
        PlayerObject.gameObject.SetActive(true);
        PlayerObject.PosessionTarget.gameObject.SetActive(true);
    }
}
