using Assets.Scripts.Classes;
using System.Collections.Generic;
using UnityEngine;

public class ChunkGenerator
{
    int _randomSeed;
    ChunkGenerationSetting _setting;
    public ChunkGenerator(ChunkGenerationSetting setting)
    {
        _setting = setting;
        _randomSeed = Random.Range(0, 10000);
    }

    public ChunkData GetChunkData(int chunkX, int chunkZ)
    {
        return new ChunkData
        {
            Data = GenerateBasicDataForChunk(chunkX, chunkZ),
            Coordinates = new Vector2Int(chunkX, chunkZ),
        };
    }

    public Dictionary<Vector3Int, Enums.BlockType> GenerateBasicDataForChunk(int chunkX, int chunkZ)
    {
        var startingX = ChunkCoorToRealCoor.GetRealCoor(chunkX, _setting.chunkSize.x);
        var startingZ = ChunkCoorToRealCoor.GetRealCoor(chunkZ, _setting.chunkSize.z);
        Dictionary<Vector3Int, Enums.BlockType> chunkBasicData = new Dictionary<Vector3Int, Enums.BlockType>();
        int height;
        Enums.BlockType blockType;

        for (int x = startingX; x < _setting.chunkSize.x + startingX; x++)
        {
            for (int z = startingZ; z < _setting.chunkSize.z + startingZ; z++)
            {
                height = GetHeighFromPerlinNoise(x, z);

                for (int y = 0; y < _setting.chunkSize.y; y++)
                {
                    blockType = Enums.BlockType.Air;
                    if (y <= height)
                    {
                        if (y == height)
                        {
                            if (y >= _setting.snowHeight)
                            {
                                blockType = Enums.BlockType.Snow;
                            }
                            else { blockType = Enums.BlockType.Grass; }
                        }
                        if (y < height && y > height - 4) { blockType = Enums.BlockType.Dirt; }
                        if (y <= height - 4 && y > 0) { blockType = Enums.BlockType.Rock; }
                        if (y == 0) { blockType = Enums.BlockType.Bedrock; }
                    }
                    chunkBasicData.Add(new Vector3Int(x, y, z), blockType);
                }
            }
        }
        return chunkBasicData;
    }

    private int GetHeighFromPerlinNoise(int x, int z)
    {
        float perlinCoordsX = _setting.noiseOffset.x + _randomSeed + x / (float)_setting.chunkSize.x * _setting.noiseScale.x;
        float perlinCoordY = _setting.noiseOffset.y + +_randomSeed + z / (float)_setting.chunkSize.z * _setting.noiseScale.y;

        return Mathf.RoundToInt(Mathf.PerlinNoise(perlinCoordsX, perlinCoordY) * _setting.heightIntensity + _setting.heightOffset);
    }
}
