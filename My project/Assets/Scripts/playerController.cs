using System.Collections;
using System.Collections.Generic;
using System.Data;
using UnityEngine;

public class playerController : MonoBehaviour
{
    [SerializeField] CharacterController controller;

    [SerializeField] int HP;
    [SerializeField] float playerSpeed;
    [SerializeField] int jumpMax;
    [SerializeField] float jumpForce;
    [SerializeField] float gravity;

    [SerializeField] int shootDamage;
    [SerializeField] int shootDist;
    [SerializeField] float shootRate;

    Vector3 move;
    Vector3 playerVel;
    int jumpCount;
    bool isShooting;
    int HPOrig;

    // Start is called before the first frame update
    void Start()
    {
        HPOrig = HP;
        updatePlayerUI();
    }

    // Update is called once per frame
    void Update()
    {
        if (!gameManager.instance.isPaused)
        {
            Debug.DrawRay(Camera.main.transform.position, Camera.main.transform.forward * shootDist, Color.red);

            movement();

            if (Input.GetButton("Shoot") && !isShooting)
            {
                StartCoroutine(shoot());
            }
        }

    }

    void movement()
    {
        if (controller.isGrounded)
        {
            jumpCount = 0;
            playerVel = Vector3.zero;
        }

        move = Input.GetAxis("Horizontal") * transform.right
            + Input.GetAxis("Vertical") * transform.forward;
        controller.Move(move * playerSpeed * Time.deltaTime);

        if (Input.GetButtonDown("Jump") && jumpCount < jumpMax)
        {
            playerVel.y = jumpForce;
            jumpCount++;
        }

        playerVel.y += gravity * Time.deltaTime;
        controller.Move(playerVel * Time.deltaTime);
    }

    IEnumerator shoot ()
    {
        isShooting = true;

        RaycastHit hit;
        if (Physics.Raycast(Camera.main.ViewportPointToRay(new Vector2(0.5f, 0.5f)), out hit, shootDist))
        {
            IDamage dmg = hit.collider.GetComponent<IDamage>();

            if (hit.transform != transform && dmg != null)
            {
                dmg.takeDamage(shootDamage);
            }
        }

        yield return new WaitForSeconds(shootRate);
        isShooting = false;
    }

    public void takeDamage(int amount)
    {
        HP -= amount;
        updatePlayerUI();

        if (HP <= 0)
        {
            gameManager.instance.youLose();
        }
    }

    void updatePlayerUI()
    {
        gameManager.instance.playerHPBar.fillAmount = (float)HP / HPOrig;
    }
}
