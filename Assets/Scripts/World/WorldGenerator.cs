using Assets.Scripts;
using Assets.Scripts.Classes;
using Assets.Scripts.Events;
using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using UnityEngine;

public class WorldGenerator : MonoBehaviour
{
    [SerializeField]
    private ChunkGenerationSetting _chungGenerationSetting = new ChunkGenerationSetting();

    private ChunkGenerator _chunkGenerator;
    private CoordinatesUtil _coordinatesUtil;

    [SerializeField]
    private GameObject _player;
    private BlockManipulator _blockManipulator;
    private World _world;
    private Vector2Int? _currentPlayersChunk = null;

    private ConcurrentQueue<Action> _mainThreadActions = new ConcurrentQueue<Action>();


    void Start()
    {
        _world = GetComponent<World>();
        _world.SetGhunkSetting(_chungGenerationSetting);
        _chunkGenerator = new ChunkGenerator(_chungGenerationSetting);
        _blockManipulator = _player.GetComponent<BlockManipulator>();
        _coordinatesUtil = new CoordinatesUtil(_chungGenerationSetting);

        SubscribeToEvents();
    }

    void Update()
    {
        while (_mainThreadActions.TryDequeue(out var action))
        {
            action.Invoke();
        }
        GenerateChunkBasedOnPlayer();
    }

    private void GenerateChunkBasedOnPlayer()
    {
        var playersChunk = _coordinatesUtil.GetChunkCoordsFromPosition(_player.transform.position);
        if (_currentPlayersChunk == null || playersChunk != _currentPlayersChunk)
        {
            _currentPlayersChunk = playersChunk;

            GenerateChunks();
        }
    }

    private void GenerateChunks()
    {
        var chunksToRemove = new List<ChunkData>();
        foreach (var chunk in _world.ActiveChunks)
        {
            chunksToRemove.Add(chunk);
        }
        for (int x = _currentPlayersChunk.Value.x - _chungGenerationSetting.viewDistance; x <= _currentPlayersChunk.Value.x + _chungGenerationSetting.viewDistance; x++)
        {
            for (int z = _currentPlayersChunk.Value.y - _chungGenerationSetting.viewDistance; z <= _currentPlayersChunk.Value.y + _chungGenerationSetting.viewDistance; z++)
            {
                var chunk = _world.ActiveChunks.Where(ch => ch.Coordinates == new Vector2Int(x, z)).FirstOrDefault();
                if (chunk == null)
                {
                    CreateChunkAsync(x, z);
                }
                else
                {
                    chunksToRemove.Remove(chunk);
                }
            }
        }

        for (int i = chunksToRemove.Count - 1; i >= 0; i--)
        {
            var chunkToDestroy = chunksToRemove[i];
            _world.RemoveChunkFromActive(chunkToDestroy);
            Destroy(chunkToDestroy.GameObject);
            //TODO: save data of destroy chunks, so players block manipulation will be preserved
        }
    }

    private void SubscribeToEvents()
    {
        //TODO: create event manager and try more decoupling objects
        _blockManipulator.OnBlockPickUp += OnBlockPickup;
        _blockManipulator.OnBlockPlaced += OnBlockPlaced;
    }

    private void OnBlockPlaced(object sender, OnBlockEventArgs e)
    {
        var blockCoords = new Vector3Int(Mathf.RoundToInt(e.BlockPosition.x), Mathf.RoundToInt(e.BlockPosition.y), Mathf.RoundToInt(e.BlockPosition.z));
        var blocksChunk = _coordinatesUtil.GetChunkCoordsFromPosition(blockCoords);
        ChangeBlockType(blockCoords, blocksChunk, e.BlockType);
    }

    private void OnBlockPickup(object sender, OnBlockEventArgs e)
    {
        var blockCoords = new Vector3Int(Mathf.RoundToInt(e.BlockPosition.x), Mathf.RoundToInt(e.BlockPosition.y), Mathf.RoundToInt(e.BlockPosition.z));
        var blocksChunk = _coordinatesUtil.GetChunkCoordsFromPosition(blockCoords);
        ChangeBlockType(blockCoords, blocksChunk, Enums.BlockType.Air);
    }

    private void ChangeBlockType(Vector3Int blockCoord, Vector2Int chunkCoords, Enums.BlockType blockType)
    {
        var chunk = _world.ActiveChunks.Where(x => x.Coordinates == chunkCoords).FirstOrDefault();

        if (chunk != null && CheckWorldBound(blockCoord.y, chunk.Data[blockCoord]))
        {
            chunk.Data[blockCoord] = blockType;

            //TODO: have separate queu for chunks where block are manipulated, change them before generation new chunks
            UpdateChunkAsync(chunk.Coordinates.x, chunk.Coordinates.y);
        }
    }
    private bool CheckWorldBound(int blockY, Enums.BlockType blockType)
    {
        var result = true;
        if (blockY > _chungGenerationSetting.chunkSize.y) result = false;
        if (blockType == Enums.BlockType.Bedrock) result = false;

        return result;
    }

    private async void CreateChunkAsync(int x, int z)
    {
        
        var chunkData = _chunkGenerator.GetChunkData(x, z);

        int groundLevel = LayerMask.NameToLayer(Constants.GROUND_LAYER);
        GameObject tempChunk = new GameObject($"{Constants.CHUNK} {x},{z}", new System.Type[]
        { typeof(MeshRenderer), typeof(MeshFilter), typeof(MeshCollider) });
        tempChunk.layer = groundLevel;

        chunkData.GameObject = tempChunk;

        //TODO: optimalize generation of mesh, using task queue, or atleast throtle number of tasks
        await Task.Run(() => new ChunkMeshGenerator(_chungGenerationSetting, _mainThreadActions).CalculateMeshData(chunkData));

        _world.AddChunkToActive(chunkData);
    }

    private async void UpdateChunkAsync(int chunkX, int chunkZ)
    {
        var chunkCoord = new Vector2Int(chunkX, chunkZ);
        var chunk = _world.ActiveChunks.Where(x => x.Coordinates == chunkCoord).FirstOrDefault();

        if (chunk != null)
        {
            await Task.Run(() => new ChunkMeshGenerator(_chungGenerationSetting, _mainThreadActions).CalculateMeshData(chunk));
        }
    }

}
