using UnityEngine;

namespace Assets.Scripts.Classes
{
    public class CoordinatesUtil
    {
        private ChunkGenerationSetting _chungGenerationSetting;

        public CoordinatesUtil(ChunkGenerationSetting chungGenerationSetting)
        {
            _chungGenerationSetting = chungGenerationSetting;
        }

        public Vector2Int GetChunkCoordsFromPosition(Vector3 WorldPosition)
        {
            return new Vector2Int(
                Mathf.FloorToInt(WorldPosition.x / _chungGenerationSetting.chunkSize.x),
                Mathf.FloorToInt(WorldPosition.z / _chungGenerationSetting.chunkSize.z)
            );
        }
    }
}
