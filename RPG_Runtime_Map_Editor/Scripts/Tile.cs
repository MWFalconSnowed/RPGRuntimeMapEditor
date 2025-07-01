using UnityEngine;

public enum TileType
{
    Grass,
    Ruins,
    Water,
    Portal,
    Path,
    GlowingFlower,
    MossStone
}

public enum TileLayer
{
    Base,
    Decoration,
    Interaction
}

[RequireComponent(typeof(SpriteRenderer))]
public class Tile : MonoBehaviour
{
    public Sprite[] tileSprites; // Assigné dans l'inspecteur
    public TileType tileType;
    public TileLayer tileLayer;
    public int tileIndex;

    private SpriteRenderer spriteRenderer;

    void Awake()
    {
        spriteRenderer = GetComponent<SpriteRenderer>();
    }

    public void ChangeTile(int index)
    {
        if (index >= 0 && index < tileSprites.Length)
        {
            tileIndex = index;
            tileType = (TileType)index;
            spriteRenderer.sprite = tileSprites[index];
        }
    }

    public void SetLayer(TileLayer layer)
    {
        tileLayer = layer;
        switch (layer)
        {
            case TileLayer.Base:
                spriteRenderer.sortingOrder = 0;
                break;
            case TileLayer.Decoration:
                spriteRenderer.sortingOrder = 1;
                break;
            case TileLayer.Interaction:
                spriteRenderer.sortingOrder = 2;
                break;
        }
    }
}
