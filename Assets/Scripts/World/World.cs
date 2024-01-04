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
    [SerializeField]
    private GameObject _player;
    [SerializeField]
    Camera _initCamera;
    [SerializeField]
    Camera _mainCamera;
    [SerializeField]
    GameObject _loadingScreen;
    [SerializeField]
    GameObject _gameUI;

    private void Start()
    {
        _player.SetActive(false);
        _loadingScreen.SetActive(true);
        _gameUI.SetActive(false);
    }
    private void Update()
    {
        SpawnPlayer();
    }

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

    private void SpawnPlayer()
    {
        int oneRowGenerated = _generationSetting.viewDistance + (_generationSetting.viewDistance + 1);
        if (_player.activeSelf == false && ActiveChunks.Count() == oneRowGenerated * oneRowGenerated)
        {
            _player.SetActive(true);
            _initCamera.gameObject.SetActive(false);
            _loadingScreen.SetActive(false);
            _gameUI.SetActive(true);
        }
    }
}
