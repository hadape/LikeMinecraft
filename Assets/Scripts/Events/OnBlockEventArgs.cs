using Assets.Scripts.Classes;
using System;
using UnityEngine;

namespace Assets.Scripts.Events
{
    public class OnBlockEventArgs : EventArgs
    {
        public Vector3Int BlockPosition;
        public Enums.BlockType BlockType;
    }
}
