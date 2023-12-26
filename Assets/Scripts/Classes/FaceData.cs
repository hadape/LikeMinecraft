using System;
using System.Collections.Generic;
using UnityEngine;


namespace Assets.Scripts.Classes
{
    public class FaceData
    {
        public Vector3[] Vertices { get; set; }
        public int[] Inidces { get; set; }

        public FaceData(Vector3[] verts, int[] tris)
        {
            Vertices = verts;
            Inidces = tris;
        }
    }
}
