using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class PlayerController : MonoBehaviour
{
    public float Speed;
    public float Acceleration;
    public bool CanMove = true;

    // Start is called before the first frame update
    void Start()
    {
        
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
        if (!CanMove) return;
            
        float xAxis = Input.GetAxis("Horizontal") * Speed;
        float yAxis = Input.GetAxis("Vertical") * Speed;

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
        StartCoroutine(DoDeath());
    }

    private IEnumerator DoDeath() {
        //TODO: Death animation
        yield return new WaitForSeconds(3);
        SceneManager.LoadScene(SceneManager.GetActiveScene().name);
    }
}
