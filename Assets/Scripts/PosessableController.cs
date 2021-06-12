using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PosessableController : MonoBehaviour
{
    public bool CanBePosessed = true;
    public bool IsPosessed = false;
    public bool OnPosessCooldown = false;
    public int PosessionTimeInSeconds = 10;
    public float MovementSpeed = 2;
    public List<InteractionTag> InteractionTags;

    private float PosessionStartTime;
    private float PosessionCooldownStartTime;

    private Coroutine PosessionTimeoutCoroutine;
    private Slider SliderElement;
    private Canvas SliderCanvas;
    private PosessionController PlayerPosessionController;
    private BoxCollider2D PosessableCollider;
    private Animator PosessableAnimator;


    // Start is called before the first frame update
    void Start() {
        CanBePosessed = true;
        SliderElement = GetComponentInChildren<Slider>();
        SliderCanvas = GetComponentInChildren<Canvas>();
        PosessableCollider = GetComponent<BoxCollider2D>();
        PosessableAnimator = GetComponent<Animator>();

        SliderElement.maxValue = PosessionTimeInSeconds;
        SliderCanvas.enabled = false;
        PlayerPosessionController = GameObject.FindGameObjectWithTag("Player").GetComponent<PosessionController>();
    }

    // Update is called once per frame
    void Update() {
        HandlePosessed();
        HandleCooldown();
    }

    void HandlePosessed() {
        if (!IsPosessed) return;

        float posessionTimeRemaining = PosessionTimeInSeconds - (Time.time - PosessionStartTime);
        SliderElement.value = posessionTimeRemaining;
    }

    void HandleCooldown() {
        if (!OnPosessCooldown) return;

        float cooldownTimeRemaining = Time.time - PosessionCooldownStartTime;
        Debug.Log(cooldownTimeRemaining);
        SliderElement.value = cooldownTimeRemaining;
    }

    public void Posess() {
        CanBePosessed = false;
        IsPosessed = true;
        PosessionStartTime = Time.time;
        SliderCanvas.enabled = true;
        PosessableCollider.enabled = false;
        PosessableAnimator.speed = 1.5f;

        PosessionTimeoutCoroutine = StartCoroutine(PosessionTimeout());
    }

    private IEnumerator PosessionTimeout() {
        yield return new WaitForSeconds(PosessionTimeInSeconds);
        EndPosess();
    }

    public void EndPosess() {
        StopCoroutine(PosessionTimeoutCoroutine);

        PosessableCollider.enabled = true;
        IsPosessed = false;
        PosessionStartTime = 0;
        PosessableAnimator.speed = 1;
        PlayerPosessionController.EndPosession();

        StartCoroutine(PosessionCooldown());
    }

    private IEnumerator PosessionCooldown() {
        PosessionCooldownStartTime = Time.time;
        OnPosessCooldown = true;
        yield return new WaitForSeconds(PosessionTimeInSeconds);
        OnPosessCooldown = false;
        CanBePosessed = true;
        PosessionCooldownStartTime = 0;
        SliderCanvas.enabled = false;
    }
}
