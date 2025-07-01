using UnityEngine;

public class TileManager : MonoBehaviour
{
    public GameObject tilePrefab;
    public int mapWidth = 31;  // 1980 / 64
    public int mapHeight = 17; // 1080 / 64
    public float tileSize = 1.0f;

    private Tile[,] tiles;

    void Start()
    {
        GenerateMap();
    }

    void GenerateMap()
    {
        tiles = new Tile[mapWidth, mapHeight];

        for (int x = 0; x < mapWidth; x++)
        {
            for (int y = 0; y < mapHeight; y++)
            {
                Vector3 position = new Vector3(x * tileSize, y * tileSize, 0);
                GameObject tileObj = Instantiate(tilePrefab, position, Quaternion.identity, transform);
                Tile tile = tileObj.GetComponent<Tile>();
                tile.ChangeTile(Random.Range(0, tile.tileSprites.Length)); // assignation aléatoire
                tile.SetLayer(TileLayer.Base);
                tiles[x, y] = tile;
            }
        }

        CenterCamera();
    }

    void CenterCamera()
    {
        Camera.main.orthographic = true;
        Camera.main.orthographicSize = mapHeight / 2f;
        Camera.main.transform.position = new Vector3(mapWidth / 2f, mapHeight / 2f, -10);
    }
}
