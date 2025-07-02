using System.Collections.Generic;
using UnityEngine;

public class PolygonExtendedFeatures : MonoBehaviour
{
    public List<Vector2> polygonReference;

    public void CenterPolygon()
    {
        if (polygonReference == null || polygonReference.Count == 0) return;

        Vector2 centroid = Vector2.zero;
        foreach (Vector2 point in polygonReference)
        {
            centroid += point;
        }
        centroid /= polygonReference.Count;

        for (int i = 0; i < polygonReference.Count; i++)
        {
            polygonReference[i] -= centroid;
        }

        Debug.Log("🎯 Polygone recentré autour du (0,0)");
    }

    public void ScalePolygon(float scale)
    {
        for (int i = 0; i < polygonReference.Count; i++)
        {
            polygonReference[i] *= scale;
        }

        Debug.Log("📐 Polygone mis à l’échelle x" + scale);
    }

    public void RotatePolygon(float degrees)
    {
        float rad = degrees * Mathf.Deg2Rad;
        for (int i = 0; i < polygonReference.Count; i++)
        {
            float x = polygonReference[i].x;
            float y = polygonReference[i].y;
            polygonReference[i] = new Vector2(
                x * Mathf.Cos(rad) - y * Mathf.Sin(rad),
                x * Mathf.Sin(rad) + y * Mathf.Cos(rad)
            );
        }

        Debug.Log("🔄 Polygone pivoté de " + degrees + "°");
    }

    public void JitterPolygon(float intensity)
    {
        for (int i = 0; i < polygonReference.Count; i++)
        {
            polygonReference[i] += Random.insideUnitCircle * intensity;
        }

        Debug.Log("🌪️ Points perturbés aléatoirement");
    }
}
