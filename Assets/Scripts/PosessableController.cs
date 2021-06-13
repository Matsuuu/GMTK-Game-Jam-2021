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
    private float OriginalMovementSpeed;
    public List<InteractionTag> InteractionTags;
    public List<Transform> PatrolPoints;
    public bool Patrolling = true;
    public bool HidePlayerSprite;

    public int PatrolPointIndex = 0;
    public Transform PatrolTarget;

    private float PosessionStartTime;
    private float PosessionCooldownStartTime;

    private SpriteRenderer PlayerSprite;
    private Coroutine PosessionTimeoutCoroutine;
    private Slider SliderElement;
    private Canvas SliderCanvas;
    private PosessionController PlayerPosessionController;
    private BoxCollider2D PosessableCollider;
    private Animator PosessableAnimator;


    // Start is called before the first frame update
    void Start() {
        OriginalMovementSpeed = MovementSpeed;

        SliderElement = GetComponentInChildren<Slider>();
        SliderCanvas = GetComponentInChildren<Canvas>();
        PosessableCollider = GetComponent<BoxCollider2D>();
        PosessableAnimator = GetComponent<Animator>();
        PlayerSprite = GameObject.FindGameObjectWithTag("Player").GetComponent<SpriteRenderer>();

        SliderElement.maxValue = PosessionTimeInSeconds;
        SliderCanvas.enabled = false;
        PlayerPosessionController = GameObject.FindGameObjectWithTag("Player").GetComponent<PosessionController>();

    }

    private void Awake() {
        if (PatrolPoints.Count > 0) {
            Patrolling = true;
            StartCoroutine(DoPatrol());
        }
    }

    // Update is called once per frame
    void Update() {
        HandlePosessed();
        HandleCooldown();

        HandlePatrol();
    }

    void HandlePatrol() {

    }

    private IEnumerator DoPatrol() {
        PatrolTarget = PatrolPoints[PatrolPointIndex];
        while (Patrolling) {
            yield return new WaitForFixedUpdate();
            float step = 2 * Time.deltaTime * MovementSpeed;
            transform.position = Vector2.MoveTowards(transform.position, PatrolTarget.position, step);

            Vector3 targetLookAtPos = new Vector3(PatrolTarget.position.x, PatrolTarget.position.y, 0);

            Vector3 playerPos = transform.position;
            targetLookAtPos.x -= playerPos.x;
            targetLookAtPos.y -= playerPos.y;

            float angle = Mathf.Atan2(targetLookAtPos.y, targetLookAtPos.x) * Mathf.Rad2Deg;
            angle -= 90;
            transform.rotation = Quaternion.Euler(new Vector3(0, 0, angle));

        }
    }

    private void OnTriggerEnter2D(Collider2D collision) {
        if (collision.CompareTag("PatrolPoint") && PatrolTarget && collision.transform == PatrolTarget.transform) {
            StartCoroutine(ChangePatrolPoint());
        }
    }

    IEnumerator ChangePatrolPoint() {
        MovementSpeed = 0;
        yield return new WaitForSeconds(Random.value * 3);

        PatrolPointIndex++;
        if (PatrolPointIndex >= PatrolPoints.Count) PatrolPointIndex = 0;
        PatrolTarget = PatrolPoints[PatrolPointIndex];

        MovementSpeed = OriginalMovementSpeed;

        Patrolling = false;
        yield return new WaitForSeconds(1);
        Patrolling = true;
        StartCoroutine(DoPatrol());
    }

    void HandlePosessed() {
        if (!IsPosessed) return;

        float posessionTimeRemaining = PosessionTimeInSeconds - (Time.time - PosessionStartTime);
        SliderElement.value = posessionTimeRemaining;
    }

    void HandleCooldown() {
        if (!OnPosessCooldown) return;

        float cooldownTimeRemaining = Time.time - PosessionCooldownStartTime;
        SliderElement.value = cooldownTimeRemaining * 2;
    }

    public void Posess() {
        CanBePosessed = false;
        IsPosessed = true;
        PosessionStartTime = Time.time;
        SliderCanvas.enabled = true;
        PosessableCollider.enabled = false;
        PosessableAnimator.speed = 1.5f;
        Patrolling = false;
        MovementSpeed = OriginalMovementSpeed;
        PosessableAnimator.SetTrigger("Posess");
        if (HidePlayerSprite) {
            PlayerSprite.enabled = false;
        }

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

        if (HidePlayerSprite) {
            PlayerSprite.enabled = true;
        }
        PlayerPosessionController.EndPosession();

        StartCoroutine(PosessionCooldown());
    }

    private IEnumerator PosessionCooldown() {
        PosessionCooldownStartTime = Time.time;
        OnPosessCooldown = true;
        yield return new WaitForSeconds(PosessionTimeInSeconds / 2);
        OnPosessCooldown = false;
        CanBePosessed = true;
        PosessionCooldownStartTime = 0;
        SliderCanvas.enabled = false;
        Patrolling = true;
    }
}
