using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HazmatController : MonoBehaviour
{
    public PosessableController PosessableScript;
    public Animator HazmatAnimator;

    public AudioSource HazmatAudio;
    public AudioClip GunCock;
    public AudioClip GunManSayBad;
    // Start is called before the first frame update
    void Start()
    {
        HazmatAudio = GetComponent<AudioSource>();
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void OnInteract() {
        PosessionController posessionController = GameObject.FindGameObjectWithTag("Player").GetComponent<PosessionController>();
        Animator gunManAnimator = posessionController.PosessionTarget.GetComponent<Animator>();
        gunManAnimator.SetTrigger("Action");
        StartCoroutine(DoUndress());
    }

    private IEnumerator DoUndress() {
        HazmatAudio.clip = GunManSayBad;
        HazmatAudio.Play();
        yield return new WaitForSeconds(1.4f);
        HazmatAudio.clip = GunCock;
        HazmatAudio.Play();
        yield return new WaitForSeconds(0.5f);
        PosessableScript.CanBePosessed = true;
        gameObject.SetActive(false);
        HazmatAnimator.SetTrigger("Action");
    }
}
