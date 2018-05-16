using UnityEngine;
using System.Collections;
using System;
using System.IO;

public class TerrainHeightMap : MonoBehaviour {
    private Terrain terrain;
    public static TerrainHeightMap Instance { get; private set; }
    public GameObject BuildObject;
    private volatile float[,] heights;

    public int Width
    {
        get
        {
            return GameParams.Width;
        }
    }

    public int Length
    {
        get
        {
            return GameParams.Length;
        }
    }

    public int Height
    {
        get
        {
            return GameParams.Height;
        }
    }

    public float Scale
    {
        get
        {
            return GameParams.MapScale;
        }
    }

    public float Seed
    {
        get
        {
            return GameParams.Seed;
        }
    }

    public float GetHeight(int x, int y)
    {
        return heights[y, x];
    }

    public float GetHeight(Vector2Int pos)
    {
        return GetHeight(pos.x, pos.y);
    }

    void Awake()
    {
        terrain = GetComponent<Terrain>();  //  находим террейн
        if (!GameConstants.IsMapEditorActive && (GameParams.MapType == MapTypes.DefaultMaps && GameParams.MapId != 0 ||
            GameParams.MapType != MapTypes.DefaultMaps))
            SetTerrainData();
        else
        {
            GameParams.Width = Mathf.RoundToInt(terrain.terrainData.size.x); // width
            GameParams.Height = Mathf.RoundToInt(terrain.terrainData.size.y); // height
            GameParams.Length = Mathf.RoundToInt(terrain.terrainData.size.z); // length
            //terrain.terrainData = GetTerrainData(terrain.terrainData, new float[Width, Length]);
        }
        Instance = this;
        heights = new float[GameParams.Length, GameParams.Width];
        for (int y = 0; y < Length; y++)
            for (int x = 0; x < Width; x++)
                heights[y, x] = terrain.terrainData.GetHeight(x, y);
        //if (GameConstants.IsMapEditorActive)
        //    SaveHeightMap();
    }

    private void SetTerrainData()
    {
        float[,] heightMap;
        if (GameParams.MapType == MapTypes.DefaultMaps)
            heightMap = LoadHeightMapFromFile();
        else if (GameParams.MapType == MapTypes.Generation)
            heightMap = GenerateHeightMap();
        else
            heightMap = new float[Length, Width];
        terrain.terrainData = GetTerrainData(terrain.terrainData, heightMap);
        if (GameParams.MapType == MapTypes.Labyrinth)
            CreateMaze();
    }

    private float[,] GenerateHeightMap()
    {
        float[,] result = new float[Length, Width];  //  создаем массив карты высот
        for (int y = 0; y < Length; y++)
            for (int x = 0; x < Width; x++)
                result[y, x] = Mathf.PerlinNoise(((float)x + Seed) / (float)Width * Scale, ((float)y + Seed) / (float)Length * Scale);
        //  присваиваем значения карте высот с использованием шума Перлина
        return result;
    }

    private TerrainData GetTerrainData(TerrainData data, float[,] heightMap)
    {
        data.heightmapResolution = Width + 1;  //  задаём разрешение карты высот
        data.size = new Vector3(Width, Height, Length);  //  задаём размеры террейна
        data.SetHeights(0, 0, heightMap);  //  присваиваем карту высот
        return data;
    }

    private void SaveHeightMap()
    {
        float [,] heightMap = terrain.terrainData.GetHeights(0, 0, Mathf.RoundToInt(terrain.terrainData.size.x), Mathf.RoundToInt(terrain.terrainData.size.z));
        using (BinaryWriter writer = new BinaryWriter(File.Open(GameConstants.MapPath + GameConstants.MapEditorOutputFileName + 
            GameConstants.MapFileEtension, FileMode.CreateNew)))
        {
            writer.Write(Mathf.RoundToInt(terrain.terrainData.size.x)); // width
            writer.Write(Mathf.RoundToInt(terrain.terrainData.size.y)); // height
            writer.Write(Mathf.RoundToInt(terrain.terrainData.size.z)); // length
            for (int y = 0; y < terrain.terrainData.size.z; y++)
                for (int x = 0; x < terrain.terrainData.size.x; x++)
                    writer.Write(heightMap[y, x]);
            writer.Close();
        }
    }

    private float[,] LoadHeightMapFromFile()
    {
        float[,] result;
        using (BinaryReader reader = new BinaryReader(File.Open(GameParams.MapNames[GameParams.MapId-1], FileMode.Open)))
        {
            GameParams.Width = reader.ReadInt32();
            GameParams.Height = reader.ReadInt32();
            GameParams.Length = reader.ReadInt32();
            result = new float[Length, Width];
            for (int y = 0; y < Length; y++)
                for (int x = 0; x < Width; x++)
                    result[y, x] = reader.ReadSingle();
            reader.Close();
        }
        return result;
    }

    private void CreateMaze()
    {
        GameObject target;
        MazeGenerator generator = new MazeGenerator(Width, Length);
        bool [,] field = generator.GenerateMaze();
        for(int y = 0; y < Length; y++)
            for (int x = 0; x < Width; x++)
                if (field[y, x])
                    target = Instantiate(BuildObject, new Vector3(x, 0, y), new Quaternion()) as GameObject;
    }
}
