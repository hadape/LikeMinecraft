using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;

namespace Assets.Scripts.Player.Classes
{
    [Serializable]
    public class MovementSettings
    {

        public float GroundDistance = 0.4f;
        public LayerMask GroundMask;
        public float Speed = 5f;
        public float Gravity = -9.81f;
        public float JumpHeight = 2f;
    }
}
