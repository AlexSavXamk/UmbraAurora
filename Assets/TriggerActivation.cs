using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TriggerActivation : MonoBehaviour{
    
    
    
    void Start()
    {
        
    }

    void Update()
    {
        
    }

    void OnTriggerEnter(Collider other)
    {
        if(other.gameObject.tag == "PlayerActivator")
        {
            GameplayManager.gameplayManager.ToggleBetweenDarkAndLightMusic();
        }
    }
}
