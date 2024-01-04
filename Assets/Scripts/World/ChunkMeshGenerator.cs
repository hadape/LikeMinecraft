using Assets.Scripts.Classes;
using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using UnityEngine;

public class ChunkMeshGenerator
{
    private List<Vector3> _vertices;
    private List<int> _triangles;
    private List<Color> _colors;

    private Vector3Int _checkingBlockPosition;
    private Vector3Int _currentBlockPosition;
    private int _tris = 0;

    private MeshRenderer _meshRenderer;

    private Mesh _mesh;

    private ChunkGenerationSetting _chunkGenerationSetting;

    private ConcurrentQueue<Action> _mainThreadActions;

    public ChunkMeshGenerator(ChunkGenerationSetting chunkGenerationSetting, ConcurrentQueue<Action> mainThreadActions)
    {
        _chunkGenerationSetting = chunkGenerationSetting;
        _mainThreadActions = mainThreadActions;
    }

    public Mesh CreateMesh(ChunkData chunkData)
    {
        _mesh = new Mesh();

        CalculateMeshData(chunkData);

        _meshRenderer = chunkData.GameObject.GetComponent<MeshRenderer>();
        _meshRenderer.material = _chunkGenerationSetting.material;
        var arrVertices = _vertices.ToArray();

        _mesh.SetVertices(arrVertices);
        _mesh.SetIndices(_triangles, MeshTopology.Triangles, 0);
        _mesh.SetColors(_colors.ToArray());

        _mesh.RecalculateBounds();
        _mesh.RecalculateTangents();
        _mesh.RecalculateNormals();

        return _mesh;
    }

    private void DecideMesh(CubeSide cubeSide, Dictionary<Vector3Int, Enums.BlockType> basicData)
    {
        if (basicData[_currentBlockPosition] != Enums.BlockType.Air && basicData.ContainsKey(_checkingBlockPosition))
        {
            if (basicData[_checkingBlockPosition] == Enums.BlockType.Air)
            {
                AddMesh(cubeSide);
                AddColor(basicData[_currentBlockPosition]);
            }
        }
        if (basicData[_currentBlockPosition] != Enums.BlockType.Air && basicData.ContainsKey(_checkingBlockPosition) == false)
        {
            AddMesh(cubeSide);
            AddColor(basicData[_currentBlockPosition]);
        }

    }
    private void AddMesh(CubeSide cubeSide)
    {
        _vertices.AddRange(Cube.IncrementSideVertices(cubeSide, _currentBlockPosition).Vertices);
        _triangles.AddRange(Cube.IncrementSideTriangles(cubeSide, _tris).Triangles);
        _tris += 4;
    }
    private void AddColor(Enums.BlockType type)
    {
        switch (type)
        {
            case Enums.BlockType.Grass: _colors.AddRange(MeshColor.Green); break;
            case Enums.BlockType.Dirt: _colors.AddRange(MeshColor.Brown); break;
            case Enums.BlockType.Rock: _colors.AddRange(MeshColor.Gray); break;
            case Enums.BlockType.Bedrock: _colors.AddRange(MeshColor.Black); break;
            case Enums.BlockType.Snow: _colors.AddRange(MeshColor.White); break;
        }
    }

    public void CalculateMeshData(ChunkData chunkData)
    {
        CalulateMesh(chunkData);
        _mainThreadActions.Enqueue(() =>
        {
            if (chunkData.GameObject != null)
            {
                var newMesh = new Mesh();
                var meshRenderer = chunkData.GameObject.GetComponent<MeshRenderer>();
                meshRenderer.material = _chunkGenerationSetting.material;
                newMesh.SetVertices(_vertices);
                newMesh.SetTriangles(_triangles, 0);
                newMesh.SetColors(_colors);

                newMesh.RecalculateBounds();
                newMesh.RecalculateNormals();

                chunkData.GameObject.GetComponent<MeshFilter>().mesh = newMesh;
                chunkData.GameObject.GetComponent<MeshCollider>().sharedMesh = newMesh;
            }
        });
    }
    private void CalulateMesh(ChunkData chunkData)
    {
        _vertices = new List<Vector3>();
        _triangles = new List<int>();
        _colors = new List<Color>();
        var startingX = ChunkCoorToRealCoor.GetRealCoor(chunkData.Coordinates.x, _chunkGenerationSetting.chunkSize.x);
        var startingZ = ChunkCoorToRealCoor.GetRealCoor(chunkData.Coordinates.y, _chunkGenerationSetting.chunkSize.z);

        _tris = 0;
        for (int x = startingX; x < _chunkGenerationSetting.chunkSize.x + startingX; x++)
        {
            for (int z = startingZ; z < _chunkGenerationSetting.chunkSize.z + startingZ; z++)
            {
                for (int y = 0; y < _chunkGenerationSetting.chunkSize.y; y++)
                {
                    _currentBlockPosition = new Vector3Int(x, y, z);

                    //checking for surrounding blocks, if any air we will have to create mesh for that side
                    foreach (Enums.Side side in (Enums.Side[])Enum.GetValues(typeof(Enums.Side)))
                    {
                        _checkingBlockPosition = Vector3Int.zero;
                        switch (side)
                        {
                            case Enums.Side.Left:
                                _checkingBlockPosition = _currentBlockPosition + Vector3Int.left;
                                DecideMesh(Cube.LeftSide, chunkData.Data);
                                break;
                            case Enums.Side.Right:
                                _checkingBlockPosition = _currentBlockPosition + Vector3Int.right;
                                DecideMesh(Cube.RightSide, chunkData.Data);
                                break;
                            case Enums.Side.Up:
                                _checkingBlockPosition = _currentBlockPosition + Vector3Int.up;
                                DecideMesh(Cube.UpSide, chunkData.Data);
                                break;
                            case Enums.Side.Down:
                                _checkingBlockPosition = _currentBlockPosition + Vector3Int.down;
                                DecideMesh(Cube.DownSide, chunkData.Data);
                                break;
                            case Enums.Side.Front:
                                _checkingBlockPosition = _currentBlockPosition + Vector3Int.forward;
                                DecideMesh(Cube.FrontSide, chunkData.Data);
                                break;
                            case Enums.Side.Back:
                                _checkingBlockPosition = _currentBlockPosition + Vector3Int.back;
                                DecideMesh(Cube.BackSide, chunkData.Data);
                                break;

                        }
                    }
                }
            }
        }
    }
}
