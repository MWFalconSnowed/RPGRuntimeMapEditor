// PolygonColliderGizmos.cs - Affiche les PolygonCollider2D avec Gizmos dans la scène
using UnityEngine;

[ExecuteInEditMode]
public class PolygonColliderGizmos : MonoBehaviour
{
    public Color colliderColor = new Color(0f, 1f, 1f, 0.4f);

    private void OnDrawGizmos()
    {
        Gizmos.color = colliderColor;

        PolygonCollider2D[] colliders = GetComponentsInChildren<PolygonCollider2D>();
        foreach (PolygonCollider2D col in colliders)
        {
            if (col != null && col.points.Length >= 3)
            {
                Vector3 offset = col.transform.position;
                Vector2[] points = col.points;

                for (int i = 0; i < points.Length; i++)
                {
                    Vector3 a = (Vector3)points[i] + offset;
                    Vector3 b = (Vector3)points[(i + 1) % points.Length] + offset;
                    Gizmos.DrawLine(a, b);
                }
            }
        }
    }
}
