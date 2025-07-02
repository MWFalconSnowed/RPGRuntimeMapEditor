using UnityEngine;
using System.IO;

[RequireComponent(typeof(SpriteRenderer))]
public class MapBackground : MonoBehaviour
{
    [Tooltip("Nom de la map sans extension, exemple 'MAP001'")]
    public string currentMapName;

    public Sprite fallbackSprite;

    private const float pixelsPerUnit = 100f;

    void Start()
    {
        LoadMapSprite(currentMapName);
    }

    public void LoadMapSprite(string mapName)
    {
        string fullPath = Path.Combine(Application.streamingAssetsPath, "Maps", mapName + ".png");

        if (!File.Exists(fullPath))
        {
            Debug.LogWarning($"🧱 Image introuvable dans StreamingAssets : {fullPath}");
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

            // Centre la map sur la caméra
            float mapWidthUnits = tex.width / pixelsPerUnit;
            float mapHeightUnits = tex.height / pixelsPerUnit;
            transform.position = new Vector3(mapWidthUnits / 2f, mapHeightUnits / 2f, 0f);

            // Configure caméra orthographique
            if (Camera.main != null)
            {
                Camera.main.orthographic = true;
                Camera.main.orthographicSize = mapHeightUnits / 2f;
                Camera.main.transform.position = new Vector3(mapWidthUnits / 2f, mapHeightUnits / 2f, -10f);
            }

            Debug.Log($"🖼️ Map '{mapName}' chargée. Pos: {transform.position}, Taille: {mapWidthUnits}x{mapHeightUnits} unités");
        }
        catch (System.Exception ex)
        {
            Debug.LogError($"⚠️ Erreur chargement map '{mapName}': {ex.Message}");
            if (fallbackSprite != null)
                GetComponent<SpriteRenderer>().sprite = fallbackSprite;
        }
    }
}
