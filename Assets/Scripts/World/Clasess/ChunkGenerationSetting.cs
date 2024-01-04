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
        public Vector2 noiseScale = new Vector2(.5f,.5f);
        public Vector2 noiseOffset = Vector2.zero;
        public int heightOffset = 60;
        public float heightIntensity = 10;
        public Material material;
        public int viewDistance = 1;
        public int snowHeight = 70;
    }
}
