using Assets.Scripts.Events;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class BlockManipulator : MonoBehaviour
{
    [SerializeField]
    Transform PlayerCamera;
    [SerializeField] 
    private LayerMask ChunkInteractMask;
    [SerializeField]
    private float maxDistance = 8f;

    public event EventHandler<OnBlockPickupEventArgs> OnBlockPickUp;

    void Update()
    {
        Debug.DrawRay(PlayerCamera.position, PlayerCamera.forward * maxDistance, Color.red);
        if (Input.GetMouseButtonDown(0))
        {
           

            Ray ray = new Ray(PlayerCamera.position, PlayerCamera.forward);

            if (Physics.Raycast(ray, out RaycastHit hitInfo, maxDistance, ChunkInteractMask))
            {
                Debug.Log("Block hit");
                Vector3 targetPoint = hitInfo.point - hitInfo.normal * .1f;

                Vector3Int targetBlock = new Vector3Int
                {
                    x = Mathf.RoundToInt(targetPoint.x),
                    y = Mathf.RoundToInt(targetPoint.y),
                    z = Mathf.RoundToInt(targetPoint.z)
                };


                OnBlockPickUp?.Invoke(this, new OnBlockPickupEventArgs { BlockPosition = targetBlock });
            }
            else
            {
                Debug.Log("No block hit");
            }
        }
       
    }
}
