
using UnityEngine;

public class RPGPlayer : MonoBehaviour
{
    public string playerName = "Hero";
    public int hp = 100;
    public int mp = 50;
    public int level = 1;
    public int strength = 10;
    public int defense = 5;

    private Rigidbody2D rb;
    public float moveSpeed = 6f; // Plus rapide

    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        if (rb == null)
        {
            rb = gameObject.AddComponent<Rigidbody2D>();
        }
        rb.gravityScale = 0f;
        rb.constraints = RigidbodyConstraints2D.FreezeRotation;

        if (GetComponent<BoxCollider2D>() == null && GetComponent<CircleCollider2D>() == null)
        {
            gameObject.AddComponent<CircleCollider2D>();
        }

        Debug.Log("Player spawned: " + playerName);
    }

    void FixedUpdate()
    {
        float h = Input.GetAxisRaw("Horizontal");
        float v = Input.GetAxisRaw("Vertical");
        Vector2 move = new Vector2(h, v).normalized;
        rb.MovePosition(rb.position + move * moveSpeed * Time.fixedDeltaTime);
    }
}
