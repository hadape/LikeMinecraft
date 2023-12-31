using Assets.Scripts.Classes;
using Assets.Scripts.Events;
using System;
using System.Collections.Concurrent;
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

    private ConcurrentQueue<Action> _mainThreadActions = new ConcurrentQueue<Action>();

    // Start is called before the first frame update
    void Start()
    {
        _world = GetComponent<World>();
        _world.SetGhunkSetting(_chungGenerationSetting);
        _chunkGenerator = new ChunkGenerator(_chungGenerationSetting);
        _blockManipulator = _player.GetComponent<BlockManipulator>();
        _coordinatesUtil = new CoordinatesUtil(_chungGenerationSetting);

        for (int x = -_chungGenerationSetting.viewDistance; x <= _chungGenerationSetting.viewDistance; x++)
        {
            for (int z = -_chungGenerationSetting.viewDistance; z <= _chungGenerationSetting.viewDistance; z++)
            {
                CreateChunkAsync(x, z);
            }
        }
        SubscribeToEvents();
    }

    void Update()
    {
        while (_mainThreadActions.TryDequeue(out var action))
        {
            action.Invoke();
        }
    }

    private void SubscribeToEvents()
    {
        _blockManipulator.OnBlockPickUp += OnBlockPickup;
        _blockManipulator.OnBlockPlaced += OnBlockPlaced;
    }

    private void OnBlockPlaced(object sender, OnBlockEventArgs e)
    {
        var blockCoords = new Vector3Int(Mathf.RoundToInt(e.BlockPosition.x), Mathf.RoundToInt(e.BlockPosition.y), Mathf.RoundToInt(e.BlockPosition.z));
        var blocksChunk = _coordinatesUtil.GetChunkCoordsFromPosition(blockCoords);
        ChangeBlockType(blockCoords, blocksChunk, Enums.BlockType.Dirt);
    }

    private void OnBlockPickup(object sender, OnBlockEventArgs e)
    {

        Debug.Log($"block pickuped: {e.BlockPosition.x},{e.BlockPosition.y},{e.BlockPosition.z}");
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

        int groundLevel = LayerMask.NameToLayer("Ground");
        GameObject tempChunk = new GameObject($"Chunk {x},{z}", new System.Type[]
        { typeof(MeshRenderer), typeof(MeshFilter), typeof(MeshCollider) });
        tempChunk.layer = groundLevel;

        chunkData.GameObject = tempChunk;

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
