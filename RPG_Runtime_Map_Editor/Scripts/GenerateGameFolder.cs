using System.IO;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class MapExportData
{
    public List<Vector2> polygonPoints = new List<Vector2>();
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

    public string customMapName = "MAP001"; // Sans extension

    public void Generate()
    {
        try
        {
            string desktopPath = System.Environment.GetFolderPath(System.Environment.SpecialFolder.Desktop);
            string basePath = Path.Combine(desktopPath, "GeneratedGame");
            customMapName = FindNextMapName();

            string mapDir = Path.Combine(basePath, "Maps");
            string prefabDir = Path.Combine(basePath, "Prefabs");
            string resDir = Path.Combine(basePath, "Resources");
            string statsDir = Path.Combine(basePath, "Stats");

            Directory.CreateDirectory(basePath);
            Directory.CreateDirectory(mapDir);
            Directory.CreateDirectory(prefabDir);
            Directory.CreateDirectory(resDir);
            Directory.CreateDirectory(statsDir); // 🔧 oublié dans ton script

            string jsonPath = Path.Combine(mapDir, customMapName + ".json");
            string imgPath = Path.Combine(mapDir, customMapName + ".png");
            string metaPath = Path.Combine(basePath, "GameMeta.json");

            string prefabPath = Path.Combine(prefabDir, "PlayerPrefab.txt");
            string statsPath = Path.Combine(statsDir, "PlayerStats.txt");
            string readmePath = Path.Combine(basePath, "README.txt");

            // Sauvegarde du polygone
            PolygonCollider2D poly = FindObjectOfType<PolygonCollider2D>();
            if (poly != null)
            {
                MapExportData data = new MapExportData();
                data.polygonPoints.AddRange(poly.points);
                string json = JsonUtility.ToJson(data, true);
                File.WriteAllText(jsonPath, json);
            }

            // Capture PNG de la map
            if (captureCamera != null)
                SaveScreenshot(imgPath);

            // Sauvegarde du prefab
            string prefabNote = playerPrefab != null ? playerPrefab.name : "❌ No PlayerPrefab Assigned";
            File.WriteAllText(prefabPath, "Use prefab: " + prefabNote);

            // Statistiques RPG
            if (playerStatsRef != null)
            {
                File.WriteAllText(statsPath,
                    "👤 Name: " + playerStatsRef.playerName + "\n" +
                    "❤️ HP: " + playerStatsRef.hp + "\n" +
                    "🔷 MP: " + playerStatsRef.mp + "\n" +
                    "⭐ Level: " + playerStatsRef.level + "\n" +
                    "💪 Strength: " + playerStatsRef.strength + "\n" +
                    "🛡️ Defense: " + playerStatsRef.defense + "\n");
            }

            // Métadonnées
            GameMetadata meta = new GameMetadata
            {
                createdAt = System.DateTime.Now.ToString("yyyy-MM-dd HH:mm"),
                mapName = customMapName,
                unityVersion = Application.unityVersion
            };
            File.WriteAllText(metaPath, JsonUtility.ToJson(meta, true));

            // README
            File.WriteAllText(readmePath,
                "📝 Données exportées automatiquement.\n" +
                "- " + customMapName + ".json : données de collision\n" +
                "- " + customMapName + ".png : aperçu visuel\n" +
                "- PlayerPrefab.txt : prefab utilisé\n" +
                "- PlayerStats.txt : stats RPG\n" +
                "- GameMeta.json : métadonnées");

            Debug.Log("✅ Export complet dans : " + basePath);
        }
        catch (System.Exception ex)
        {
            Debug.LogError("❌ ERREUR EXPORT : " + ex.Message);
        }
    }

    public string FindNextMapName()
    {
        string mapsDir = Path.Combine(Application.dataPath, "Maps");
        Directory.CreateDirectory(mapsDir);
        int index = 1;

        while (File.Exists(Path.Combine(mapsDir, $"MAP{index:D3}.json")))
        {
            index++;
        }

        return $"MAP{index:D3}";
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
