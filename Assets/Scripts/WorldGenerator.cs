using Assets.Scripts.Classes;
using Assets.Scripts.Events;
using System;
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

    [SerializeField]
    private GameObject _player;
    private BlockManipulator _blockManipulator;

    // Start is called before the first frame update
    void Start()
    {
        _chunkMeshGenerator = new ChunkMeshGenerator(_chungGenerationSetting);
        _chunkGenerator = new ChunkGenerator(_chungGenerationSetting);
        _blockManipulator = _player.GetComponent<BlockManipulator>();
        CreateChunk(0, 0);
        //for (int x = -_chungGenerationSetting.viewDistance; x <=  _chungGenerationSetting.viewDistance; x++)
        //{
        //    for (int z =  -_chungGenerationSetting.viewDistance;z <= _chungGenerationSetting.viewDistance; z++)
        //    {
        //        CreateChunk(x,z);
        //    }
        //}
        SubscribeToEvents();
    }

    private void SubscribeToEvents()
    {
        _blockManipulator.OnBlockPickUp += OnBlockPickup;
        _blockManipulator.OnBlockPlaced += OnBlockPlaced;
    }

    private void OnBlockPlaced(object sender, OnBlockEventArgs e)
    {
        var blockCoords = new Vector3Int(Mathf.RoundToInt(e.BlockPosition.x), Mathf.RoundToInt(e.BlockPosition.y), Mathf.RoundToInt(e.BlockPosition.z));
        var blocksChunk = GetChunkCoordsFromPosition(blockCoords);
        ChangeBlockType(blockCoords, blocksChunk, Enums.BlockType.Dirt);
    }

    private void OnBlockPickup(object sender, OnBlockEventArgs e)
    {

        Debug.Log($"block pickuped: {e.BlockPosition.x},{e.BlockPosition.y},{e.BlockPosition.z}");
        var blockCoords = new Vector3Int(Mathf.RoundToInt(e.BlockPosition.x), Mathf.RoundToInt(e.BlockPosition.y), Mathf.RoundToInt(e.BlockPosition.z));
        var blocksChunk = GetChunkCoordsFromPosition(blockCoords);
        ChangeBlockType(blockCoords, blocksChunk, Enums.BlockType.Air);
    }



    private void ChangeBlockType(Vector3Int blockCoord, Vector2Int chunkCoords,Enums.BlockType blockType)
    {
        var chunk = _activeChunks.Where(x=>x.Coordinates == chunkCoords).FirstOrDefault();
        if (chunk != null)
        {
            chunk.Data[blockCoord] = blockType;
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

        chunkData.GameObject = tempChunk;

        var mesh = new ChunkMeshGenerator(_chungGenerationSetting).CreateMesh(chunkData);


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
            var newMesh = _chunkMeshGenerator.CreateMesh(chunk);
            chunk.GameObject.GetComponent<MeshFilter>().mesh = newMesh;
            chunk.GameObject.GetComponent<MeshCollider>().sharedMesh = newMesh;

            chunk.GameObject.GetComponent<MeshFilter>().sharedMesh = newMesh;
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
