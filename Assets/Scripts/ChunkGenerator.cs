using Assets.Scripts.Classes;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ChunkGenerator 
{

    ChunkGenerationSetting _setting;
    public ChunkGenerator(ChunkGenerationSetting setting)
    {
        _setting = setting;
    }


    public Dictionary<Vector3Int, Enums.BlockType> GenerateBasicDataForChunk()
    {
        Dictionary<Vector3Int, Enums.BlockType> chunkBasicData = new Dictionary<Vector3Int, Enums.BlockType>();
        int height;
        Enums.BlockType blockType;

        for(int x = 0;  x < _setting.chunkSize.x; x++)
        {
            for(int z=0;  z < _setting.chunkSize.z; z++) 
            {
                height = GetHeighFromPerlinNoise(x, z);
                
                for (int y = 0; y <_setting.chunkSize.y ; y++)
                {
                    blockType = Enums.BlockType.Air;
                    if (y <= height)
                    {
                        if (y == height) { blockType = Enums.BlockType.Grass; }
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
        float perlinCoordsX = _setting.noiseOffset.x + x / (float)_setting.chunkSize.x * _setting.noiseScale.x;
        float perlinCoordY = _setting.noiseOffset.y + z / (float)_setting.chunkSize.z * _setting.noiseScale.y;

        return Mathf.RoundToInt(Mathf.PerlinNoise(perlinCoordsX, perlinCoordY) * _setting.heightIntensity + _setting.heightOffset);
    }
}
