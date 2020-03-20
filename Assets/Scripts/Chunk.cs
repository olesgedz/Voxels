using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Chunk
{
    public Block[,,] chunkData;
    public Material cubeMaterial;
    public GameObject chunk;

    void BuildChunk()
    {
        chunkData = new Block[World.chunkSize, World.chunkSize, World.chunkSize];
        for (int z = 0; z < World.chunkSize; z++)
        {
            for (int y = 0; y < World.chunkSize; y++)
            {
                for (int x = 0; x < World.chunkSize; x++)
                {
                    Vector3 pos = new Vector3(x, y, z);
                    if (Random.Range(0, 100) < 50)
                    {
                        chunkData[x, y, z] = new Block(Block.BlockType.GRASS, pos, chunk.gameObject, cubeMaterial, this);
                    }
                    else
                    {
                        chunkData[x, y, z] = new Block(Block.BlockType.AIR, pos, chunk.gameObject, cubeMaterial, this);

                    }
                }
            }
        }
    }

    public void DrawChunk()
    {
        for (int z = 0; z < World.chunkSize; z++)
        {
            for (int y = 0; y < World.chunkSize; y++)
            {
                for (int x = 0; x < World.chunkSize; x++)
                {
                    chunkData[x, y, z].Draw();
                }
            }
        }
        CombineQuads();
    }

    public Chunk(Vector3 position, Material c)
    {
        chunk = new GameObject(World.BuildChunkName(position));
        chunk.transform.position = position;
        cubeMaterial = c;
        BuildChunk();
    }

    public void CombineQuads()
    {
        //Combine meshes
        MeshFilter[] meshFilters = chunk.GetComponentsInChildren<MeshFilter>();
        CombineInstance[] combine = new CombineInstance[meshFilters.Length];
        int i = 0;
        while (i < meshFilters.Length)
        {
            combine[i].mesh = meshFilters[i].sharedMesh;
            combine[i].transform = meshFilters[i].transform.localToWorldMatrix;
            i++;
        }
        // Create mesh on the parent object
        MeshFilter mf = (MeshFilter)this.chunk.gameObject.AddComponent(typeof(MeshFilter));
        mf.mesh = new Mesh();
        //add combined, meshes on children as parent's mesh
        mf.mesh.CombineMeshes(combine);
        //renderer for parent 
        MeshRenderer renderer = this.chunk.gameObject.AddComponent(typeof(MeshRenderer)) as MeshRenderer;
        renderer.material = cubeMaterial;

        foreach (Transform quad in this.chunk.transform)
        {
            Object.Destroy(quad.gameObject);
        }

    }
}
