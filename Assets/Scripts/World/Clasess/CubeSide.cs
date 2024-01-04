using UnityEngine;

namespace Assets.Scripts.Classes
{
    public class CubeSide
    {
        public Enums.Side Side { get; set; }
        public Vector3[] Vertices { get; set; }
        public int[] Triangles { get; set; }
    }
}
