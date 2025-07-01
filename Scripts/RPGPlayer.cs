using UnityEngine;

public class RPGPlayer : MonoBehaviour
{
    public int health;
    public int mana;
    public int attack;
    public int defense;

    private void OnGUI()
    {
        Vector3 screenPos = Camera.main.WorldToScreenPoint(transform.position);
        GUI.Label(new Rect(screenPos.x - 40, Screen.height - screenPos.y - 60, 120, 20),
            $"❤️ {health} | ✨ {mana}");
    }

    private void Update()
    {
        float h = Input.GetAxisRaw("Horizontal");
        float v = Input.GetAxisRaw("Vertical");
        transform.position += new Vector3(h, v, 0) * Time.deltaTime * 5f;
    }
}
