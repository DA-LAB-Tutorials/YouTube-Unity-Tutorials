using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SphereCast : MonoBehaviour
{
    private Ray ray;
    private RaycastHit raycastHit;
    private float sphereCastRadius = 12.0f; 

    // Update is called once per frame
    void Update()
    {

        if (Physics.SphereCast(ray, sphereCastRadius, out raycastHit))
        {
            GameObject sphereGameObject = raycastHit.transform.gameObject;  
        }

    }
}






















