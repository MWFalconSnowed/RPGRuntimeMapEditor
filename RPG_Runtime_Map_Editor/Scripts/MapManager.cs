using UnityEngine;

public class MapManager : MonoBehaviour
{
    public GameObject tilePrefab;  // Préfabriqué pour les tuiles
    public int mapWidth = 31;  // 1980 / 64 = ~31 tuiles
    public int mapHeight = 17; // 1080 / 64 = ~17 tuiles

    private Tile[,] tiles;  // Matrice pour les tuiles

    private void Start()
    {
        GenerateMap();
    }

    void Update()
    {
        // Permet de placer des tuiles à la souris
        if (Input.GetMouseButtonDown(0))  // Clique gauche pour placer une tuile
        {
            Vector3 worldPos = Camera.main.ScreenToWorldPoint(Input.mousePosition);
            PlaceTile(worldPos);
        }
    }

    // Générer la carte
    void GenerateMap()
    {
        tiles = new Tile[mapWidth, mapHeight];

        for (int x = 0; x < mapWidth; x++)
        {
            for (int y = 0; y < mapHeight; y++)
            {
                Vector3 position = new Vector3(x, y, 0);
                GameObject tileObj = Instantiate(tilePrefab, position, Quaternion.identity);
                Tile tile = tileObj.GetComponent<Tile>();
                tiles[x, y] = tile;
            }
        }
    }

    // Placer une tuile à une position donnée
    void PlaceTile(Vector3 position)
    {
        int x = Mathf.FloorToInt(position.x);
        int y = Mathf.FloorToInt(position.y);

        // Vérifier si la position est dans la grille de la carte
        if (x >= 0 && x < mapWidth && y >= 0 && y < mapHeight)
        {
            Tile tile = tiles[x, y];
            tile.ChangeTile(Random.Range(0, tile.tileSprites.Length)); // Change la tuile à une tuile aléatoire
        }
    }
}
