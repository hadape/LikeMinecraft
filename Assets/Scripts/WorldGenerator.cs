using Assets.Scripts.Classes;
using System.Collections;
using System.Collections.Generic;
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
        CreateChunk(0,0);
        CreateChunk(0,1);
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    private void CreateChunk(int x,int z)
    {
       
        var chunkData = _chunkGenerator.GetChunkData(x,z);

        int groundLevel = LayerMask.NameToLayer("Ground");
        GameObject tempChunk = new GameObject("Chunk", new System.Type[] { typeof(MeshRenderer), typeof(MeshFilter), typeof(MeshCollider) });
        tempChunk.layer = groundLevel;

        var mesh = new ChunkMeshGenerator(_chungGenerationSetting).CreateMesh(chunkData, tempChunk);


        tempChunk.GetComponent<MeshFilter>().mesh = mesh;
        tempChunk.GetComponent<MeshCollider>().sharedMesh = mesh;
    }



}
