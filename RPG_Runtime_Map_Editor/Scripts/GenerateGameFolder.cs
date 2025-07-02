using System.IO;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class MapExportData
{
    public List<Vector2> polygonPoints = new List<Vector2>();
}

[System.Serializable]
public class RPGStats
{
    public int level = 1;
    public float health = 100;
    public float mana = 50;
    public float stamina = 75;
}

[System.Serializable]
public class GameMetadata
{
    public string createdAt;
    public string mapName;
    public string unityVersion;
}

public class GenerateGameFolder : MonoBehaviour
{
    public GameObject playerPrefab;
    public Camera captureCamera;
    public RPGPlayer playerStatsRef;

    public void Generate()
    {
        string basePath = Path.Combine(Application.persistentDataPath, "GeneratedGame");
        string mapDir = Path.Combine(basePath, "Maps");
        string prefabDir = Path.Combine(basePath, "Prefabs");
        string resDir = Path.Combine(basePath, "Resources");

        Directory.CreateDirectory(basePath);
        Directory.CreateDirectory(mapDir);
        Directory.CreateDirectory(prefabDir);
        Directory.CreateDirectory(resDir);

        // Save polygon
        PolygonCollider2D poly = FindObjectOfType<PolygonCollider2D>();
        if (poly != null)
        {
            MapExportData data = new MapExportData();
            data.polygonPoints.AddRange(poly.points);
            string json = JsonUtility.ToJson(data, true);
            File.WriteAllText(Path.Combine(mapDir, "MAP001.json"), json);
        }

        // Save screenshot
        if (captureCamera != null)
        {
            string imgPath = Path.Combine(mapDir, "MAP001.png");
            SaveScreenshot(imgPath);
        }

        // Save player prefab name
        string prefabNote = playerPrefab != null ? playerPrefab.name : "No PlayerPrefab Assigned";
        File.WriteAllText(Path.Combine(prefabDir, "PlayerPrefab.txt"), "Use prefab: " + prefabNote);

        // Save player stats
        if (playerStatsRef != null)
        {
            RPGStats stats = new RPGStats
            {
                level = playerStatsRef.level,
                health = playerStatsRef.health,
                mana = playerStatsRef.mana,
                stamina = playerStatsRef.stamina
            };
            File.WriteAllText(Path.Combine(basePath, "RPGPlayerStats.json"), JsonUtility.ToJson(stats, true));
        }

        // Metadata
        GameMetadata meta = new GameMetadata
        {
            createdAt = System.DateTime.Now.ToString("yyyy-MM-dd HH:mm"),
            mapName = "MAP001",
            unityVersion = Application.unityVersion
        };
        File.WriteAllText(Path.Combine(basePath, "GameMeta.json"), JsonUtility.ToJson(meta, true));

        // README
        File.WriteAllText(Path.Combine(basePath, "README.txt"),
            "📝 Données exportées automatiquement.\n" +
            "- MAP001.json : données de collision\n" +
            "- MAP001.png : aperçu visuel\n" +
            "- PlayerPrefab.txt : prefab utilisé\n" +
            "- RPGPlayerStats.json : stats RPG\n" +
            "- GameMeta.json : métadonnées");

        Debug.Log("✅ Export terminé dans : " + basePath);
    }

    private void SaveScreenshot(string path)
    {
        RenderTexture rt = new RenderTexture(1980, 1080, 24);
        captureCamera.targetTexture = rt;
        Texture2D tex = new Texture2D(1980, 1080, TextureFormat.RGB24, false);
        captureCamera.Render();
        RenderTexture.active = rt;
        tex.ReadPixels(new Rect(0, 0, 1980, 1080), 0, 0);
        captureCamera.targetTexture = null;
        RenderTexture.active = null;
        byte[] bytes = tex.EncodeToPNG();
        File.WriteAllBytes(path, bytes);
    }
}
