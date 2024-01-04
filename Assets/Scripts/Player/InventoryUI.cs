using Assets.Scripts.Classes;
using Assets.Scripts.Player.Classes;
using System.Collections.Generic;
using UnityEngine;

public class InventoryUI : MonoBehaviour
{
    [SerializeField]
    private GameObject _inventoryObject;
    private Inventory _inventory;

    [SerializeField]
    private List<BlockUI> _blockUIList = new List<BlockUI>();

    private Dictionary<Enums.BlockType, BlockUI> _inventoryBlocksUI = new Dictionary<Enums.BlockType, BlockUI>();
    private Enums.BlockType _activeType;


    // Start is called before the first frame update
    private void Start()
    {
        _inventory = _inventoryObject.GetComponent<Inventory>();
        InitializeBlockUIDictionary();
        SetActiveType(Enums.BlockType.Grass);
        AssignValuesToUI();
        SubscribeToEvents();
    }

    private void SubscribeToEvents()
    {
        _inventory.OnInventoryActiveTypeChanged += OnInventoryActiveTypeChanged;
        _inventory.OnInventoryChanged += OnInventoryChanged;
    }

    private void OnInventoryChanged(object sender, Assets.Scripts.Events.OnInvetoryEventArgs e)
    {
        UpdateBlockUI(e.BlockType, e.Value);
    }

    private void OnInventoryActiveTypeChanged(object sender, Assets.Scripts.Events.OnInvetoryEventArgs e)
    {
        SetActiveType(e.BlockType);
    }

    private void InitializeBlockUIDictionary()
    {
        foreach (var blockUI in _blockUIList)
        {
            _inventoryBlocksUI[blockUI.BlockType] = blockUI;
        }
    }

    private void AssignValuesToUI()
    {
        foreach (var blockUI in _inventoryBlocksUI.Values)
        {
            blockUI.TextComponent.text = blockUI.Value.ToString();
        }
    }

    public void UpdateBlockUI(Enums.BlockType blockType, int value)
    {

        _inventoryBlocksUI[blockType].Value = value;
        _inventoryBlocksUI[blockType].TextComponent.text = value.ToString();

        AssignValuesToUI();
    }
    public void SetActiveType(Enums.BlockType type)
    {
        _activeType = type;

        foreach (var blockUI in _inventoryBlocksUI.Values)
        {
            blockUI.TextComponent.color = blockUI.BlockType == _activeType ? Color.red : Color.white;
        }
    }
}
