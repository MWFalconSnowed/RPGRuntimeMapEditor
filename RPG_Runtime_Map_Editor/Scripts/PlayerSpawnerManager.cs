using System.Collections.Generic;
using UnityEngine;

public class PlayerSpawnerManager : MonoBehaviour
{
    public GameObject playerPrefab;
    public PolygonMetaTagger metaTagger;

    public void SpawnInPolygon(string tag)
    {
        var polygon = metaTagger.GetPolygon(tag);
        if (polygon == null || polygon.Count < 3)
        {
            Debug.LogWarning("❌ Polygone introuvable pour le tag : " + tag);
            return;
        }

        Vector2 spawn;
        int tries = 0;
        do
        {
            spawn = new Vector2(Random.Range(-5f, 5f), Random.Range(-5f, 5f));
            tries++;
            if (tries > 100) return;
        } while (!IsInsidePolygon(spawn, polygon));

        Instantiate(playerPrefab, spawn, Quaternion.identity);
        Debug.Log("🎮 Spawn dans la zone : " + tag);
    }

    bool IsInsidePolygon(Vector2 point, List<Vector2> poly)
    {
        bool inside = false;
        for (int i = 0, j = poly.Count - 1; i < poly.Count; j = i++)
        {
            if ((poly[i].y > point.y) != (poly[j].y > point.y) &&
                point.x < (poly[j].x - poly[i].x) * (point.y - poly[i].y) / (poly[j].y - poly[i].y) + poly[i].x)
                inside = !inside;
        }
        return inside;
    }
}
