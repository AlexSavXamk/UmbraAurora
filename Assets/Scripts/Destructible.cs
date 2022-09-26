using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Destructible : MonoBehaviour{
    
    int health = 15;

    public GameObject destroyedVersion;
    
    void Start()
    {
        if(destroyedVersion != null)
            destroyedVersion.SetActive(false);
    }

    void Update()
    {
        
    }

    public void Damage(int amount)
    {
        health -= amount;

        //Debug.Log(gameObject.name + " damaged, health reduced to " + health);

        if(health <= 0)
        {
            Destroy();
        }
    }

    public void Destroy()
    {
        if(destroyedVersion != null)
        {
            destroyedVersion.SetActive(true);
        }

        gameObject.SetActive(false);
    }
}
