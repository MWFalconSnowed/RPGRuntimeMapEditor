using UnityEngine;
using System.IO;

public class SaveLoadManager : MonoBehaviour
{
    public static void SaveMap(Tile[,] tiles, int mapWidth, int mapHeight)
    {
        MapData mapData = new MapData(mapWidth, mapHeight);

        for (int x = 0; x < mapWidth; x++)
        {
            for (int y = 0; y < mapHeight; y++)
            {
                mapData.tileTypes[x, y] = tiles[x, y].tileIndex; // Sauvegarde l'index de la tuile
            }
        }

        string json = JsonUtility.ToJson(mapData);
        File.WriteAllText("Assets/Maps/saved_map.json", json);
        Debug.Log("Map Saved!");
    }

    public static void LoadMap(Tile[,] tiles, int mapWidth, int mapHeight)
    {
        string json = File.ReadAllText("Assets/Maps/saved_map.json");
        MapData mapData = JsonUtility.FromJson<MapData>(json);

        for (int x = 0; x < mapWidth; x++)
        {
            for (int y = 0; y < mapHeight; y++)
            {
                tiles[x, y].ChangeTile(mapData.tileTypes[x, y]);
            }
        }

        Debug.Log("Map Loaded!");
    }
}

[System.Serializable]
public class MapData
{
    public int[,] tileTypes;

    public MapData(int width, int height)
    {
        tileTypes = new int[width, height];
    }
}
