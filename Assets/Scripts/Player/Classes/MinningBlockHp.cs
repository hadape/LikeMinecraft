namespace Assets.Scripts.Classes
{
    public static class MinningBlockHp
    {
        public static float GetHpByBlockType(Enums.BlockType type)
        {
            switch (type)
            {
                case Enums.BlockType.Snow: return .1f;
                case Enums.BlockType.Dirt: case Enums.BlockType.Grass: return .5f;
                case Enums.BlockType.Rock: return 2;
                default: return int.MaxValue;
            }
        }
    }
}
