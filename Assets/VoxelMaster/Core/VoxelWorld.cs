﻿using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;


public class VoxelWorld : MonoBehaviour
{
    private List<VoxelMaterial> _materials = new List<VoxelMaterial>();
    public List<VoxelMaterial> materials { get => new List<VoxelMaterial>(_materials); private set { _materials = value; } }

    public float voxelScale;
    public float isoLevel;
    public Vector3Int chunkSize;
    public Material material;
    private Dictionary<Vector3Int, VoxelChunk> chunks = new Dictionary<Vector3Int, VoxelChunk>();
    private Dictionary<VoxelChunk, Mesh> chunkMeshes = new Dictionary<VoxelChunk, Mesh>();
    // [HideInInspector] public String dataStructureType;
    // [HideInInspector] public String meshGeneratorType;
    // [HideInInspector] public String heightmapGeneratorType;
    [SerializeReference] public VoxelDataStructure dataStructure;
    [SerializeReference] public VoxelMeshGenerator meshGenerator;
    [SerializeReference] public HeightmapGenerator heightmapGenerator;
    public Mesh mesh;


    public Texture2D sdfsddsfsdf;


    private float SignedDistanceSphere(Vector3 pos)
    {
        Vector3 center = new Vector3(chunkSize.x / 2, chunkSize.y / 2, chunkSize.z / 2);
        float radius = chunkSize.x / 2.33f;
        return Vector3.Distance(pos, center) - radius;
    }
    FastNoise noise = new FastNoise();
    private float DensityFunction(Vector3 pos)
    {
        pos *= 1.0001f;
        pos *= 4f;
        return noise.GetPerlin(pos.x, pos.y, pos.z);
        // return SignedDistanceSphere(pos);
    }
    void Start()
    {
        dataStructure.Init(chunkSize);

        for (int x = 0; x < chunkSize.x - 1; x++)
            for (int y = 0; y < chunkSize.y - 1; y++)
                for (int z = 0; z < chunkSize.z - 1; z++)
                {
                    dataStructure.SetVoxel(new Vector3Int(x, y, z), new Voxel
                    {
                        density = DensityFunction(new Vector3(x, y, z))
                    });
                }

        VoxelChunk chunk = new VoxelChunk(chunkSize, voxelScale, isoLevel, dataStructure);
        var meshData = meshGenerator.generateMesh(chunk, DensityFunction);

        mesh = new Mesh();
        mesh.SetVertices(meshData.vertices);
        mesh.SetTriangles(meshData.triangleIndicies, 0);
        if (meshData.normals == null || meshData.normals.Length == 0)
        {
            mesh.RecalculateNormals();
        }
        else
        {
            mesh.SetNormals(meshData.normals);
        }
    }

    void Update()
    {
        if (mesh != null)
        {
            Graphics.DrawMesh(mesh, Vector3.zero, Quaternion.identity, material, 0);

        }
    }


    public void AddChunk(Vector3Int pos)
    {
        throw new NotImplementedException();
    }
    public void RemoveChunk(Vector3Int pos)
    {
        throw new NotImplementedException();
    }
    public VoxelChunk GetChunk(Vector3Int pos)
    {
        throw new NotImplementedException();
    }
    public void AddDensity(Vector3Int pos, float[][][] densities)
    {
        throw new NotImplementedException();
    }
    public void SetDensity(Vector3Int pos, float[][][] densities)
    {
        throw new NotImplementedException();
    }
    public void RemoveDensity(Vector3Int pos, float[][][] densities)
    {
        throw new NotImplementedException();
    }
    public VoxelMaterial GetMaterial(Vector3 pos)
    {
        throw new NotImplementedException();
    }
    public void SetMaterial(Vector3 pos, byte materialIndex)
    {
        throw new NotImplementedException();
    }
    public void SetMaterialInRadius(Vector3 pos, float radius, byte materialIndex)
    {
        throw new NotImplementedException();
    }
}