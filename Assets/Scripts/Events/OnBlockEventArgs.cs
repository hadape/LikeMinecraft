﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;

namespace Assets.Scripts.Events
{
    public class OnBlockEventArgs : EventArgs
    {
        public Vector3Int BlockPosition;
    }
    
}