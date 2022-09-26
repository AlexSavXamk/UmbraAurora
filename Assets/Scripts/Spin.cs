using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Spin : MonoBehaviour{
    
    public float spinSpeed = 1;

    void Start()
    {
        
    }

    void Update()
    {
        this.transform.eulerAngles = new Vector3(0, this.transform.eulerAngles.y + spinSpeed * Time.deltaTime,0);
    }
}
