using System.Collections.Generic;
using UnityEngine;

namespace Assets.Scripts.Classes
{
    public class MeshData
    {
        public List<Vector3> Vertices { get; set; } = new List<Vector3>();
        public List<int> Triangles { get; set; } = new List<int>();
        public List<Color> Colors { get; set; } = new List<Color>();
    }
}
