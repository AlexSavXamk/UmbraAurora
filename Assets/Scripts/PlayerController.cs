using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class PlayerController : MonoBehaviour{
    
    public TextMeshProUGUI healthText;
    public Image healthBar;
    public SkinnedMeshRenderer[] meshes;
    public Material theBoyMat;
    float damageCooldown = 0.5f;
    public int health = 20;
    int maxHealth;
    public bool isDead = false;

    public float movementSppeed;
	public float jumpForce;
	public float gravity;

    Transform camMain;
    internal Vector3 moveDir;
	CharacterController controller;

    bool isMouseLocked;
    bool hasLanded;

    float h;
    float v;

    //CameraController cameraCont;
    PlayerCombat combat;

    float oldMovementSpeed;
    float x;

    public Animator playerAnimR;

    public AudioSource playerHurt;
    AudioSource footstepsAudio;
    public AudioClip[] audioClips;
    public AudioClip boiHurt;
    public LayerMask feetMask;

	void Start () 
    {
        maxHealth = health;
        healthText.text = health + "/" + maxHealth;
        
        theBoyMat = meshes[0].material;
        foreach(SkinnedMeshRenderer m in meshes)
        {
            m.material = theBoyMat;
        }

        theBoyMat.SetColor("_EmissionColor", Color.black);

        controller = GetComponent<CharacterController> ();
        camMain = Camera.main.transform;
        //cameraCont = FindObjectOfType<CameraController>();
        combat = GetComponent<PlayerCombat>();
        footstepsAudio = GetComponent<AudioSource>();
	}
	
	void Update () 
    {
        if(isDead || GameManager.gm.isGamePaused)
            return;
        
        damageCooldown -= 1 * Time.deltaTime;

        h = Input.GetAxisRaw("Horizontal");
        v = Input.GetAxisRaw("Vertical");

        //h = h > 0 && h > .3f ? 1 : h < 0 && h < -.3f ? -1 : 0;
        //v = v > 0 && v > .3f ? 1 : v < 0 && v < -.3f ? -1 : 0;

        float initialY = moveDir.y;

        moveDir = v * camMain.forward + h * camMain.right;

        if (moveDir.magnitude > 1)
            moveDir.Normalize();

        moveDir *= movementSppeed;
        moveDir.y = initialY;

        if(combat.isAttacking){
            playerAnimR.SetInteger("Hands", 1);
        }else{
            playerAnimR.SetInteger("Hands", 0);
        }

		if (controller.isGrounded) 
        {
            if (Input.GetButtonDown("Jump"))
            {
                moveDir.y = jumpForce;
            }

            float hh = h;
            hh = Mathf.Abs(hh);
            float vv = v;
            vv = Mathf.Abs(vv);

            if(hh >= 0.1f || vv >= 0.1f){
                //Walking animation
                if(!footstepsAudio.isPlaying)
                    footstepsAudio.Play();
                    
                playerAnimR.SetInteger("Animation", 1);
            }else{
                //Idle animation
                footstepsAudio.Stop();
                playerAnimR.SetInteger("Animation", 0);
            }
		}

        if(!controller.isGrounded)
        {
            footstepsAudio.Stop();
            hasLanded = false;
            moveDir.y -= gravity * Time.deltaTime;

            if(controller.velocity.y >= 0){
                //Jump animation
                playerAnimR.SetInteger("Animation", 2);
            }else{
                //Falling animation
                playerAnimR.SetInteger("Animation", 3);
            }
        }

        if(controller.isGrounded && !hasLanded){
            hasLanded = true;
            moveDir.y = -10;
        }

        //if(h != 0 && v != 0)
        //RotatePlayer(camMain.eulerAngles.y);

        if(combat.isAiming)
        {
            RotatePlayer(camMain.eulerAngles.y);

            if(v == -1){
                playerAnimR.SetFloat("RunSpeedMultiplier", -1);
            }else{
                playerAnimR.SetFloat("RunSpeedMultiplier", 1);
            }
        }else
        {
            playerAnimR.SetFloat("RunSpeedMultiplier", 1);

            if (h != 0)
                RotatePlayer(h > 0 ? camMain.eulerAngles.y + 90 : camMain.eulerAngles.y - 90);

            if(v != 0)
                RotatePlayer(v > 0 ? camMain.eulerAngles.y : camMain.eulerAngles.y - 180);

            if (h != 0 && v != 0)
            {
                if (v > 0)
                    RotatePlayer(h > 0 ? camMain.eulerAngles.y + 45 : camMain.eulerAngles.y - 45);

                if (v < 0)
                    RotatePlayer(h > 0 ? camMain.eulerAngles.y + 135 : camMain.eulerAngles.y - 135);
            }
        }

		controller.Move(moveDir * Time.deltaTime);
        
        /*RaycastHit hit;
        //Ray footstepRay = new Ray (transform.position, Vector3.down);

        if(Physics.Raycast(transform.position, transform.TransformDirection(Vector3.down), out hit, 3, feetMask))
        {
            Debug.DrawRay(transform.position, transform.TransformDirection(Vector3.down) * 3);

            if(hit.transform.GetComponent<MeshRenderer>().material.name == "Stone" || hit.transform.GetComponent<MeshRenderer>().material.name == "Ruins")
            {
                footstepsAudio.clip = audioClips[1];
                print("Walking on stone");
            }else
            {
                footstepsAudio.clip = audioClips[0];
                print("Walking on dirt");
            }
        }*/
	}

    void RotatePlayer(float degreesY)
    {
        //transform.rotation = Quaternion.Euler(0, degreesY, 0);
        transform.rotation = Quaternion.RotateTowards(transform.rotation, Quaternion.Euler(0, degreesY, 0), 400 * Time.deltaTime);
    }

    void DamagePlayer(int amount)
    {
        if(damageCooldown > 0)
            return;

        damageCooldown = 0.5f;
        //print("Player damaged by: " + amount);
        
        StartCoroutine(Damaged());
        
        health -= amount;
        health = Mathf.Clamp(health, 0, maxHealth);

        healthText.text = health + "/" + maxHealth;
        healthBar.fillAmount = (float) health / maxHealth;

        playerHurt.PlayOneShot(boiHurt);

        if(health <= 0)
            Death();
    }

    void HealPlayer(int amount)
    {
        //print("Player healed by: " + amount);
        
        health += amount;
        health = Mathf.Clamp(health, 0, maxHealth);

        healthText.text = health + "/" + maxHealth;
        healthBar.fillAmount = (float) health / maxHealth;
    }

    void OnTriggerEnter(Collider other)
    {
        if(other.gameObject.tag == "Spikes" ||other.gameObject.tag == "EnemyMelee")
            DamagePlayer(8);

        if(other.gameObject.tag == "HealthPickup" && health != maxHealth)
        {
            HealPlayer(12);
            other.gameObject.SetActive(false);
        }
            

        if(other.gameObject.tag == "Stone"){
            //Stone footsteps
            footstepsAudio.clip = audioClips[1];
            footstepsAudio.Play();
        }
        else{
            //Dirt footsteps
            footstepsAudio.clip = audioClips[0];
        }
    }

    void Death()
    {   
        footstepsAudio.Stop();
        isDead = true;
        //Dying animation
        playerAnimR.SetInteger("Animation", 4);
    }

    IEnumerator Damaged()
    {
        theBoyMat.SetColor("_EmissionColor", Color.red);
        yield return new WaitForSeconds(0.2f);
        theBoyMat.SetColor("_EmissionColor", Color.black);
    } 
}