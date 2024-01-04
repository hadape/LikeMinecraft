using Assets.Scripts.Classes;
using Assets.Scripts.Events;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Inventory : MonoBehaviour
{
    [SerializeField]
    Canvas uiCanvas;
    InventoryUI _inventoryUI;



    public Dictionary<Enums.BlockType, int> PlayerInventory { get; private set; } = new Dictionary<Enums.BlockType, int>();
    public Enums.BlockType ActiveType { get; private set; } = Enums.BlockType.Grass;

    [SerializeField]
    GameObject _player;

    private BlockManipulator _blockManipulator;
    // Start is called before the first frame update
    void Start()
    {
        _blockManipulator = GetComponentInParent<BlockManipulator>();
        _inventoryUI = uiCanvas.GetComponent<InventoryUI>();
        InitInventory();
        SubscribeToEvents();
    }

    private void InitInventory()
    {
        PlayerInventory.Add(Enums.BlockType.Grass,0);
        PlayerInventory.Add(Enums.BlockType.Dirt,0);
        PlayerInventory.Add(Enums.BlockType.Rock,0);
        PlayerInventory.Add(Enums.BlockType.Snow,0);
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Alpha1))
        {
            ActiveType = Enums.BlockType.Grass;
            _inventoryUI.SetActiveType(ActiveType);
        }
        else if (Input.GetKeyDown(KeyCode.Alpha2))
        {
            ActiveType = Enums.BlockType.Dirt;
            _inventoryUI.SetActiveType(ActiveType);
        }
        else if (Input.GetKeyDown(KeyCode.Alpha3))
        {
            ActiveType = Enums.BlockType.Rock;
            _inventoryUI.SetActiveType(ActiveType);
        }
        else if (Input.GetKeyDown(KeyCode.Alpha4))
        {
            ActiveType = Enums.BlockType.Snow;
            _inventoryUI.SetActiveType(ActiveType);
        }
    }

    private void SubscribeToEvents()
    {
        _blockManipulator.OnBlockPickUp += OnBlockPickup;
        _blockManipulator.OnBlockPlaced += OnBlockPlaced;
    }

    private void OnBlockPlaced(object sender, OnBlockEventArgs e)
    {
        PlayerInventory[e.BlockType]--;

        if (PlayerInventory[e.BlockType] < 0)
        {
            PlayerInventory[e.BlockType] = 0;
        }
        _inventoryUI.UpdateBlockUI(e.BlockType, PlayerInventory[e.BlockType]);
    }

    private void OnBlockPickup(object sender, OnBlockEventArgs e)
    {
        if (PlayerInventory.ContainsKey(e.BlockType))
        {
            PlayerInventory[e.BlockType] ++;
        }
        else
        {
            PlayerInventory.Add(e.BlockType, 1);
        }
        _inventoryUI.UpdateBlockUI(e.BlockType, PlayerInventory[e.BlockType]);
    }
}
