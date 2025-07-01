using UnityEngine;
using System.Collections.Generic;
using System.IO;

public class PolygonEditorRuntimeGUI : MonoBehaviour
{
    public Texture2D mapTexture;
    public GameObject mapPlanePrefab; // <-- assignable GameObject in scene (Quad, Sprite, etc.)

    private GameObject mapPlaneInstance;
    private List<List<Vector2>> polygons = new List<List<Vector2>>();
    private List<Vector2> currentPolygon = new List<Vector2>();
    private Texture2D pixelTex;
    private bool drawMode = false;
    private bool mapLoaded = false;

    private string SavePath => Path.Combine(Application.persistentDataPath, "polygons.txt");

    void Start()
    {
        pixelTex = new Texture2D(1, 1);
        pixelTex.SetPixel(0, 0, Color.cyan);
        pixelTex.Apply();
    }

    void OnGUI()
    {
        GUILayout.BeginArea(new Rect(10, 10, 300, 300), GUI.skin.box);
        GUILayout.Label("🧭 Polygon Editor (Runtime)");

        if (!mapLoaded && GUILayout.Button("📥 Load Map"))
        {
            LoadMap();
        }

        if (mapLoaded)
        {
            drawMode = GUILayout.Toggle(drawMode, "✍️ Draw Mode");

            if (GUILayout.Button("✅ Close Polygon") && currentPolygon.Count >= 3)
            {
                polygons.Add(new List<Vector2>(currentPolygon));
                CreateCollider(currentPolygon);
                currentPolygon.Clear();
            }

            if (GUILayout.Button("💾 Save")) SavePolygons();
            if (GUILayout.Button("📂 Load")) { ClearAll(); LoadPolygons(); }
            if (GUILayout.Button("🧹 Clear All")) ClearAll();

            GUILayout.Label("📌 Left Click: Add Point");
            GUILayout.Label("📌 Right Click: Cancel Polygon");
        }

        GUILayout.EndArea();

        if (mapLoaded)
        {
            DrawPolygons();
        }
    }

    void Update()
    {
        if (!mapLoaded || !drawMode) return;

        Vector2 mouseWorld = Camera.main.ScreenToWorldPoint(Input.mousePosition);

        if (Input.GetMouseButtonDown(0))
        {
            currentPolygon.Add(mouseWorld);
        }

        if (Input.GetMouseButtonDown(1))
        {
            currentPolygon.Clear();
        }
    }

    void LoadMap()
    {
        if (mapTexture == null || mapPlanePrefab == null) return;

        mapPlaneInstance = Instantiate(mapPlanePrefab, Vector3.zero, Quaternion.identity);
        var renderer = mapPlaneInstance.GetComponent<Renderer>();
        if (renderer != null) renderer.material.mainTexture = mapTexture;

        mapLoaded = true;
        LoadPolygons();
    }

    void DrawPolygons()
    {
        foreach (var poly in polygons)
        {
            for (int i = 0; i < poly.Count; i++)
            {
                Vector2 a = poly[i];
                Vector2 b = poly[(i + 1) % poly.Count];
                DrawLine(a, b, Color.cyan);
            }
        }

        for (int i = 0; i < currentPolygon.Count - 1; i++)
        {
            DrawLine(currentPolygon[i], currentPolygon[i + 1], Color.yellow);
        }
    }

    void DrawLine(Vector2 a, Vector2 b, Color color, float thickness = 2f)
    {
        Vector2 start = Camera.main.WorldToScreenPoint(a);
        Vector2 end = Camera.main.WorldToScreenPoint(b);
        start.y = Screen.height - start.y;
        end.y = Screen.height - end.y;

        Vector2 delta = end - start;
        float angle = Mathf.Atan2(delta.y, delta.x) * Mathf.Rad2Deg;
        float length = delta.magnitude;

        Matrix4x4 backup = GUI.matrix;
        GUIUtility.RotateAroundPivot(angle, start);
        GUI.color = color;
        GUI.DrawTexture(new Rect(start.x, start.y, length, thickness), pixelTex);
        GUI.matrix = backup;
    }

    void CreateCollider(List<Vector2> points)
    {
        GameObject go = new GameObject("Polygon_" + polygons.Count);
        var col = go.AddComponent<PolygonCollider2D>();
        col.points = points.ToArray();
    }

    void SavePolygons()
    {
        using (StreamWriter sw = new StreamWriter(SavePath))
        {
            foreach (var poly in polygons)
            {
                string line = string.Join("|", poly.ConvertAll(p => $"{p.x},{p.y}"));
                sw.WriteLine(line);
            }
        }
    }

    void LoadPolygons()
    {
        if (!File.Exists(SavePath)) return;

        string[] lines = File.ReadAllLines(SavePath);
        foreach (var line in lines)
        {
            string[] points = line.Split('|');
            List<Vector2> poly = new List<Vector2>();

            foreach (string pt in points)
            {
                string[] split = pt.Split(',');
                if (split.Length == 2 &&
                    float.TryParse(split[0], out float x) &&
                    float.TryParse(split[1], out float y))
                {
                    poly.Add(new Vector2(x, y));
                }
            }

            if (poly.Count >= 3)
            {
                polygons.Add(poly);
                CreateCollider(poly);
            }
        }
    }

    void ClearAll()
    {
        polygons.Clear();
        currentPolygon.Clear();

        foreach (var poly in GameObject.FindObjectsOfType<PolygonCollider2D>())
        {
            Destroy(poly.gameObject);
        }
    }
}
