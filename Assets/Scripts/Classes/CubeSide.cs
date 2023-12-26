using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
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
