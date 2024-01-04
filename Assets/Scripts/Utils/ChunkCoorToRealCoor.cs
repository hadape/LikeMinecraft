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
