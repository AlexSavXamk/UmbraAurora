using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraController : MonoBehaviour {	
    
    public Transform target;
    public Transform pivot;
    public Transform aimPivot;

    Quaternion rigRotation;
    Quaternion pivotRotation;
    Vector3 pivotEulers;

    public float cameraFollowSpeed;
    public float mouseSensitivity;
    public float tiltMin, tiltMax;
    float lookAngle;
    float tiltAngle;

    CameraCollision camColl;

    public float maxDistance;
    public float minDistance;

    public PlayerCombat combat;
    
	void Start ()
	{
        rigRotation = transform.localRotation;
        pivotEulers = pivot.rotation.eulerAngles;

        Cursor.lockState = CursorLockMode.Locked;

        camColl = FindObjectOfType<CameraCollision>();
        combat = FindObjectOfType<PlayerCombat>();
	}
	
	void LateUpdate () 
	{
        if(GameManager.gm.isGamePaused)
            return;

        /*if(Cursor.lockState == CursorLockMode.None)
        {
            if(Input.GetMouseButtonDown(0))
            {
                Cursor.lockState = CursorLockMode.Locked;
            }

            return;
        }*/
            
        if(Input.GetAxis("Mouse ScrollWheel") > 0 && camColl.maxDistance > minDistance)
		{
			camColl.maxDistance -= 1;
		}
		else if (Input.GetAxis("Mouse ScrollWheel") < 0 && camColl.maxDistance < maxDistance)
		{
			camColl.maxDistance += 1;
		}
        
        float x = Input.GetAxis("Mouse X");
        float y = Input.GetAxis("Mouse Y");

        //y = y > 0 && y > .2f || y < 0 && y < -.2f ? y : 0;
        //x = x > 0 && x > .2f || x < 0 && x < -.2f ? x : 0;

        lookAngle += x * mouseSensitivity;
        rigRotation = Quaternion.Euler(0, lookAngle, 0);
        transform.localRotation = rigRotation;

        tiltAngle -= y * mouseSensitivity;
        tiltAngle = Mathf.Clamp(tiltAngle, tiltMin, tiltMax);
        pivotRotation = Quaternion.Euler(tiltAngle, pivotEulers.y, pivotEulers.z);
        pivot.localRotation = pivotRotation;

        //transform.position = Vector3.Lerp(transform.position, target.position, Time.deltaTime * cameraFollowSpeed);
        if(combat.isAiming)
            transform.position = new Vector3(aimPivot.position.x, aimPivot.position.y, aimPivot.position.z);
        else
            transform.position = new Vector3(target.position.x, target.position.y, target.position.z);
    }
}