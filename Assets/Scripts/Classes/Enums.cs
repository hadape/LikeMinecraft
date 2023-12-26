using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Assets.Scripts.Classes
{
    public class Enums
    {
        public enum Side
        {
            Up, Down,
            Left, Right,
            Front, Back
        }

        public enum BlockType
        {
            Air,
            Grass,
            Dirt,
            Rock,
            Bedrock
        }
    }
}
