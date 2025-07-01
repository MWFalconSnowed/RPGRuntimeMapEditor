// PolygonColliderGenerator.cs - Génère des PolygonCollider2D depuis polygons.txt
using UnityEngine;
using UnityEditor;
using System.Collections.Generic;
using System.IO;

public class PolygonColliderGenerator : MonoBehaviour
{
    public string path = "Assets/Resources/polygons.txt";
    public GameObject colliderParent;

    [ContextMenu("?? Générer les colliders depuis le fichier")]
    public void GenerateColliders()
    {
        if (!File.Exists(path))
        {
            Debug.LogError("? Fichier de polygones introuvable : " + path);
            return;
        }

        // Supprimer les anciens colliders si existants
        foreach (Transform child in colliderParent.transform)
        {
            DestroyImmediate(child.gameObject);
        }

        string[] lines = File.ReadAllLines(path);
        int polyIndex = 0;

        foreach (var line in lines)
        {
            string[] points = line.Split('|');
            List<Vector2> poly = new List<Vector2>();

            foreach (var pt in points)
            {
                string[] xy = pt.Split(',');
                if (xy.Length == 2 && float.TryParse(xy[0], out float x) && float.TryParse(xy[1], out float y))
                    poly.Add(new Vector2(x, y));
            }

            if (poly.Count >= 3)
            {
                GameObject go = new GameObject("PolygonCollider_" + polyIndex);
                go.transform.parent = colliderParent.transform;

                PolygonCollider2D pc = go.AddComponent<PolygonCollider2D>();
                pc.points = poly.ToArray();
                pc.isTrigger = true;
                polyIndex++;
            }
        }

        Debug.Log("? Colliders générés : " + polyIndex);
    }
}
