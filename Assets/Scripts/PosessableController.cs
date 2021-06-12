using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PosessableController : MonoBehaviour
{
    public bool CanBePosessed = false;
    public bool IsPosessed = false;
    public bool OnPosessCooldown = false;
    public int PosessionTimeInSeconds = 10;
    public int PosessionCooldownInSeconds = 20;

    private float PosessionStartTime;

    private Coroutine PosessionTimeoutCoroutine;

    public Slider SliderElement;
    public PosessionController PlayerPosessionController;


    // Start is called before the first frame update
    void Start() {
        SliderElement = gameObject.GetComponentInChildren<Slider>();
        SliderElement.enabled = false;
        PlayerPosessionController = GameObject.FindGameObjectWithTag("Player").GetComponent<PosessionController>();
    }

    // Update is called once per frame
    void Update() {
        HandlePosessed();
    }

    void HandlePosessed() {
        if (!IsPosessed) return;

        float posessionTimeRemaining = Time.time - PosessionStartTime;
        SliderElement.value = posessionTimeRemaining;
    }

    public void Posess() {
        CanBePosessed = false;
        IsPosessed = true;
        PosessionStartTime = Time.time;

        PosessionTimeoutCoroutine = StartCoroutine(PosessionTimeout());
    }

    private IEnumerator PosessionTimeout() {
        yield return new WaitForSeconds(PosessionTimeInSeconds);
        EndPosess();
    }

    public void EndPosess() {
        StopCoroutine(PosessionTimeoutCoroutine);

        IsPosessed = false;
        OnPosessCooldown = true;
        PosessionStartTime = 0;
        PlayerPosessionController.EndPosession();

        StartCoroutine(PosessionCooldown());
    }

    private IEnumerator PosessionCooldown() {
        yield return new WaitForSeconds(PosessionCooldownInSeconds);
        OnPosessCooldown = false;
        CanBePosessed = true;
    }
}
