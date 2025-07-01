using UnityEngine;
using System.Collections.Generic;
using System.IO;

public class PolygonEditorRuntimeGUI : MonoBehaviour
{
    public Texture2D mapTexture;

    private List<List<Vector2>> polygons = new List<List<Vector2>>();
    private List<Vector2> currentPolygon = new List<Vector2>();
    private Texture2D pixelTex;

    private string SavePath => Path.Combine(Application.persistentDataPath, "polygons.txt");
    private bool drawMode = true;

    void Start()
    {
        pixelTex = new Texture2D(1, 1);
        pixelTex.SetPixel(0, 0, Color.cyan);
        pixelTex.Apply();

        LoadPolygons();
    }

    void OnGUI()
    {
        if (mapTexture != null)
        {
            GUI.DrawTexture(new Rect(0, 0, mapTexture.width, mapTexture.height), mapTexture);
        }

        // Dessin des traits
        DrawAllPolygons();
        DrawCurrentPolygon();

        // Interface
        GUILayout.BeginArea(new Rect(10, 10, 300, 220), GUI.skin.box);
        GUILayout.Label("🧭 Editeur de Polygones Runtime");

        drawMode = GUILayout.Toggle(drawMode, "✍️ Mode dessin");

        if (GUILayout.Button("✅ Fermer polygone") && currentPolygon.Count >= 3)
        {
            polygons.Add(new List<Vector2>(currentPolygon));
            CreateCollider(currentPolygon);
            currentPolygon.Clear();
        }

        if (GUILayout.Button("💾 Sauver")) SavePolygons();
        if (GUILayout.Button("📥 Charger")) { ClearAll(); LoadPolygons(); }
        if (GUILayout.Button("🧹 Effacer tout")) ClearAll();

        GUILayout.Label("📌 Clic gauche : ajouter point");
        GUILayout.Label("📌 Clic droit : annuler polygone");

        GUILayout.EndArea();
    }

    void Update()
    {
        if (!drawMode) return;

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

    void DrawLine(Vector2 a, Vector2 b, Color color, float thickness = 2f)
    {
        Vector2 start = Camera.main.WorldToScreenPoint(a);
        Vector2 end = Camera.main.WorldToScreenPoint(b);
        start.y = Screen.height - start.y;
        end.y = Screen.height - end.y;

        Vector2 delta = end - start;
        float angle = Mathf.Atan2(delta.y, delta.x) * Mathf.Rad2Deg;
        float length = delta.magnitude;

        Matrix4x4 matrixBackup = GUI.matrix;
        GUIUtility.RotateAroundPivot(angle, start);
        GUI.color = color;
        GUI.DrawTexture(new Rect(start.x, start.y, length, thickness), pixelTex);
        GUI.matrix = matrixBackup;
    }

    void DrawAllPolygons()
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
    }

    void DrawCurrentPolygon()
    {
        for (int i = 0; i < currentPolygon.Count - 1; i++)
        {
            DrawLine(currentPolygon[i], currentPolygon[i + 1], Color.yellow);
        }
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
        Debug.Log("✅ Sauvegarde : " + SavePath);
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
                if (split.Length == 2 && float.TryParse(split[0], out float x) && float.TryParse(split[1], out float y))
                    poly.Add(new Vector2(x, y));
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
            Destroy(poly.gameObject);
    }
}
