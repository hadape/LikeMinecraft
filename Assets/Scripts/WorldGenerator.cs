using Assets.Scripts.Classes;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class WorldGenerator : MonoBehaviour
{
    [SerializeField]
    private ChunkGenerationSetting _chungGenerationSetting = new ChunkGenerationSetting();


    private ChunkGenerator _chunkGenerator;
    private ChunkMeshGenerator _chunkMeshGenerator;

    List<ChunkData> _activeChunks = new List<ChunkData>();

    // Start is called before the first frame update
    void Start()
    {
        _chunkMeshGenerator = new ChunkMeshGenerator(_chungGenerationSetting);
        _chunkGenerator = new ChunkGenerator(_chungGenerationSetting);
        for (int x = -_chungGenerationSetting.viewDistance; x <=  _chungGenerationSetting.viewDistance; x++)
        {
            for (int z =  -_chungGenerationSetting.viewDistance;z <= _chungGenerationSetting.viewDistance; z++)
            {
                CreateChunk(x,z);
            }
        }
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void OnBlockPickup(Vector3 blockCoord)
    {
        Debug.Log($"block pickuped: {blockCoord.x},{blockCoord.y},{blockCoord.z}");
        var blockCoords = new Vector3Int(Mathf.RoundToInt(blockCoord.x), Mathf.RoundToInt(blockCoord.y), Mathf.RoundToInt(blockCoord.z));
        var blocksChunk = GetChunkCoordsFromPosition(blockCoords);
        ChangeBlockTypeToAirInChunk(blockCoords, blocksChunk);
    }
    private void ChangeBlockTypeToAirInChunk(Vector3Int blockCoord, Vector2Int chunkCoords)
    {
        var chunk = _activeChunks.Where(x=>x.Coordinates == chunkCoords).FirstOrDefault();
        if (chunk != null)
        {
            chunk.Data[blockCoord] = Enums.BlockType.Air;
        }
        UpdateChunk(chunk.Coordinates.x, chunk.Coordinates.y);
    }

    private void CreateChunk(int x,int z)
    {
       
        var chunkData = _chunkGenerator.GetChunkData(x,z);

        int groundLevel = LayerMask.NameToLayer("Ground");
        GameObject tempChunk = new GameObject($"Chunk {x},{z}", new System.Type[] 
        { typeof(MeshRenderer), typeof(MeshFilter), typeof(MeshCollider) });
        tempChunk.layer = groundLevel;

        var mesh = new ChunkMeshGenerator(_chungGenerationSetting).CreateMesh(chunkData, tempChunk);


        tempChunk.GetComponent<MeshFilter>().mesh = mesh;
        tempChunk.GetComponent<MeshCollider>().sharedMesh = mesh;
        _activeChunks.Add(chunkData);
    }

    private void UpdateChunk(int chunkX, int chunkZ)
    {
        var chunkCoord = new Vector2Int(chunkX, chunkZ);
        var chunk = _activeChunks.Where(x=>x.Coordinates == chunkCoord).FirstOrDefault();
        if (chunk != null)
        {
            
        }

    }

    public Vector2Int GetChunkCoordsFromPosition(Vector3 WorldPosition)
    {
        return new Vector2Int(
            Mathf.FloorToInt(WorldPosition.x / _chungGenerationSetting.chunkSize.x),
            Mathf.FloorToInt(WorldPosition.z / _chungGenerationSetting.chunkSize.z)
        );
    }

}
