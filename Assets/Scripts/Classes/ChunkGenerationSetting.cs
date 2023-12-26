using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;

namespace Assets.Scripts.Classes
{
    [Serializable]
    public class ChunkGenerationSetting
    {
        
        public Vector3Int chunkSize = new Vector3Int(16, 256, 16);
        public Vector2 noiseScale = Vector2.one;
        public Vector2 noiseOffset = Vector2.zero;
        public int heightOffset = 60;
        public float heightIntensity = 5;
        public Material material;
    }
}
