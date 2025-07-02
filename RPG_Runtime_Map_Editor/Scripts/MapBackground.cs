using UnityEngine;

[RequireComponent(typeof(SpriteRenderer))]
public class MapBackground : MonoBehaviour
{
    public Sprite mapImage;

    void Start()
    {
        if (mapImage != null)
        {
            var sr = GetComponent<SpriteRenderer>();
            sr.sprite = mapImage;

            // On force l'échelle pour correspondre à 1980x1080
            float pixelsPerUnit = mapImage.pixelsPerUnit; // normalement 100
            float unitWidth = mapImage.texture.width / pixelsPerUnit;
            float unitHeight = mapImage.texture.height / pixelsPerUnit;

            transform.localScale = new Vector3(1, 1, 1); // on garde l'échelle à 1 (pas besoin d’adapter)

            // Centrage optionnel si nécessaire
            transform.position = new Vector3(unitWidth / 2f, unitHeight / 2f, 0f);
        }
        else
        {
            Debug.LogError("🧱 Aucun sprite de map assigné !");
        }
    }
}
