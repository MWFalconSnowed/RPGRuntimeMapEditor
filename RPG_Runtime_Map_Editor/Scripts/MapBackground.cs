using UnityEngine;
using System.IO;

[RequireComponent(typeof(SpriteRenderer))]
public class MapBackground : MonoBehaviour
{
    public string currentMapName;
    public Sprite fallbackSprite;

    private const float pixelsPerUnit = 100f;

    void Start()
    {
        LoadMapSprite(currentMapName);
    }

    public void LoadMapSprite(string mapName)
    {
        string fullPath = Path.Combine(Application.dataPath, "Maps", mapName + ".png");

        if (!File.Exists(fullPath))
        {
            Debug.LogWarning("🧱 Image introuvable : " + fullPath);
            if (fallbackSprite != null)
                GetComponent<SpriteRenderer>().sprite = fallbackSprite;
            return;
        }

        try
        {
            byte[] imageData = File.ReadAllBytes(fullPath);
            Texture2D tex = new Texture2D(2, 2);
            tex.LoadImage(imageData);

            Sprite sprite = Sprite.Create(tex, new Rect(0, 0, tex.width, tex.height), new Vector2(0.5f, 0f), pixelsPerUnit);
            var sr = GetComponent<SpriteRenderer>();
            sr.sprite = sprite;

            transform.localScale = Vector3.one;

            // ✅ Positionne le coin bas-gauche en (0,0)
            float mapWidth = tex.width / pixelsPerUnit;
            float mapHeight = tex.height / pixelsPerUnit;
            transform.position = new Vector3(mapWidth / 2f, mapHeight / 2f, 0f);

            // ✅ Caméra forcée à Y = 10.8
            if (Camera.main != null)
            {
                Camera.main.orthographic = true;
                Camera.main.orthographicSize = mapHeight / 2f;
                Camera.main.transform.position = new Vector3(mapWidth / 2f, 10.8f, -10f);
            }

            Debug.Log($"🖼️ Map chargée : {mapName} | Pos caméra Y = 10.8");
        }
        catch (System.Exception ex)
        {
            Debug.LogError("⚠️ Erreur lors du chargement : " + ex.Message);
            if (fallbackSprite != null)
                GetComponent<SpriteRenderer>().sprite = fallbackSprite;
        }
    }
}
