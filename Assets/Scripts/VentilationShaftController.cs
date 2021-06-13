using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class VentilationShaftController : MonoBehaviour
{

    public PosessionController PlayerObject;

    public VentilationShaftController TargetVent;
    private Animator AnimatorController;
    private AudioSource VentAudio;
    // Start is called before the first frame update
    void Start()
    {
        AnimatorController = GetComponent<Animator>();
        PlayerObject = GameObject.FindGameObjectWithTag("Player").GetComponent<PosessionController>();
        VentAudio = GetComponent<AudioSource>();
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void OnInteraction() {
        StartCoroutine(DoTransition());
    }

    private IEnumerator DoTransition() {
        AnimatorController.SetTrigger("Open");
        SpriteRenderer PlayerSprite = PlayerObject.GetComponent<SpriteRenderer>();
        SpriteRenderer PosessionTargetSprite = PlayerObject.PosessionTarget.GetComponent<SpriteRenderer>();
        PlayerController PlayerController = PlayerObject.GetComponent<PlayerController>();

        VentAudio.Play();
        PlayerSprite.enabled = false;
        PosessionTargetSprite.enabled = false;
        PlayerController.CanMove = false;
        yield return new WaitForSeconds(0.4f);
        PlayerObject.transform.position = TargetVent.transform.position;
        yield return new WaitForSeconds(0.3f);

        PlayerSprite.enabled = true;
        PosessionTargetSprite.enabled = true;
        PlayerController.CanMove = true;
    }
}
