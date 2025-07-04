using System.Collections.Generic;
using System.IO;
using UnityEditor;
using UnityEngine;

[System.Serializable]
public class PolygonData
{
    public List<Vector2> points = new List<Vector2>();
}

public class UltimateRuntimeEditor : MonoBehaviour
{
    public List<Vector2> currentPolygon = new List<Vector2>();
    private float zoom = 1f;
    private Vector3 dragOrigin;
    private bool dragging = false;
    private Color polygonColor = Color.cyan;
    public GameObject playerPrefab; // À glisser dans l'inspecteur
    private GenerateGameFolder generator;
    private Vector2 mapScroll;
    private string selectedMapPath = "";
    private bool GUIElementsVisible = true;
    bool IsMouseOverGUIRect(Rect rect)
    {
        Vector2 mouseGUI = new Vector2(Input.mousePosition.x, Screen.height - Input.mousePosition.y);
        return rect.Contains(mouseGUI);
    }





    void OnGUI()
    {
        if (isPreviewMode)
        {
            GUI.color = new Color(1f, 1f, 1f, 0.12f);
            GUI.DrawTexture(new Rect(0, 0, Screen.width, Screen.height), Texture2D.whiteTexture);
            GUI.color = Color.white;
        }
        if (showConsole)
            DrawConsole();

        if (GUILayout.Button(showConsole ? "🔒 Fermer Console" : "💬 Ouvrir Console"))
        {
            showConsole = !showConsole;
        }

        if (!GUIElementsVisible)
            return;

        if (showConsole)
            DrawConsole();

        GUILayout.BeginArea(new Rect(10, 10, 240, 300), GUI.skin.box);
        GUILayout.Label("🗺️ Sélection de la map");
        string[] mapPaths = Directory.GetFiles(Application.dataPath + "/MapExports", "*.json");
        mapScroll = GUILayout.BeginScrollView(mapScroll, GUILayout.Height(200));
        foreach (string path in mapPaths)
        {
            string fileName = Path.GetFileName(path);
            if (GUILayout.Button(fileName))
            {
                selectedMapPath = path;
                LoadMapFromPath(selectedMapPath);
            }
        }
        GUILayout.EndScrollView();
        GUILayout.Label("Map sélectionnée :");
        GUILayout.Label(Path.GetFileName(selectedMapPath));
        GUILayout.EndArea();

        Color originalColor = GUI.backgroundColor;
        GUI.backgroundColor = new Color(0f, 0f, 0f, 0.6f);

        GUIStyle panelStyle = new GUIStyle(GUI.skin.box)
        {
            normal = { textColor = Color.white },
            fontSize = 14,
            alignment = TextAnchor.UpperLeft,
            padding = new RectOffset(10, 10, 10, 10)
        };

        GUILayout.BeginArea(new Rect(Screen.width - 270, 10, 260, 850), panelStyle);
        GUIStyle titleStyle = new GUIStyle(GUI.skin.label)
        {
            fontStyle = FontStyle.Bold,
            fontSize = 16
        };
        GUILayout.Label("Ultimate Runtime Editor", titleStyle);

        GUILayout.Space(5);
        GUILayout.Label("📐 Polygon Tools");
        if (GUILayout.Button("↩️ Undo")) UndoLast();
        if (GUILayout.Button("💾 Sauvegarder")) SavePolygon();
        if (GUILayout.Button("📂 Charger")) LoadPolygon();
        if (GUILayout.Button("🧱 Générer Collider")) GenerateCollider();

        GUILayout.Space(10);
        GUILayout.Label("🎨 Couleur du polygone");
        if (GUILayout.Button("🔵 Cyan")) ChangeColor(Color.cyan);
        if (GUILayout.Button("🟢 Vert")) ChangeColor(Color.green);
        if (GUILayout.Button("🔴 Rouge")) ChangeColor(Color.red);
        if (GUILayout.Button("💾 Exporter Map JSON")) ExportMapData();

        GUILayout.Space(10);
        GUILayout.Label("🧍 Spawn Joueur");
        GUI.enabled = currentPolygon.Count >= 3 && playerPrefab != null;
        if (GUILayout.Button("🚀 Spawn")) SpawnPlayerOutsidePolygon();
        GUI.enabled = true;

        GUILayout.Space(10);
        if (GUILayout.Button("🖼️ Export PNG")) ExportPNG();
        if (GUILayout.Button("🛠 Generate Game Folder"))
        {
            if (generator != null)
                generator.Generate();
            else
                Debug.LogWarning("⛔ Aucun composant GenerateGameFolder trouvé !");
        }

        GUILayout.EndArea();

        GUIStyle tipStyle = new GUIStyle(GUI.skin.label)
        {
            normal = { textColor = Color.white },
            fontSize = 12
        };
        GUI.Label(new Rect(10, Screen.height - 250, 850, 80),
            "🖱️ Clic gauche = Ajouter point\n" +
            "🔄 Molette = Zoom\n" +
            "✋ Clic milieu = Drag\n" +
            "⌫ Backspace = Undo", tipStyle);

        GUI.backgroundColor = originalColor;
    }



    [System.Serializable]
    public class MapExportData
    {
        public List<Vector2> polygonPoints = new List<Vector2>();
        public string mapName = "MAP001";
        public Vector2 playerSpawn = new Vector2(142.78f, 69.89999f);
    }

    void ExportMapData()
    {
        PolygonCollider2D poly = FindObjectOfType<PolygonCollider2D>();
        if (poly == null)
        {
            Debug.LogWarning("❌ Aucun PolygonCollider2D trouvé !");
            return;
        }

        MapExportData data = new MapExportData();
        data.polygonPoints.AddRange(poly.points); // ou poly.GetPath(0)

        string json = JsonUtility.ToJson(data, true);
        string path = Application.dataPath + "/MapExports/map_" + System.DateTime.Now.Ticks + ".json";

        System.IO.Directory.CreateDirectory(Application.dataPath + "/MapExports");
        System.IO.File.WriteAllText(path, json);

        Debug.Log("✅ Export JSON sauvegardé : " + path);
    }
    private bool isPreviewMode = false;

    void Update()
    {
        isPreviewMode = Input.GetKey(KeyCode.LeftAlt) || Input.GetKey(KeyCode.RightAlt);

        if (isPreviewMode)
        {
            GUIElementsVisible = false;
        }
        else
        {
            GUIElementsVisible = true;
        }
        if (Input.GetKeyDown(KeyCode.KeypadMultiply)); // Touche * du pavé numérique
    {
            showConsole = !showConsole;
        }

        GUIElementsVisible = !showConsole; // 
    }

    void LateUpdate()
    {
        HandleZoom();
        HandlePan();
        HandleAddPoint();
        HandleKeyboardShortcuts();
    }

    void SpawnPlayerOutsidePolygon()
    {
        if (playerPrefab == null)
        {
            Debug.LogWarning("❌ Aucun prefab joueur assigné.");
            return;
        }

        PolygonCollider2D polyCollider = FindObjectOfType<PolygonCollider2D>(); // récupère le collider existant

        if (polyCollider == null)
        {
            Debug.LogWarning("❌ PolygonCollider2D manquant.");
            return;
        }

        Vector3 spawnPos = new Vector3(6.78f, 6.97f, 0f);

        if (polyCollider.OverlapPoint(spawnPos))
        {
            Debug.LogWarning("⚠️ La position de spawn est à l'intérieur du polygone !");
            return;
        }

        GameObject playerInstance = Instantiate(playerPrefab, spawnPos, Quaternion.identity);
        playerInstance.transform.localScale = Vector3.one * 3f;
        Debug.Log("🧍 Joueur apparu à la position : " + spawnPos);
    }



    void OnPostRender()
    {
        if (!Application.isPlaying || currentPolygon == null || currentPolygon.Count < 2)
            return;

        GL.PushMatrix();
        GL.LoadOrtho();
        Material mat = new Material(Shader.Find("Hidden/Internal-Colored"));
        mat.SetPass(0);
        GL.Begin(GL.LINES);
        GL.Color(polygonColor);

        for (int i = 0; i < currentPolygon.Count; i++)
        {
            Vector3 start = Camera.main.WorldToViewportPoint(currentPolygon[i]);
            Vector3 end = Camera.main.WorldToViewportPoint(currentPolygon[(i + 1) % currentPolygon.Count]);

            GL.Vertex(start);
            GL.Vertex(end);
        }

        GL.End();
        GL.PopMatrix();
    }


    void HandleZoom()
    {
#if UNITY_EDITOR
        if (Event.current != null && Event.current.type == EventType.ScrollWheel)
        {
            float scroll = Event.current.delta.y;
            zoom = Mathf.Clamp(zoom + scroll * 0.01f, 0.1f, 3f);
            UnityEditor.SceneView.RepaintAll();
            Event.current.Use();
        }
#endif
    }


    void Start()
    {
        string exportPath = Application.dataPath + "/MapExports";
        if (!Directory.Exists(exportPath))
        {
            Directory.CreateDirectory(exportPath);
            Debug.Log("📁 Dossier MapExports créé automatiquement : " + exportPath);
        }
        Screen.SetResolution(1980, 1080, false);

        Debug.Log("Dossier de génération : " + Application.persistentDataPath);

        generator = FindObjectOfType<GenerateGameFolder>();
        Camera.main.transform.position = new Vector3(9.9f, 10.8f, -10f); // pour 2D


        Camera.main.orthographic = true;
        // 🔥 Supprime le point (0,0) s’il est tout seul
        if (currentPolygon.Count == 1 && currentPolygon[0] == Vector2.zero)
        {
            currentPolygon.Clear();
            Debug.Log("🧹 Point (0,0) retiré à l'initialisation.");
        }
  
        Camera.main.orthographicSize = 5.9f;
    }
    private Vector3 dragOriginWorld;

    void HandlePan()
    {
        if (Input.GetMouseButtonDown(2))  // Clic molette enfoncé
        {
            // Convertit la position souris écran en position monde
            dragOriginWorld = Camera.main.ScreenToWorldPoint(Input.mousePosition);
            dragging = true;
        }

        if (Input.GetMouseButton(2) && dragging)
        {
            // Calcul déplacement entre dernière position et actuelle en world space
            Vector3 currentWorldPos = Camera.main.ScreenToWorldPoint(Input.mousePosition);
            Vector3 diff = dragOriginWorld - currentWorldPos;

            // Applique déplacement à la caméra
            Camera.main.transform.position += diff;

            // Met à jour dragOriginWorld pour le prochain calcul
            dragOriginWorld = currentWorldPos;
        }

        if (Input.GetMouseButtonUp(2))
        {
            dragging = false;
        }
    }


    void HandleAddPoint()
    {
        if (UnityEngine.EventSystems.EventSystem.current != null &&
            UnityEngine.EventSystems.EventSystem.current.IsPointerOverGameObject())
            return;

        // Empêche le clic si la souris est dans la zone GUI de droite
        Rect panelRect = new Rect(Screen.width - 270, 10, 260, 850);
        if (IsMouseOverGUIRect(panelRect))
            return;

        // Empêche le clic si la souris est dans la console
        if (showConsole)
        {
            Rect consoleRect = new Rect(10, Screen.height - 150, 610, 150);
            if (IsMouseOverGUIRect(consoleRect))
                return;
        }

        if (Input.GetMouseButtonDown(0))
        {
            Vector2 worldPos = Camera.main.ScreenToWorldPoint(Input.mousePosition);
            currentPolygon.Add(worldPos);
            Debug.Log("🟢 Point ajouté: " + worldPos);
        }
    }



    void HandleKeyboardShortcuts()
    {
        if (Input.GetKeyDown(KeyCode.Backspace))
            UndoLast();
    }

    void OnDrawGizmos()
    {
        if (!Application.isPlaying || currentPolygon == null) return;

        Gizmos.color = polygonColor;

        for (int i = 0; i < currentPolygon.Count; i++)
        {
            Vector2 a = currentPolygon[i];
            Vector2 b = currentPolygon[(i + 1) % currentPolygon.Count];
            Gizmos.DrawLine(a, b);
            Gizmos.DrawSphere(a, 0.1f);
        }
    }

    void ExportPNG()
    {
        string path = Application.persistentDataPath + "/2DRPGEditorMap_export.png";
        ScreenCapture.CaptureScreenshot(path);
        Debug.Log("📸 Exported PNG to: " + path);
    }

    void DrawCirclePoints()
    {
        currentPolygon.Clear();
        Vector2 center = new Vector2(0f, 0f);
        float radius = 3f;
        int segments = 20;

        for (int i = 0; i < segments; i++)
        {
            float angle = i * Mathf.PI * 2f / segments;
            float x = center.x + radius * Mathf.Cos(angle);
            float y = center.y + radius * Mathf.Sin(angle);
            currentPolygon.Add(new Vector2(x, y));
        }

        Debug.Log("🟢 Cercle généré au centre");
    }

    void UndoLast()
    {
        if (currentPolygon.Count > 0)
        {
            currentPolygon.RemoveAt(currentPolygon.Count - 1);
            Debug.Log("↩️ Dernier point supprimé");
        }
        else
        {
            Debug.Log("❌ Aucun point à supprimer");
        }
    }

    void SavePolygon()
    {
        PolygonData data = new PolygonData { points = currentPolygon };
        string json = JsonUtility.ToJson(data, true);
        string path = Path.Combine(Application.persistentDataPath, "polygon_data.json");
        File.WriteAllText(path, json);
        Debug.Log("💾 Polygon saved to: " + path);
    }

    void LoadPolygon()
    {
        string path = Path.Combine(Application.persistentDataPath, "polygon_data.json");
        if (File.Exists(path))
        {
            string json = File.ReadAllText(path);
            PolygonData data = JsonUtility.FromJson<PolygonData>(json);
            currentPolygon = data.points;
            Debug.Log("📂 Polygon loaded from: " + path);
        }
        else
        {
            Debug.LogWarning("❌ No polygon file found at " + path);
        }
    }

    void GenerateCollider()
    {
        GameObject oldCollider = GameObject.Find("GeneratedPolygon");
        if (oldCollider != null)
            DestroyImmediate(oldCollider);

        GameObject go = new GameObject("GeneratedPolygon");
        var rb = go.AddComponent<Rigidbody2D>();
        rb.bodyType = RigidbodyType2D.Static;

        PolygonCollider2D collider = go.AddComponent<PolygonCollider2D>();
        collider.points = currentPolygon.ToArray();
        collider.isTrigger = false;

        Debug.Log("🧱 PolygonCollider2D créé");
    }


    private PolygonExtendedFeatures extended;
    private PolygonAnalyzer analyzer;

    void Awake()
    {
        extended = GetComponent<PolygonExtendedFeatures>();
        if (extended == null)
            extended = gameObject.AddComponent<PolygonExtendedFeatures>();
        analyzer = gameObject.AddComponent<PolygonAnalyzer>();
        analyzer.polygon = currentPolygon;
        extended.polygonReference = currentPolygon;
        if (Application.isPlaying && Camera.main != null)
        {
            Camera.main.orthographicSize = 116.86f;
        }


    }
    private string consoleInput = "";
    private List<string> logHistory = new List<string>();
    private bool showConsole = false;

    void DrawConsole()
    {
        GUILayout.BeginArea(new Rect(10, Screen.height - 140, 600, 130), GUI.skin.box);
        GUILayout.Label("🎮 Console de l'éditeur");

        GUILayout.TextArea(string.Join("\n", logHistory), GUILayout.Height(80));

        GUI.SetNextControlName("ConsoleInput");
        consoleInput = GUILayout.TextField(consoleInput);
        GUI.FocusControl("ConsoleInput");


        GUILayout.EndArea();
    }

    public void AutoPolygonGenerator(Texture2D edgeImage, float alphaThreshold = 0.1f)
    {
        int width = edgeImage.width;
        int height = edgeImage.height;

        Color[] pixels = edgeImage.GetPixels();
        List<List<Vector2>> detectedPolygons = new List<List<Vector2>>();

        bool[,] visited = new bool[width, height];

        for (int y = 0; y < height; y++)
        {
            for (int x = 0; x < width; x++)
            {
                if (visited[x, y]) continue;

                Color pixel = pixels[y * width + x];
                if (pixel.grayscale < alphaThreshold) continue;

                List<Vector2> region = ExtractRegion(pixels, visited, width, height, x, y, alphaThreshold);

                if (region.Count > 20)
                {
                    List<Vector2> simplified = SimplifyPolygon(region, 2f);
                    detectedPolygons.Add(simplified);
                }
            }
        }

        foreach (var poly in detectedPolygons)
        {
            GameObject obj = new GameObject("GeneratedCollider");
            var col = obj.AddComponent<PolygonCollider2D>();
            col.points = poly.ToArray();
            obj.transform.parent = this.transform;
        }

        Debug.Log("✅ AutoPolygonGenerator terminé : " + detectedPolygons.Count + " polygones détectés.");
    }

    private List<Vector2> ExtractRegion(Color[] pixels, bool[,] visited, int width, int height, int startX, int startY, float threshold)
    {
        List<Vector2> region = new List<Vector2>();
        Queue<Vector2Int> queue = new Queue<Vector2Int>();
        queue.Enqueue(new Vector2Int(startX, startY));
        visited[startX, startY] = true;

        while (queue.Count > 0)
        {
            Vector2Int current = queue.Dequeue();
            region.Add(new Vector2(current.x, current.y));

            for (int dx = -1; dx <= 1; dx++)
            {
                for (int dy = -1; dy <= 1; dy++)
                {
                    int nx = current.x + dx;
                    int ny = current.y + dy;

                    if (nx >= 0 && ny >= 0 && nx < width && ny < height && !visited[nx, ny])
                    {
                        Color neighborPixel = pixels[ny * width + nx];
                        if (neighborPixel.grayscale >= threshold)
                        {
                            visited[nx, ny] = true;
                            queue.Enqueue(new Vector2Int(nx, ny));
                        }
                    }
                }
            }
        }

        return region;
    }

    private List<Vector2> SimplifyPolygon(List<Vector2> points, float tolerance)
    {
        if (points == null || points.Count < 3)
            return new List<Vector2>(points);

        return DouglasPeucker(points, tolerance);
    }

    private List<Vector2> DouglasPeucker(List<Vector2> points, float tolerance)
    {
        if (points.Count < 3)
            return new List<Vector2>(points);

        int index = -1;
        float maxDist = 0f;

        for (int i = 1; i < points.Count - 1; i++)
        {
            float dist = PerpendicularDistance(points[i], points[0], points[points.Count - 1]);
            if (dist > maxDist)
            {
                maxDist = dist;
                index = i;
            }
        }

        if (maxDist > tolerance)
        {
            List<Vector2> left = DouglasPeucker(points.GetRange(0, index + 1), tolerance);
            List<Vector2> right = DouglasPeucker(points.GetRange(index, points.Count - index), tolerance);
            left.RemoveAt(left.Count - 1);
            left.AddRange(right);
            return left;
        }
        else
        {
            return new List<Vector2> { points[0], points[points.Count - 1] };
        }
    }

    private float PerpendicularDistance(Vector2 pt, Vector2 lineStart, Vector2 lineEnd)
    {
        float dx = lineEnd.x - lineStart.x;
        float dy = lineEnd.y - lineStart.y;

        if (dx == 0f && dy == 0f)
            return Vector2.Distance(pt, lineStart);

        float t = ((pt.x - lineStart.x) * dx + (pt.y - lineStart.y) * dy) / (dx * dx + dy * dy);
        Vector2 projection = new Vector2(lineStart.x + t * dx, lineStart.y + t * dy);
        return Vector2.Distance(pt, projection);
    }




    void RunConsoleCommand(string cmd)
    {
        logHistory.Add("> " + cmd);

        if (cmd.StartsWith(">help")) { /* Show help */ }
        else if (cmd.StartsWith(">spawn player")) SpawnPlayerOutsidePolygon();
        else if (cmd.StartsWith(">export json")) ExportMapData();
        else if (cmd.StartsWith(">clear poly")) currentPolygon.Clear();
        else if (cmd.StartsWith(">color red")) ChangeColor(Color.red);
        else if (cmd.StartsWith(">zoom "))
        {
            if (float.TryParse(cmd.Substring(6), out float zoomVal))
                Camera.main.orthographicSize = zoomVal;
        }
        else logHistory.Add("Commande inconnue !");
    }

    void ChangeColor(Color newColor)
    {
        polygonColor = newColor;
        Debug.Log("🎨 Couleur changée en : " + newColor);
    }
    private void LoadMapFromPath(string path)
    {
        if (File.Exists(path))
        {
            string json = File.ReadAllText(path);
            PolygonData data = JsonUtility.FromJson<PolygonData>(json);
            currentPolygon = new List<Vector2>(data.points);

            string mapName = Path.GetFileNameWithoutExtension(path);
            MapBackground bg = FindObjectOfType<MapBackground>();
            if (bg != null)
            {
                bg.LoadMapSprite(mapName);
            }

            Debug.Log("📂 Map chargée depuis : " + path);
        }
        else
        {
            Debug.LogWarning("❌ Fichier de map introuvable : " + path);
        }
    }
}