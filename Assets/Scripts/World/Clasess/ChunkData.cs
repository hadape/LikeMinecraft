using System.Collections.Generic;
using UnityEngine;

namespace Assets.Scripts.Classes
{
    public class ChunkData
    {
        public Dictionary<Vector3Int, Enums.BlockType> Data { get; set; }
        public Vector2Int Coordinates { get; set; }
        public GameObject GameObject { get; set; }

    }
}
