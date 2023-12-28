using Assets.Scripts.Events;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class BlockManipulator : MonoBehaviour
{
    [SerializeField]
    Transform PlayerCamera;
    [SerializeField] 
    private LayerMask ChunkInteractMask;
    [SerializeField]
    private float maxDistance = 8f;

    public BlockEvent OnBlockPickUp;

    void Update()
    {
        if (Input.GetMouseButtonDown(0))
        {
           

            Ray ray = new Ray(transform.position, transform.forward);

            if (Physics.Raycast(ray, out RaycastHit hitInfo, maxDistance, ChunkInteractMask))
            {

                Vector3 hitPosition = hitInfo.point;
               

                OnBlockPickUp?.Invoke(hitPosition);
            }
            else
            {
                Debug.Log("No block hit");
            }
        }
       
    }
}
