using UnityEngine;

public class PlayerSpawner : MonoBehaviour
{
    public GameObject playerPrefab;
    public PolygonCollider2D polygonCollider;

    public void SpawnPlayerOutsidePolygon()
    {
        if (playerPrefab == null || polygonCollider == null || polygonCollider.points.Length < 3)
        {
            Debug.LogWarning("❌ Préfab ou PolygonCollider invalide.");
            return;
        }

        // Essayons de spawner en bordure ou à une position fixe hors polygon
        Vector2 spawnPos = new Vector2(-5f, -5f); // 👈 modifie cette position selon ton besoin

        // Optionnel : vérifie si le point est à l’extérieur
        if (polygonCollider.OverlapPoint(spawnPos))
        {
            Debug.LogWarning("❌ La position de spawn est DANS le polygone, essaie une autre position.");
            return;
        }

        Instantiate(playerPrefab, spawnPos, Quaternion.identity);
        Debug.Log("✅ Joueur spawné à " + spawnPos);
    }
}
