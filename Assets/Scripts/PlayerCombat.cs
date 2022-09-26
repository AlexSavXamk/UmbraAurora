using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerCombat : MonoBehaviour{
    
    //Weapon types:
    //1 white magic ball
    //2 black magic ball
    //3
    //4
    //10 dark sword
    //11
    //12
    //13
    //14
    public int WeaponType;
    public GameObject []weaponObjects;
    public float []weaponCooldowns;
    public int []damageAmount;
    //public bool []isWeaponActivated;
    public Transform particlePosition;

    public bool isAttacking;
    public bool isAiming = false;

    PlayerController playerContr;

    public ParticleSystem lightnessParticles;
    public ParticleSystem darknessParticles;

    public Texture lightnessSprite;
    public Texture darknessSprite;
    bool isDarkness = false;
    
    void Start()
    {
        //ActivateWeapon(WeaponType);
        playerContr = GetComponent<PlayerController>();

        /*for(int i = 0; i < WeaponType; i++)zs
        {
            weaponAnim[i] = weaponObjects[i].GetComponent<Animator>();
        }*/
    }

    void Update()
    {
        if(playerContr.isDead || GameManager.gm.isGamePaused)
            return;

        if (Input.GetKeyDown(KeyCode.Alpha1) && !isAttacking)
        {
            //ActivateWeapon(0);
        }

        if (Input.GetKeyDown(KeyCode.R) && !isAttacking)
        {
            ModeTransition();
        }

        if(Input.GetMouseButtonDown(1))
        {
            if(!isAiming)
                isAiming = true;
            else
                isAiming = false;
        }

        if(Input.GetMouseButton(0) && !isAttacking){
            StartCoroutine(Attack());
        }
    }

    IEnumerator Attack()
    {
        switch(WeaponType)
        {
        case 0:
            isAttacking = true;
            Instantiate(weaponObjects[0], particlePosition.position, gameObject.transform.rotation);

            yield return new WaitForSeconds(weaponCooldowns[0]);

            isAttacking = false;
            StopCoroutine(Attack());
            break;
        case 1:
            isAttacking = true;
            Instantiate(weaponObjects[1], particlePosition.position, gameObject.transform.rotation);

            yield return new WaitForSeconds(weaponCooldowns[1]);

            isAttacking = false;
            StopCoroutine(Attack());
            break;
        default:
            StopCoroutine(Attack());
            break;
        }
    }

    public void ModeTransition()
    {
        if(isDarkness)
        {
            isDarkness = false;

            playerContr.theBoyMat.mainTexture = lightnessSprite;
            lightnessParticles.Play();
            WeaponType = 0;
        }else
        {
            isDarkness = true;

            playerContr.theBoyMat.mainTexture = darknessSprite;
            darknessParticles.Play();
            WeaponType = 1;
        }
    }

    public void DeactivateWeapon(int type)
    {
        if(type == 0)
        {
            print("Something went fricking wrong here mate.");
        }
    }
}