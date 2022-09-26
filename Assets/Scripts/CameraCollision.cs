using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraCollision : MonoBehaviour
{

    public float minDistance;
    public float maxDistance;
    public float smooth = 5.0f;
    Vector3 dollyDir;
    //Vector3 dollyDirAdjusted;
    public float distance;
    public LayerMask whatToCollideWith;

    //PlayerWeapon _playerWeapon;
    public Vector3 aimingOffset;

    void Start()
    {
        //_playerWeapon = FindObjectOfType<PlayerWeapon>();
        dollyDir = transform.localPosition.normalized;
        distance = transform.localPosition.magnitude;
    }

    void Update()
    {
        Vector3 desiredCameraPos = transform.parent.TransformPoint(dollyDir * maxDistance);
        RaycastHit hit;

        if(Physics.Linecast(transform.parent.position, desiredCameraPos, out hit, whatToCollideWith))
        {
            distance = Mathf.Clamp((hit.distance * 0.87f), minDistance, maxDistance);
        }
        else
        {
            distance = maxDistance;
        }

        transform.localPosition = Vector3.Lerp(transform.localPosition, dollyDir * distance, Time.deltaTime * smooth);

        if(false)//_playerWeapon.isAiming)
        {
            transform.localPosition = transform.localPosition + aimingOffset;
        }
    }
}
