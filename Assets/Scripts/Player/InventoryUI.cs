using Assets.Scripts.Classes;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class InventoryUI : MonoBehaviour
{
    [SerializeField]
    TextMeshProUGUI _grassText;
    int _grassValue=0;
    [SerializeField]
    TextMeshProUGUI _dirtText;
    int _dirtValue = 0;
    [SerializeField]
    TextMeshProUGUI _rockText;
    int _rockValue = 0;
    [SerializeField]
    TextMeshProUGUI _snowText;
    int _snowValue = 0;
    Enums.BlockType _activeType;
    // Start is called before the first frame update
    void Start()
    {
        SetActiveType(Enums.BlockType.Grass);
        AssignValuesToUI();


    }

    private void AssignValuesToUI()
    {
        _grassText.text = _grassValue.ToString();
        _dirtText.text = _dirtValue.ToString();
        _rockText.text = _rockValue.ToString();
        _snowText.text = _snowValue.ToString();
    }

    public void UpdateBlockUI(Enums.BlockType blockType, int value)
    {
        switch (blockType)
        {
            case Enums.BlockType.Grass: _grassValue = value; break;
            case Enums.BlockType.Dirt: _dirtValue = value; break;
            case Enums.BlockType.Rock: _rockValue = value; break;
            case Enums.BlockType.Snow: _snowValue = value; break;
             
        }
        AssignValuesToUI();
    }
    public void SetActiveType(Enums.BlockType type)
    {
        _activeType = type;

        if (_activeType == Enums.BlockType.Grass) _grassText.color = Color.red; else  _grassText.color = Color.white;
        if (_activeType == Enums.BlockType.Dirt) _dirtText.color = Color.red; else _dirtText.color = Color.white;
        if (_activeType == Enums.BlockType.Rock) _rockText.color = Color.red; else _rockText.color = Color.white;
        if (_activeType == Enums.BlockType.Snow) _snowText.color = Color.red; else _rockText.color = Color.white;
    }
}
