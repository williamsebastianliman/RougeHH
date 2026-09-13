using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Billboard : MonoBehaviour
{
    [SerializeField] private bool damageText; 
    private Camera mainCamera;
    private Vector3 initialLocalPosition;

    private void Start()
    {
        mainCamera = Camera.main;
        initialLocalPosition = transform.localPosition;
    }

    void LateUpdate()
    {
        if(!damageText)
        {
            transform.localPosition = initialLocalPosition;
        }
        
        transform.LookAt(transform.position + mainCamera.transform.forward);
    }
}
