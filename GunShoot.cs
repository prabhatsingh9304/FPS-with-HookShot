using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GunShoot : MonoBehaviour
{
    public Transform gunPoint;
    public float shootingRange = 100f;
    public LayerMask enemyLayer;
    public GameObject bulletImpact;
    void Update()
    {
        if(Input.GetButtonDown("Fire1"))
        {
            Shoot();
        }
        
    }
    void Shoot()
    {
        if(Physics.Raycast(gunPoint.transform.position,gunPoint.transform.forward,out RaycastHit hit,shootingRange))
        {
            Debug.Log(hit.transform.name);
            GameObject tempImpact = Instantiate(bulletImpact,hit.point + (hit.normal * 0.002f),Quaternion.LookRotation(hit.normal,Vector3.up));
            Destroy(tempImpact,3f);
        }
    }
    
}
