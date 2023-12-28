using Assets.Scripts.Classes;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ChunkMeshGenerator 
{
    List<Vector3> vertices = new List<Vector3>();
    List<int> triangles = new List<int>();
    List<Color> colors = new List<Color>();

    Vector3Int checkingBlockPosition;
    Vector3Int currentBlockPosition;
    int tris = 0;

    MeshRenderer meshRenderer;

    Mesh mesh;

    ChunkGenerationSetting _chunkGenerationSetting;

    public ChunkMeshGenerator(ChunkGenerationSetting chunkGenerationSetting)
    {
        _chunkGenerationSetting = chunkGenerationSetting;
    }

    public Mesh CreateMesh(ChunkData chunkData, GameObject chunk)
    {
        mesh = new Mesh();
        var startingX = ChunkCoorToRealCoor.GetRealCoor(chunkData.Coordinates.x, _chunkGenerationSetting.chunkSize.x);
        var startingZ = ChunkCoorToRealCoor.GetRealCoor(chunkData.Coordinates.y, _chunkGenerationSetting.chunkSize.z);
        meshRenderer = chunk.GetComponent<MeshRenderer>();
        meshRenderer.material = _chunkGenerationSetting.material;
        tris = 0; 
        for (int x = startingX; x< _chunkGenerationSetting.chunkSize.x+ startingX; x++)
        {
            for (int z = startingZ; z < _chunkGenerationSetting.chunkSize.z+ startingZ; z++)
            {
                for (int y = 0; y < _chunkGenerationSetting.chunkSize.y; y++)
                {
                    currentBlockPosition = new Vector3Int(x, y, z);

                    //checking for surrounding blocks, if any air we will have to create mesh for that side
                    foreach (Enums.Side side in (Enums.Side[])Enum.GetValues(typeof(Enums.Side)))
                    {
                        checkingBlockPosition = Vector3Int.zero;
                        switch (side)
                        {
                            case Enums.Side.Left:
                                checkingBlockPosition = currentBlockPosition + Vector3Int.left;
                                DecideMesh(Cube.LeftSide, chunkData.Data);
                                break;
                            case Enums.Side.Right:
                                checkingBlockPosition = currentBlockPosition + Vector3Int.right;
                                DecideMesh(Cube.RightSide, chunkData.Data);
                                break;
                            case Enums.Side.Up:
                                checkingBlockPosition = currentBlockPosition + Vector3Int.up;
                                DecideMesh(Cube.UpSide, chunkData.Data);
                                break;
                            case Enums.Side.Down:
                                checkingBlockPosition = currentBlockPosition + Vector3Int.down;
                                DecideMesh(Cube.DownSide, chunkData.Data);
                                break;
                            case Enums.Side.Front:
                                checkingBlockPosition = currentBlockPosition + Vector3Int.forward;
                                DecideMesh( Cube.FrontSide, chunkData.Data);
                                break;
                            case Enums.Side.Back:
                                checkingBlockPosition = currentBlockPosition + Vector3Int.back;
                                DecideMesh(Cube.BackSide, chunkData.Data);
                                break;

                              
                        }                        
                    }
                }
            }
        }
        var arrVertices = vertices.ToArray();
        mesh.SetVertices(arrVertices);
        mesh.SetTriangles(triangles,0);

        mesh.colors = colors.ToArray();
        mesh.RecalculateBounds();
        mesh.RecalculateTangents();
        mesh.RecalculateNormals();

        return mesh;
    }

    private void DecideMesh(CubeSide cubeSide, Dictionary<Vector3Int, Enums.BlockType> basicData)
    {
        if (basicData[currentBlockPosition] != Enums.BlockType.Air && basicData.ContainsKey(checkingBlockPosition))
        {
            if (basicData[checkingBlockPosition] == Enums.BlockType.Air)
            {
                AddMesh(cubeSide);
                AddColor(basicData[currentBlockPosition]);
            }
        }
        if (basicData[currentBlockPosition] != Enums.BlockType.Air && basicData.ContainsKey(checkingBlockPosition) == false)
        {
            AddMesh(cubeSide);
            AddColor(basicData[currentBlockPosition]);
        }
        
    }
    private void AddMesh( CubeSide cubeSide)
    {
        vertices.AddRange(Cube.IncrementSideVertices(cubeSide, currentBlockPosition).Vertices);
        triangles.AddRange(Cube.IncrementSideTriangles(cubeSide, tris).Triangles);
        tris += 4;
    }
    private void AddColor(Enums.BlockType type)
    {
        switch (type)
        {
            case Enums.BlockType.Grass: colors.AddRange(MeshColor.Green); break;
            case Enums.BlockType.Dirt: colors.AddRange(MeshColor.Brown); break;
            case Enums.BlockType.Rock: colors.AddRange(MeshColor.Gray); break;
            case Enums.BlockType.Bedrock: colors.AddRange(MeshColor.Black); break;
        }
    }
}
