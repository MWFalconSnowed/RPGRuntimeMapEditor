using UnityEngine;

public class PlayerPanelRuntime : MonoBehaviour
{
    public GameObject playerPrefab;
    private GameObject currentPlayer;
    private Vector2 spawnPosition = new Vector2(10, 10);

    private int health = 100;
    private int mana = 50;
    private int attack = 15;
    private int defense = 5;

    private GUIStyle boxStyle;
    private GUIStyle buttonStyle;
    private GUIStyle labelStyle;
    private Texture2D backgroundTex;

    void OnGUI()
    {
        if (boxStyle == null) InitStyles();

        GUI.color = Color.white;
        GUI.backgroundColor = new Color(0f, 0f, 0f, 0.6f);

        GUILayout.BeginArea(new Rect(Screen.width - 250, 20, 230, 300), boxStyle);
        GUILayout.Label("👤 Player Panel", labelStyle);

        GUILayout.Label("❤️ Santé: " + health);
        GUILayout.Label("✨ Mana: " + mana);
        GUILayout.Label("⚔️ Attaque: " + attack);
        GUILayout.Label("🛡️ Défense: " + defense);

        GUILayout.Space(10);
        if (GUILayout.Button("Spawn Player", buttonStyle, GUILayout.Height(40)))
        {
            SpawnPlayer();
        }

        GUILayout.EndArea();
    }

    void SpawnPlayer()
    {
        if (currentPlayer != null) Destroy(currentPlayer);

        currentPlayer = Instantiate(playerPrefab, spawnPosition, Quaternion.identity);
        Camera.main.transform.position = new Vector3(spawnPosition.x, spawnPosition.y, -10);

        var rpg = currentPlayer.GetComponent<RPGPlayer>();
        if (rpg != null)
        {
            rpg.health = health;
            rpg.mana = mana;
            rpg.attack = attack;
            rpg.defense = defense;
        }
    }

    void InitStyles()
    {
        if (backgroundTex == null)
        {
            backgroundTex = new Texture2D(1, 1);
            backgroundTex.SetPixel(0, 0, new Color(0.1f, 0.1f, 0.1f, 0.7f));
            backgroundTex.Apply();
        }

        boxStyle = new GUIStyle(GUI.skin.box);
        boxStyle.normal.background = backgroundTex;
        boxStyle.padding = new RectOffset(10, 10, 10, 10);

        labelStyle = new GUIStyle(GUI.skin.label);
        labelStyle.normal.textColor = Color.white;
        labelStyle.fontStyle = FontStyle.Bold;

        buttonStyle = new GUIStyle(GUI.skin.button);
        buttonStyle.fontSize = 14;
        buttonStyle.normal.textColor = Color.white;
    }
}
