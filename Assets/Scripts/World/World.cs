using Assets.Scripts.Classes;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class World : MonoBehaviour
{
    public List<ChunkData> ActiveChunks { get; private set; } = new List<ChunkData>();
    private CoordinatesUtil _coordinatesUtil;
    private ChunkGenerationSetting _generationSetting;

    public Enums.BlockType GetTypeOfBlock(Vector3Int blockCoord)
    {
        var chunkCoord = _coordinatesUtil.GetChunkCoordsFromPosition(blockCoord);
        var chunk = ActiveChunks.Where(x=>x.Coordinates == chunkCoord).FirstOrDefault();
        if (chunk != null)
        {
            return chunk.Data[blockCoord];
        }
        else
        {
            throw new System.Exception("Chunk is null");
        }
    }

    public void AddChunkToActive(ChunkData chunk)
    {
        ActiveChunks.Add(chunk);
    }
    public void RemoveChunkFromActive(ChunkData chunk) {  ActiveChunks.Remove(chunk); }


    public Vector2Int GetChunkCoordByBlock(Vector3Int blockCoord)
    {
        return _coordinatesUtil.GetChunkCoordsFromPosition(blockCoord);
    }

    public void SetGhunkSetting(ChunkGenerationSetting generationSetting)
    {
        _generationSetting = generationSetting;
        if (_coordinatesUtil == null)
        {
            _coordinatesUtil = new CoordinatesUtil(_generationSetting);
        }
    }

}
