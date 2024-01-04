using Assets.Scripts.Classes;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Assets.Scripts.Events
{
    public class OnInvetoryEventArgs : EventArgs
    {
        public Enums.BlockType BlockType { get; set; }
        public int Value { get; set; }
    }
}
