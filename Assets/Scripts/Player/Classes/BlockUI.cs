using Assets.Scripts.Classes;
using System;
using TMPro;
using UnityEngine;

namespace Assets.Scripts.Player.Classes
{
    [Serializable]
    public class BlockUI
    {
        [SerializeField]
        public Enums.BlockType BlockType;
        [SerializeField]
        public TextMeshProUGUI TextComponent;
        [SerializeField]
        public int Value;
    }
}
