using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Assets.Scripts.Classes
{
    public static class ChunkCoorToRealCoor
    {
        public static int GetRealCoor(int chunkAxeValue, int chunkSizeAxeValue)
        {
            return chunkAxeValue * chunkSizeAxeValue;
        }
    }
}
