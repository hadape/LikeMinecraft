using Assets.Scripts.Classes;
using Assets.Scripts.Events;
using System;
using UnityEngine;

public class BlockManipulator : MonoBehaviour
{
    [SerializeField]
    private Transform _playerCamera;
    [SerializeField]
    private LayerMask _chunkInteractMask;
    [SerializeField]
    private LayerMask _boundCheckMask;
    [SerializeField]
    private float _maxDistance = 8f;
    private bool _isMining = false;

    [SerializeField]
    private GameObject _worldObject;
    private World _world;

    private Vector3Int _currentTargetBlock;
    [SerializeField]
    private float _currentBlockHealth;
    private Enums.BlockType _currentBlockType;
    [SerializeField]
    private GameObject _invetoryObject;
    private Inventory _invetory;

    public event EventHandler<OnBlockEventArgs> OnBlockPickUp;
    public event EventHandler<OnBlockEventArgs> OnBlockPlaced;

    private void Start()
    {
        _world = _worldObject.GetComponent<World>();
        _invetory = _invetoryObject.GetComponent<Inventory>();
    }

    private void Update()
    {
        Debug.DrawRay(_playerCamera.position, _playerCamera.forward * _maxDistance, Color.red);
        MiningBlock();
        PlaceBlock();

    }
    private void MiningBlock()
    {
        if (Input.GetMouseButton(0))
        {
            if (!_isMining)
            {
                StartMining();
            }
            else
            {
                MineBlock();
            }
        }
        else if (_isMining)
        {
            ResetMining();
        }
    }

    private void MineBlock()
    {
        if (_currentTargetBlock != null)
        {
            _currentBlockHealth -= Time.deltaTime;
            if (_currentBlockHealth <= 0)
            {
                OnBlockPickUp?.Invoke(this, new OnBlockEventArgs { BlockPosition = _currentTargetBlock, BlockType = _currentBlockType });
                ResetMining();
            }
        }
    }

    private void ResetMining()
    {
        _isMining = false;
        _currentTargetBlock = Vector3Int.zero;
        _currentBlockHealth = 0;
    }

    private void StartMining()
    {
        Ray ray = new Ray(_playerCamera.position, _playerCamera.forward);
        if (Physics.Raycast(ray, out RaycastHit hitInfo, _maxDistance, _chunkInteractMask))
        {
            Vector3 targetPoint = hitInfo.point - hitInfo.normal * .1f;

            _currentTargetBlock = new Vector3Int
            {
                x = Mathf.RoundToInt(targetPoint.x),
                y = Mathf.RoundToInt(targetPoint.y),
                z = Mathf.RoundToInt(targetPoint.z)
            };

            _currentBlockType = _world.GetTypeOfBlock(_currentTargetBlock);
            _currentBlockHealth = MinningBlockHp.GetHpByBlockType(_currentBlockType);
            _isMining = true;
        }
    }

    private void PlaceBlock()
    {

        if (Input.GetMouseButtonDown(1))
        {
            var blockType = _invetory.ActiveType;
            if (_invetory.PlayerInventory[blockType] > 0)
            {
                Ray ray = new Ray(_playerCamera.position, _playerCamera.forward);

                if (Physics.Raycast(ray, out RaycastHit hitInfo, _maxDistance, _chunkInteractMask))
                {
                    Vector3 targetPoint = hitInfo.point + hitInfo.normal * .1f;

                    Vector3Int targetBlock = new Vector3Int
                    {
                        x = Mathf.RoundToInt(targetPoint.x),
                        y = Mathf.RoundToInt(targetPoint.y),
                        z = Mathf.RoundToInt(targetPoint.z)
                    };

                    if (Physics.CheckBox(targetBlock, Vector3.one * .5f, Quaternion.identity, _boundCheckMask) == false)
                    {
                        OnBlockPlaced?.Invoke(this, new OnBlockEventArgs { BlockPosition = targetBlock, BlockType = blockType });
                    }

                }
            }

        }
    }
}
