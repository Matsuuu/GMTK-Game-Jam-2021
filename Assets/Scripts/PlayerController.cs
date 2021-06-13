using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class PlayerController : MonoBehaviour
{
    public float Speed;
    public float Acceleration;
    public bool CanMove = true;
    public bool IsMoving = false;
    public GameObject DeathCanvas;
    public Text DeathCanvasReasonText;
    public GameObject WinCanvas;
    private Animator PlayerAnimator;

    public AudioSource PlayerAudio;
    public AudioClip DeathAudio;
    public AudioClip WalkAudio;

    public bool StageWon = false;

    // Start is called before the first frame update
    void Start()
    {
        PlayerAnimator = GetComponent<Animator>();
        DeathCanvas.SetActive(false);
    }

    // Update is called once per frame
    void FixedUpdate()
    {
        HandleMovement();
        HandleRotation();
    }

    private void Update() {
        HandleRestart();
        HandleBackToMenu();
        HandleMovementSounds();
        HandleStageWon();
    }

    private void HandleMovementSounds() {
        if (IsMoving) {
            if (PlayerAudio.clip != WalkAudio) {
                PlayerAudio.clip = WalkAudio;
            }
            if (!PlayerAudio.isPlaying) {
                PlayerAudio.Play();
                PlayerAudio.loop = true;
            }
        } else {
            if (PlayerAudio.clip == WalkAudio) {
                PlayerAudio.Stop();
            }
        }
    }

    private void HandleStageWon() {
        if (Input.GetButtonDown("Interact") && StageWon) {
            SceneManager.LoadScene("MenuScene");
        }
    }

    private void HandleRestart() {
        if (Input.GetButtonDown("Restart")) {
            SceneManager.LoadScene(SceneManager.GetActiveScene().name);
        }
    }

    private void HandleBackToMenu() {
        if (Input.GetButtonDown("Escape")) {
            SceneManager.LoadScene("MenuScene");
        }
    }

    void HandleMovement()
    {
        if (!CanMove) {
            IsMoving = false;
            return;
        }
            
        float xAxis = Input.GetAxis("Horizontal") * Speed;
        float yAxis = Input.GetAxis("Vertical") * Speed;

        IsMoving = xAxis != 0 || yAxis != 0;

        Vector3 oldPos = transform.position;
        transform.position = new Vector3(oldPos.x + xAxis * Time.deltaTime, oldPos.y + yAxis * Time.deltaTime, 0);
    }

    void HandleRotation() {
        Vector3 mousePos = Input.mousePosition;
        mousePos.z = 0;

        Vector3 playerPos = Camera.main.WorldToScreenPoint(transform.position);
        mousePos.x -= playerPos.x;
        mousePos.y -= playerPos.y;

        float angle = Mathf.Atan2(mousePos.y, mousePos.x) * Mathf.Rad2Deg;
        angle -= 90;
        transform.rotation = Quaternion.Euler(new Vector3(0, 0, angle));
    }

    public void KillPlayer(string deathReason) {
        CanMove = false;    
        StartCoroutine(DoDeath(deathReason));
    }

    private IEnumerator DoDeath(string deathReason) {
        PlayerAnimator.SetTrigger("Death");
        PlayerAudio.loop = false;
        PlayerAudio.clip = DeathAudio;
        PlayerAudio.volume = 0.6f;
        PlayerAudio.Play();
        yield return new WaitForSeconds(2);
        DeathCanvas.SetActive(true);
        DeathCanvasReasonText.text = deathReason;
        yield return new WaitForSeconds(3);
        SceneManager.LoadScene(SceneManager.GetActiveScene().name);
    }

    public void WinStage() {
        CanMove = false;
        StageWon = true;
        WinCanvas.SetActive(true);
    }
}
