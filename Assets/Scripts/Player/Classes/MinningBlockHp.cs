using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Assets.Scripts.Classes
{
    public static class MinningBlockHp
    {
        public static float GetHpByBlockType(Enums.BlockType type)
        {
            switch (type) {
                case Enums.BlockType.Snow: return .5f;
                case Enums.BlockType.Dirt: case Enums.BlockType.Grass: return 1;
                case Enums.BlockType.Rock: return 2;
                default: return int.MaxValue;
            }

        }
    }
}
