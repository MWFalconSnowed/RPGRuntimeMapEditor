using System.Collections.Generic;
using UnityEngine;

public class PolygonAnalyzer : MonoBehaviour
{
    public List<Vector2> polygon;

    public float CalculateArea()
    {
        float area = 0f;
        int n = polygon.Count;
        for (int i = 0; i < n; i++)
        {
            Vector2 p1 = polygon[i];
            Vector2 p2 = polygon[(i + 1) % n];
            area += (p1.x * p2.y) - (p2.x * p1.y);
        }
        area = Mathf.Abs(area) * 0.5f;
        Debug.Log("📏 Aire du polygone : " + area);
        return area;
    }

    public float CalculatePerimeter()
    {
        float length = 0f;
        int n = polygon.Count;
        for (int i = 0; i < n; i++)
        {
            length += Vector2.Distance(polygon[i], polygon[(i + 1) % n]);
        }
        Debug.Log("📐 Périmètre du polygone : " + length);
        return length;
    }

    public Vector2 CalculateCentroid()
    {
        Vector2 sum = Vector2.zero;
        foreach (var pt in polygon)
            sum += pt;
        Vector2 centroid = sum / polygon.Count;
        Debug.Log("📍 Centre géométrique : " + centroid);
        return centroid;
    }
}
