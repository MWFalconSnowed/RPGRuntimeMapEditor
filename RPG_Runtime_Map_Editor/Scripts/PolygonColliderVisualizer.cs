using UnityEngine;

[ExecuteAlways]
[RequireComponent(typeof(PolygonCollider2D))]
public class PolygonColliderVisualizer : MonoBehaviour
{
    private PolygonCollider2D polygon;

    void OnDrawGizmos()
    {
        polygon = GetComponent<PolygonCollider2D>();
        if (polygon == null || polygon.pathCount == 0) return;

        Gizmos.color = Color.cyan;

        for (int i = 0; i < polygon.pathCount; i++)
        {
            Vector2[] points = polygon.GetPath(i);
            for (int j = 0; j < points.Length; j++)
            {
                Vector2 current = transform.TransformPoint(points[j]);
                Vector2 next = transform.TransformPoint(points[(j + 1) % points.Length]);
                Gizmos.DrawLine(current, next);
            }
        }
    }
}
