using Assets.Scripts.Classes;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WorldGenerator : MonoBehaviour
{
    [SerializeField]
    private ChunkGenerationSetting _chungGenerationSetting = new ChunkGenerationSetting();

    private Dictionary<Vector3Int,Enums.BlockType>chunkData;

    private ChunkGenerator _chunkGenerator;


    // Start is called before the first frame update
    void Start()
    {
        _chunkGenerator = new ChunkGenerator(_chungGenerationSetting);
        chunkData = _chunkGenerator.GenerateBasicDataForChunk();

        GameObject tempChunk = new GameObject("Chunk", new System.Type[] { typeof(MeshRenderer), typeof(MeshFilter) });
        tempChunk.GetComponent<MeshFilter>().mesh = new ChunkMeshGenerator(_chungGenerationSetting).CreateMesh(chunkData, tempChunk);
    }

    // Update is called once per frame
    void Update()
    {
        
    }


    //private void OnDrawGizmos()
    //{
    //    if (chunkData != null)
    //    {
    //        for (int x = 0; x < _chungGenerationSetting.chunkSize.x; x++)
    //        {
    //            for (int y = 0; y < _chungGenerationSetting.chunkSize.y; y++)
    //            {
    //                for (int z = 0; z < _chungGenerationSetting.chunkSize.z; z++)
    //                {
    //                    switch (chunkData[new Vector3Int(x,y,z)])
    //                    {
    //                        case Enums.BlockType.Air: continue;
    //                        case Enums.BlockType.Grass: Gizmos.color = Color.green; break;
    //                        case Enums.BlockType.Dirt: Gizmos.color = Color.yellow; break;
    //                        case Enums.BlockType.Rock: Gizmos.color = Color.gray; break;
    //                        case Enums.BlockType.Bedrock: Gizmos.color = Color.black; break;
    //                    }
    //                    Gizmos.DrawWireCube(new Vector3(x, y, z), Vector3.one);
    //                }
    //            }
    //        }
    //    }
    //}


}
