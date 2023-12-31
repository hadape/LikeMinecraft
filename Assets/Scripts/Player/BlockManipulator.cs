using Assets.Scripts.Classes;
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
    bool isMining = false;

    [SerializeField]
    private GameObject _worldObject;
    private World _world;

    private Vector3Int currentTargetBlock;
    [SerializeField]
    private float currentBlockHealth;
    private Enums.BlockType currentBlockType;

    public event EventHandler<OnBlockEventArgs> OnBlockPickUp;
    public event EventHandler<OnBlockEventArgs> OnBlockPlaced;

    private void Start()
    {
        _world = _worldObject.GetComponent<World>();
    }

    void Update()
    {
        Debug.DrawRay(PlayerCamera.position, PlayerCamera.forward * maxDistance, Color.red);
        //PickupBlock();
        MiningBlock();
        PlaceBlock();

    }
    private void MiningBlock()
    {
        if (Input.GetMouseButton(0))
        {
            if (!isMining)
            {
                StartMining();
            }
            else
            {
                MineBlock();
            }
        }
        else if (isMining)
        {
            ResetMining();
        }
    }

    private void MineBlock()
    {
        if (currentTargetBlock != null)
        {
            currentBlockHealth -= Time.deltaTime; // Snížení HP v èase
            if (currentBlockHealth <= 0)
            {
                OnBlockPickUp?.Invoke(this, new OnBlockEventArgs { BlockPosition = currentTargetBlock });

                ResetMining();
            }
        }
    }

    private void ResetMining()
    {
        isMining = false;
        currentTargetBlock = Vector3Int.zero;
        currentBlockHealth = 0;
    }

    private void StartMining()
    {
        Ray ray = new Ray(PlayerCamera.position, PlayerCamera.forward);
        if (Physics.Raycast(ray, out RaycastHit hitInfo, maxDistance, ChunkInteractMask))
        {
            Vector3 targetPoint = hitInfo.point - hitInfo.normal * .1f;

            currentTargetBlock = new Vector3Int
            {
                x = Mathf.RoundToInt(targetPoint.x),
                y = Mathf.RoundToInt(targetPoint.y),
                z = Mathf.RoundToInt(targetPoint.z)
            };

            currentBlockType = _world.GetTypeOfBlock(currentTargetBlock);
            currentBlockHealth = MinningBlockHp.GetHpByBlockType(currentBlockType);
            isMining = true;
        }
    }



    private void PickupBlock()
    {
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


                OnBlockPickUp?.Invoke(this, new OnBlockEventArgs { BlockPosition = targetBlock });
            }
            else
            {
                Debug.Log("No block hit");
            }
        }
    }

    private void PlaceBlock()
    {
        if (Input.GetMouseButtonDown(1))
        {


            Ray ray = new Ray(PlayerCamera.position, PlayerCamera.forward);

            if (Physics.Raycast(ray, out RaycastHit hitInfo, maxDistance, ChunkInteractMask))
            {
                Debug.Log("Placing block");
                Vector3 targetPoint = hitInfo.point + hitInfo.normal * .1f;

                Vector3Int targetBlock = new Vector3Int
                {
                    x = Mathf.RoundToInt(targetPoint.x),
                    y = Mathf.RoundToInt(targetPoint.y),
                    z = Mathf.RoundToInt(targetPoint.z)
                };


                OnBlockPlaced?.Invoke(this, new OnBlockEventArgs { BlockPosition = targetBlock });
            }
            else
            {
                //Debug.Log("No block hit");
            }
        }
    }
}
