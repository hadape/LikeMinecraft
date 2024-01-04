using Assets.Scripts.Classes;
using Assets.Scripts.Events;
using System;
using System.Collections.Generic;
using UnityEngine;

public class Inventory : MonoBehaviour
{
    public Dictionary<Enums.BlockType, int> PlayerInventory { get; private set; } = new Dictionary<Enums.BlockType, int>();
    public Enums.BlockType ActiveType { get; private set; }

    private BlockManipulator _blockManipulator;

    public event EventHandler<OnInvetoryEventArgs> OnInventoryChanged;
    public event EventHandler<OnInvetoryEventArgs> OnInventoryActiveTypeChanged;

    private void Start()
    {
        _blockManipulator = GetComponentInParent<BlockManipulator>();
        InitInventory();
        SubscribeToEvents();
    }

    private void InitInventory()
    {
        PlayerInventory.Add(Enums.BlockType.Grass, 0);
        PlayerInventory.Add(Enums.BlockType.Dirt, 0);
        PlayerInventory.Add(Enums.BlockType.Rock, 0);
        PlayerInventory.Add(Enums.BlockType.Snow, 0);
        SetActiveType(Enums.BlockType.Grass);
    }

    // Update is called once per frame
    private void Update()
    {
        if (Input.anyKeyDown)
        {
            SetActiveTypeBasedOnInput();
        }
    }
    private void SubscribeToEvents()
    {
        _blockManipulator.OnBlockPickUp += OnBlockPickup;
        _blockManipulator.OnBlockPlaced += OnBlockPlaced;
    }

    private void SetActiveTypeBasedOnInput()
    {
        if (Input.GetKeyDown(KeyCode.Alpha1)) SetActiveType(Enums.BlockType.Grass);
        else if (Input.GetKeyDown(KeyCode.Alpha2)) SetActiveType(Enums.BlockType.Dirt);
        else if (Input.GetKeyDown(KeyCode.Alpha3)) SetActiveType(Enums.BlockType.Rock);
        else if (Input.GetKeyDown(KeyCode.Alpha4)) SetActiveType(Enums.BlockType.Snow);
    }

    private void SetActiveType(Enums.BlockType type)
    {
        ActiveType = type;
        OnInventoryActiveTypeChanged?.Invoke(this,new OnInvetoryEventArgs {BlockType = type });
    }


    private void UpdateInventory(Enums.BlockType blockType, int changeAmount)
    {
        PlayerInventory[blockType] = PlayerInventory[blockType] + changeAmount;
        OnInventoryChanged?.Invoke(this,new OnInvetoryEventArgs { BlockType = blockType,Value = PlayerInventory[blockType] });
    }

    private void OnBlockPlaced(object sender, OnBlockEventArgs e)
    {
        UpdateInventory(e.BlockType, -1);

        if (PlayerInventory[e.BlockType] < 0)
        {
            PlayerInventory[e.BlockType] = 0;
        }
    
    }

    private void OnBlockPickup(object sender, OnBlockEventArgs e)
    {
        UpdateInventory(e.BlockType, 1);
    }
}
